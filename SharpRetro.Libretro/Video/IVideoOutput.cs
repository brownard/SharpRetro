using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Environment;
using System;

namespace SharpRetro.Libretro.Video
{
  /// <summary>
  /// Interface for handling the video output of a libretro core.
  /// </summary>
  public interface IVideoOutput
  {
    /// <summary>
    /// Sets the internal pixel format used by the libretro core.
    /// </summary>
    /// <remarks>
    /// The default pixel format is <see cref="RETRO_PIXEL_FORMAT.XRGB1555"/>.
    /// This method will be called inside <see cref="ILibretroCore.LoadGame(ref retro_game_info)"/>
    /// or <see cref="ILibretroCore.GetSystemAVInfo(ref retro_system_av_info)"/>.
    /// </remarks>
    /// <param name="pixelFormat"></param>
    /// <returns><c>true</c> if the frontent supports the specified pixel format.</returns>
    bool SetPixelFormat(RETRO_PIXEL_FORMAT pixelFormat);    
    void SetGeometry(Geometry geometry);
    void SetTiming(Timing timing);
    void OnFramebufferReady(int width, int height, int pitch);
    void OnVideoRefresh(IntPtr data, int width, int height, int pitch);
    bool TrySetHardwareRenderer(ref retro_hw_render_callback renderCallback);
  }
}
