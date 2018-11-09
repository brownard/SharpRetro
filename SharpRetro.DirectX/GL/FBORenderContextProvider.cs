using SharpGL;
using SharpGL.RenderContextProviders;
using SharpGL.Version;
using System;
using System.Runtime.InteropServices;

namespace SharpRetro.DirectX.GL
{
  /// <summary>
  /// Provides an OpenGL context that can be used by a libretro core. 
  /// </summary>
  public class FBORenderContextProvider : HiddenWindowRenderContextProvider
  {
    [DllImport("opengl32", EntryPoint = "wglGetProcAddress", ExactSpelling = true)]
    private static extern IntPtr wglGetProcAddress(IntPtr function_name);

    protected OpenGL _gl;
    protected bool _created;

    protected bool _depth;
    protected bool _stencil;
    protected bool _bottomLeftOrigin;

    protected Framebuffer _framebuffer;

    public FBORenderContextProvider(OpenGL gl, bool depth, bool stencil, bool bottomLeftOrigin)
    {
      _gl = gl;
      _depth = depth;
      _stencil = stencil;
      _bottomLeftOrigin = bottomLeftOrigin;
    }

    /// <summary>
    /// Creates the render context provider. Must also create the OpenGL extensions.
    /// </summary>
    /// <param name="openGLVersion">The desired OpenGL version.</param>
    /// <param name="gl">The OpenGL context.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="bitDepth">The bit depth.</param>
    /// <param name="parameter">The parameter</param>
    /// <returns></returns>
    public override bool Create(OpenGLVersion openGLVersion, OpenGL gl, int width, int height, int bitDepth, object parameter)
    {
      if (!base.Create(openGLVersion, gl, width, height, bitDepth, parameter))
        return false;
      CreateFramebuffer();
      _created = true;
      return true;
    }

    public bool Created
    {
      get { return _created; }
    }

    public uint GetCurrentFramebuffer()
    {
      return _framebuffer.Id;
    }

    public IntPtr GetProcAddress(IntPtr sym)
    {
      return wglGetProcAddress(sym);
    }

    public override void SetDimensions(int width, int height)
    {
      //  Call the base.
      base.SetDimensions(width, height);
      DestroyFramebuffers();
      CreateFramebuffer();
    }

    protected bool ReadPixels(Framebuffer framebuffer, IntPtr buffer, int width, int height)
    {
      if (deviceContextHandle == IntPtr.Zero)
        return false;
      //  Set the read buffer.
      framebuffer.Bind();
      _gl.ReadBuffer(OpenGL.GL_COLOR_ATTACHMENT0_EXT);
      _gl.ReadPixels(0, 0, width, height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, buffer);
      framebuffer.UnBind();
      return true;
    }

    protected virtual void CreateFramebuffer()
    {
      _framebuffer = new Framebuffer(_gl);
      _framebuffer.Create();
      _framebuffer.Bind();
      CreateTexture(_framebuffer);
      CreateDepthBuffer(_framebuffer);
      _framebuffer.UnBind();
    }

    protected virtual void DestroyFramebuffers()
    {
      if (_framebuffer != null)
      {
        foreach (AbstractBuffer attachment in _framebuffer.Attachments)
          attachment.Dispose();
        _framebuffer.Dispose();
        _framebuffer = null;
      }
    }

    protected void CreateTexture(Framebuffer framebuffer)
    {
      Texture2d texture = new Texture2d(_gl);
      texture.Create();
      texture.Bind();
      texture.AddStorage(OpenGL.GL_RGBA, width, height);
      framebuffer.AddTexture(OpenGL.GL_COLOR_ATTACHMENT0_EXT, texture);
      texture.UnBind();
    }

    protected void CreateDepthBuffer(Framebuffer framebuffer)
    {
      Renderbuffer depthBuffer = new Renderbuffer(_gl);
      depthBuffer.Create();
      depthBuffer.Bind();
      depthBuffer.AddStorage(OpenGL.GL_DEPTH_COMPONENT24, width, height);
      framebuffer.AddRenderbuffer(OpenGL.GL_DEPTH_ATTACHMENT_EXT, depthBuffer);
      depthBuffer.UnBind();
    }

    public override void Destroy()
    {
      //  Delete the render buffers.
      DestroyFramebuffers();
      _created = false;
      //	Call the base, which will delete the render context handle and window.
      base.Destroy();
    }
  }
}