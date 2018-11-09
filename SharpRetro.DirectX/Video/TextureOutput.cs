using SharpDX;
using SharpDX.Direct3D9;
using SharpRetro.DirectX.GL;
using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Environment;
using SharpRetro.Libretro.Video;
using System;

namespace SharpRetro.DirectX.Video
{
  public class TextureOutput : IVideoOutput
  {
    protected ID3DContext _textureProvider;
    protected IRenderContext _renderContext;
    protected RETRO_PIXEL_FORMAT _pixelFormat = RETRO_PIXEL_FORMAT.XRGB1555; //Default libretro pixel format
    protected Geometry _geometry;
    protected Timing _timing;

    public TextureOutput(ID3DContext textureProvider, IRenderContext renderContext)
    {
      _textureProvider = textureProvider;
      _renderContext = renderContext;
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

    public bool TrySetHardwareRenderer(ref retro_hw_render_callback renderCallback)
    {
      return _renderContext != null && _renderContext.Init(ref renderCallback);
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
      _renderContext?.OnFramebufferReady(_textureProvider, width, height, pitch);
    }

    public void OnVideoRefresh(IntPtr data, int width, int height, int pitch)
    {
      Texture texture = _textureProvider?.GetTexture(width, height, Usage.Dynamic);
      if (texture != null)
        WriteToTexture(texture, data, width, height, pitch);
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
  }
}
