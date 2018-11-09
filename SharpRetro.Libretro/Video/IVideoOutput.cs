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
    
    /// <summary>
    /// Sets the video geometry used by the libretro core.
    /// </summary>
    /// <param name="geometry">The video geometry.</param>
    void SetGeometry(Geometry geometry);

    /// <summary>
    /// Sets the video/audio timing used by the core.
    /// </summary>
    /// <param name="timing"></param>
    void SetTiming(Timing timing);

    /// <summary>
    /// Called by the core when a framebuffer is ready.
    /// </summary>
    /// <param name="width">The width of the video.</param>
    /// <param name="height">The height of the video.</param>
    /// <param name="pitch">The pitch of the video.</param>
    void OnFramebufferReady(int width, int height, int pitch);

    /// <summary>
    /// Called by the core when video is ready.
    /// </summary>
    /// <param name="data">Pointer to the vide data.</param>
    /// <param name="width">The width of the video.</param>
    /// <param name="height">The height of the video.</param>
    /// <param name="pitch">The pitch of the video.</param>
    void OnVideoRefresh(IntPtr data, int width, int height, int pitch);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="renderCallback"></param>
    /// <returns></returns>
    bool TrySetHardwareRenderer(ref retro_hw_render_callback renderCallback);
  }
}
