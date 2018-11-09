using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;
using SharpRetro.Libretro.Input;

namespace SharpRetro.DirectX.Input
{
  public class XInputButton : IButtonMapping, IAnalogMapping
  {
    protected GamepadButtonFlags _button;

    public XInputButton(GamepadButtonFlags button)
    {
      _button = button;
    }

    public bool IsPressed(Gamepad gamepad)
    {
      return gamepad.Buttons.HasFlag(_button);
    }

    public short GetAnalog(Gamepad gamepad)
    {
      return gamepad.Buttons.HasFlag(_button) ? InputUtils.MAX : InputUtils.FALSE;
    }
  }
}
