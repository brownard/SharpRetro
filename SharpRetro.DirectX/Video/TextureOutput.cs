using SharpDX;
using SharpDX.Direct3D9;
using SharpRetro.DirectX.GL;
using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Environment;
using SharpRetro.Libretro.Video;
using System;
using System.Runtime.InteropServices;

namespace SharpRetro.DirectX.Video
{
  public class TextureOutput : IVideoOutput
  {
    protected ITextureProvider _textureProvider;
    protected IRenderContext _renderContext;
    protected RETRO_PIXEL_FORMAT _pixelFormat = RETRO_PIXEL_FORMAT.XRGB1555; //Default libretro pixel format
    protected Geometry _geometry;
    protected Timing _timing;

    protected retro_hw_get_current_framebuffer_t _getCurrentFramebufferDlgt;
    protected retro_hw_get_proc_address_t _getProcAddressDlgt;

    public TextureOutput()
    {
      _getCurrentFramebufferDlgt = new retro_hw_get_current_framebuffer_t(GetCurrentFrameBuffer);
      _getProcAddressDlgt = new retro_hw_get_proc_address_t(GetProcAddress);
    }

    public TextureOutput(ITextureProvider textureProvider)
      : this()
    {
      _textureProvider = textureProvider;
      _renderContext = new FBORenderContextProvider();
    }

    public bool SetPixelFormat(RETRO_PIXEL_FORMAT pixelFormat)
    {
      switch (pixelFormat)
      {
        case RETRO_PIXEL_FORMAT.RGB565:
        case RETRO_PIXEL_FORMAT.XRGB1555:
        case RETRO_PIXEL_FORMAT.XRGB8888:
          _pixelFormat = pixelFormat;
          return true;
        default:
          return false;
      }
    }

    public void SetGeometry(Geometry geometry)
    {
      _geometry = geometry;
      _renderContext?.SetGeometry(geometry);
    }

    public void SetTiming(Timing timing)
    {
      _timing = timing;
    }

    public void OnFramebufferReady(int width, int height, int pitch)
    {
      Texture texture = _textureProvider?.GetTexture(width, height, pitch);
      if (texture != null)
        _renderContext?.OnFramebufferReady(texture);
    }

    public void OnVideoRefresh(IntPtr data, int width, int height, int pitch)
    {
      Texture texture = _textureProvider?.GetTexture(width, height, pitch);
      if (texture != null)
        WriteToTexture(texture, data, width, height, pitch);
    }

    public bool TrySetHardwareRenderer(ref retro_hw_render_callback renderCallback)
    {
      if (_renderContext == null)
        return false;

      _renderContext.Init(renderCallback);
      renderCallback.get_current_framebuffer = Marshal.GetFunctionPointerForDelegate(_getCurrentFramebufferDlgt);
      renderCallback.get_proc_address = Marshal.GetFunctionPointerForDelegate(_getProcAddressDlgt);
      return true;
    }

    protected void WriteToTexture(Texture texture, IntPtr data, int width, int height, int pitch)
    {
      DataRectangle rectangle = texture.LockRectangle(0, LockFlags.Discard);
      try
      {
        VideoBlitter.Blit(_pixelFormat, data, rectangle.DataPointer, width, height, pitch, rectangle.Pitch);
      }
      finally
      {
        texture.UnlockRectangle(0);
      }
    }

    protected uint GetCurrentFrameBuffer()
    {
      return _renderContext != null ? _renderContext.GetCurrentFramebuffer() : 0;
    }

    protected IntPtr GetProcAddress(IntPtr sym)
    {
      return _renderContext != null ? _renderContext.GetProcAddress(sym) : IntPtr.Zero;
    }
  }
}
