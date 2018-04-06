using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRetro.Libretro.Cores;

namespace SharpRetro.Libretro.Input
{
  public interface IRumble : IInputDevice
  {
    bool SetRumbleState(retro_rumble_effect effect, ushort strength);
  }
}
