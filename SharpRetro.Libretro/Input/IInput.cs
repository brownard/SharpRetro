using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRetro.Libretro.Cores;

namespace SharpRetro.Libretro.Input
{
  public interface IInput
  {
    void OnInputPoll();
    short OnInputState(uint port, uint device, uint index, uint id);
    bool TrySetRumbleInterface(ref retro_rumble_interface value);
  }
}
