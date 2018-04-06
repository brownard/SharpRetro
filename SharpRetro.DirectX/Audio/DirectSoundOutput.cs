using SharpRetro.Libretro.Audio;
using SharpRetro.Libretro.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.DirectX.Audio
{
  public class DirectSoundOutput : IAudioOutput
  {
    public void SetTiming(Timing timing)
    {

    }

    public void OnAudioSample(short left, short right)
    {
      
    }

    public uint OnAudioSampleBatch(IntPtr data, uint frames)
    {
      return frames;
    }
  }
}
