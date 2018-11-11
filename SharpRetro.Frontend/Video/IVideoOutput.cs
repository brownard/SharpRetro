using System;

namespace SharpRetro.Frontend.Video
{
  public interface IVideoOutput
  {
    void OnVideoRefresh(IntPtr data, uint width, uint height, uint pitch);
  }
}