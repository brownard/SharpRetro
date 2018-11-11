using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Frontend.Audio
{
  public class AudioOutput : IAudioOutput
  {
    public void OnAudioSample(short left, short right)
    {

    }

    public uint OnAudioSampleBatch(IntPtr data, uint frames)
    {
      return 0;
    }
  }
}
