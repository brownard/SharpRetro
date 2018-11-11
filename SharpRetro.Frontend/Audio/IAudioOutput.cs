using System;

namespace SharpRetro.Frontend.Audio
{
  public interface IAudioOutput
  {
    void OnAudioSample(short left, short right);
    uint OnAudioSampleBatch(IntPtr data, uint frames);
  }
}