using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRetro.Libretro.Environment;

namespace SharpRetro.Libretro.Audio
{
  public interface IAudioOutput
  {
    void OnAudioSample(short left, short right);
    uint OnAudioSampleBatch(IntPtr data, uint frames);
    void SetTiming(Timing timing);
  }
}
