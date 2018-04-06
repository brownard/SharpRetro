using SharpDX;
using SharpDX.Direct3D9;
using SharpGL;
using SharpGL.RenderContextProviders;
using SharpGL.Version;
using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Environment;
using System;
using System.Runtime.InteropServices;

namespace SharpRetro.DirectX.GL
{
  public class FBORenderContextProvider : HiddenWindowRenderContextProvider, IRenderContext
  {
    [DllImport("opengl32", EntryPoint = "wglGetProcAddress", ExactSpelling = true)]
    private static extern IntPtr wglGetProcAddress(IntPtr function_name);

    protected retro_hw_context_reset_t _contextReset;
    protected retro_hw_context_reset_t _contextDestroy;
    protected OpenGL gl;
    protected Geometry _geometry;
    protected bool _bottomLeftOrigin;

    protected Texture2d _colourBuffer;
    protected Renderbuffer _depthBuffer;
    protected Framebuffer _framebuffer;

    protected Framebuffer _outputBuffer;
    protected Texture2d _outputTexture;

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
      this.gl = gl;
      CreateFramebuffers(width, height);
      _contextReset?.Invoke();
      return true;
    }

    public virtual void Init(retro_hw_render_callback renderCallback)
    {
      _bottomLeftOrigin = renderCallback.bottom_left_origin;
      if (renderCallback.context_reset != IntPtr.Zero)
        _contextReset = Marshal.GetDelegateForFunctionPointer<retro_hw_context_reset_t>(renderCallback.context_reset);
      if (renderCallback.context_destroy != IntPtr.Zero)
        _contextDestroy = Marshal.GetDelegateForFunctionPointer<retro_hw_context_reset_t>(renderCallback.context_destroy);
    }

    public void SetGeometry(Geometry geometry)
    {
      _geometry = geometry;
      if (gl == null)
        Create(OpenGLVersion.OpenGL2_1, new OpenGL(), geometry.MaxWidth, geometry.MaxHeight, 32, null);
      else
        SetDimensions(geometry.MaxWidth, geometry.MaxHeight);
    }

    public uint GetCurrentFramebuffer()
    {
      return _framebuffer.Id;
    }

    public IntPtr GetProcAddress(IntPtr sym)
    {
      return wglGetProcAddress(sym);
    }

    public virtual bool OnFramebufferReady(Texture texture)
    {
      DataRectangle rectangle = texture.LockRectangle(0, LockFlags.Discard);
      try
      {
        Scene.RenderQuad(gl, _outputBuffer, _colourBuffer, width, height, _bottomLeftOrigin);

        _outputBuffer.Bind();
        gl.ReadBuffer(OpenGL.GL_COLOR_ATTACHMENT0_EXT);
        gl.ReadPixels(0, 0, width, height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, rectangle.DataPointer);
      }
      finally
      {
        _outputBuffer.UnBind();
        texture.UnlockRectangle(0);
      }
      return true;
    }

    public bool ReadPixels(byte[] buffer, int width, int height)
    {
      if (deviceContextHandle == IntPtr.Zero)
        return false;
      //  Set the read buffer.
      _framebuffer.Bind();
      gl.ReadBuffer(OpenGL.GL_COLOR_ATTACHMENT0_EXT);
      gl.ReadPixels(0, 0, width, height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, buffer);
      _framebuffer.UnBind();
      return true;
    }

    protected virtual void CreateFramebuffers(int width, int height)
    {
      _framebuffer = new Framebuffer(gl);
      _framebuffer.Create();
      _framebuffer.Bind();
      _colourBuffer = CreateColourTexture(width, height);
      _depthBuffer = CreateDepthBuffer(width, height);
      _framebuffer.UnBind();

      _outputBuffer = new Framebuffer(gl);
      _outputBuffer.Create();
      _outputBuffer.Bind();
      _outputTexture = CreateColourTexture(width, height);
      _outputBuffer.UnBind();
    }

    protected Texture2d CreateColourTexture(int width, int height)
    {
      Texture2d texture = new Texture2d(gl);
      texture.Create();
      texture.Bind();
      texture.AddStorage(OpenGL.GL_RGBA, width, height);
      _framebuffer.AddTexture(OpenGL.GL_COLOR_ATTACHMENT0_EXT, texture);
      texture.UnBind();
      return texture;
    }

    protected Renderbuffer CreateDepthBuffer(int width, int height)
    {
      Renderbuffer depthBuffer = new Renderbuffer(gl);
      depthBuffer.Create();
      depthBuffer.Bind();
      depthBuffer.AddStorage(OpenGL.GL_DEPTH_COMPONENT24, width, height);
      _framebuffer.AddRenderbuffer(OpenGL.GL_DEPTH_ATTACHMENT_EXT, depthBuffer);
      depthBuffer.UnBind();
      return depthBuffer;
    }

    protected virtual void DestroyFramebuffers()
    {
      _colourBuffer?.Dispose();
      _colourBuffer = null;

      _depthBuffer?.Dispose();
      _depthBuffer = null;

      _framebuffer?.Dispose();
      _framebuffer = null;

      _outputTexture?.Dispose();
      _outputTexture = null;

      _outputBuffer?.Dispose();
      _outputBuffer = null;
    }

    public override void SetDimensions(int width, int height)
    {
      //  Call the base.
      base.SetDimensions(width, height);
      DestroyFramebuffers();
      CreateFramebuffers(width, height);
      _contextReset?.Invoke();
    }

    public override void Destroy()
    {
      //  Delete the render buffers.
      DestroyFramebuffers();      
      //	Call the base, which will delete the render context handle and window.
      base.Destroy();
    }
  }
}