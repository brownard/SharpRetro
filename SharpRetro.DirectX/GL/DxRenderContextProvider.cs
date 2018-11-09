using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;
using SharpGL;
using SharpRetro.DirectX.Video;
using System;

namespace SharpRetro.DirectX.GL
{
  public class DXRenderContextProvider : FBORenderContextProvider
  {
    protected Device _device;
    protected IntPtr _glDeviceHandle;

    protected OpenGLEx _glEx;
    protected bool _hasDXExtensions;
    protected Framebuffer _frontBuffer;
    protected SharedTexture _sharedTexture;
    protected Texture _renderTexture;

    public DXRenderContextProvider(Device device, OpenGLEx glEx, bool depth, bool stencil, bool bottomLeftOrigin)
      : base(glEx, depth, stencil, bottomLeftOrigin)
    {
      _device = device;
      _glEx = glEx;
    }

    public void Render(ID3DContext textureProvider, int width, int height, int pitch)
    {
      if (_sharedTexture != null)
        RenderSharedTexture(textureProvider, width, height);
      else
        RenderBytes(textureProvider, width, height);
    }

    protected void RenderSharedTexture(ID3DContext textureProvider, int width, int height)
    {
      //Render the framebuffer to our front buffer, flipping the image if _bottomLeftOrigin is true
      _sharedTexture.Lock();
      Scene.RenderQuad(_gl, _frontBuffer, _framebuffer.GetAttachment<Texture2d>(OpenGL.GL_COLOR_ATTACHMENT0_EXT), width, height, _bottomLeftOrigin);
      _sharedTexture.Unlock();

      //Get a render target texture from the provider and copy our shared texture to it 
      Texture texture = textureProvider.GetTexture(width, height, Usage.RenderTarget);
      _device.StretchRectangle(_renderTexture.GetSurfaceLevel(0), new RawRectangle(0, 0, width, height), texture.GetSurfaceLevel(0), null, TextureFilter.None);
    }

    protected void RenderBytes(ID3DContext textureProvider, int width, int height)
    {
      //Render the framebuffer to our front buffer, flipping the image if _bottomLeftOrigin is true
      Scene.RenderQuad(_gl, _frontBuffer, _framebuffer.GetAttachment<Texture2d>(OpenGL.GL_COLOR_ATTACHMENT0_EXT), width, height, _bottomLeftOrigin);

      //Get a dynamic texture from the provider and copy the frame buffer pixels to it 
      Texture texture = textureProvider.GetTexture(width, height, Usage.RenderTarget);
      DataRectangle rectangle = texture.LockRectangle(0, LockFlags.Discard);
      ReadPixels(_frontBuffer, rectangle.DataPointer, width, height);
      texture.UnlockRectangle(0);
    }

    protected override void CreateFramebuffer()
    {      
      base.CreateFramebuffer();
      _frontBuffer = new Framebuffer(_gl);
      _frontBuffer.Create();
      _frontBuffer.Bind();

      if (CreateSharedContext())
        CreateSharedTexture(_frontBuffer);
      else
        CreateTexture(_frontBuffer);

      _frontBuffer.UnBind();
    }

    protected bool CreateSharedContext()
    {
      _hasDXExtensions = _glEx.HasDXExtensions();
      if (!_hasDXExtensions)
        return false;
      _glDeviceHandle = _glEx.DXOpenDeviceNV(_device.NativePointer);
      return true;
    }

    /// <summary>
    /// Creates an OpenGL texture that is backed by a DirectX texture and adds it
    /// to the specified framebuffer. 
    /// </summary>
    /// <param name="framebuffer"></param>
    protected void CreateSharedTexture(Framebuffer framebuffer)
    {
      //Create the shared texture and register it with the gl context
      IntPtr sharedResourceHandle = IntPtr.Zero;
      _renderTexture = new Texture(_device, width, height, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default, ref sharedResourceHandle);
      bool result = _glEx.DXSetResourceShareHandleNV(_renderTexture.NativePointer, sharedResourceHandle);

      //Create a gl texture and add the shared tecture to it 
      _sharedTexture = new SharedTexture(_glEx, _glDeviceHandle);
      _sharedTexture.Create();
      _sharedTexture.AddSharedTexture(_renderTexture.NativePointer, sharedResourceHandle);

      //Add the gl texture to the framebuffer
      _sharedTexture.Lock();
      framebuffer.AddTexture(OpenGL.GL_COLOR_ATTACHMENT0_EXT, _sharedTexture);
      _sharedTexture.Unlock();
    }

    protected override void DestroyFramebuffers()
    {
      base.DestroyFramebuffers();
      if (_frontBuffer != null)
      {
        foreach (AbstractBuffer attachment in _frontBuffer.Attachments)
          attachment.Dispose();
        _frontBuffer.Dispose();
        _frontBuffer = null;
        _sharedTexture = null;
      }
      if (_renderTexture != null)
      {
        _renderTexture.Dispose();
        _renderTexture = null;
      }
      if (_glDeviceHandle != IntPtr.Zero)
      {
        _glEx.DXCloseDeviceNV(_glDeviceHandle);
        _glDeviceHandle = IntPtr.Zero;
      }
    }
  }
}
