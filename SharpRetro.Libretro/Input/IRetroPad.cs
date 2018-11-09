﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRetro.Libretro.Cores;

namespace SharpRetro.Libretro.Input
{
  public interface IRetroPad : IInputDevice
  {
    bool IsButtonPressed(RETRO_DEVICE_ID_JOYPAD button);
    short GetAnalog(RETRO_DEVICE_INDEX_ANALOG analogIndex, RETRO_DEVICE_ID_ANALOG analogDirection);
    bool SetRumbleState(retro_rumble_effect effect, ushort strength);
  }
}
