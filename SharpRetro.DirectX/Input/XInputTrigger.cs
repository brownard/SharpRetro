using SharpDX.XInput;
using SharpRetro.Libretro.Input;
using System;

namespace SharpRetro.DirectX.Input
{
  public enum XInputTriggerIndex
  {
    Left,
    Right
  }

  public class XInputTrigger : IAnalogMapping, IButtonMapping
  {
    protected XInputTriggerIndex _trigger;
    protected byte _deadzone;
    protected Func<Gamepad, short> _triggerDelegate;

    public XInputTrigger(XInputTriggerIndex trigger)
      : this(trigger, Gamepad.TriggerThreshold)
    { }

    public XInputTrigger(XInputTriggerIndex trigger, byte deadzone)
    {
      _trigger = trigger;
      _deadzone = deadzone;
      _triggerDelegate = _trigger == XInputTriggerIndex.Left ? (Func<Gamepad, short>)GetLeftTrigger : GetRightTrigger;
    }

    public short GetAnalog(Gamepad gamepad)
    {
      return _triggerDelegate(gamepad);
    }

    public bool IsPressed(Gamepad gamepad)
    {
      return _triggerDelegate(gamepad) != 0;
    }

    protected short GetLeftTrigger(Gamepad gamepad)
    {
      return gamepad.LeftTrigger > _deadzone ? InputUtils.Scale(gamepad.LeftTrigger) : InputUtils.FALSE;
    }

    protected short GetRightTrigger(Gamepad gamepad)
    {
      return gamepad.RightTrigger > _deadzone ? InputUtils.Scale(gamepad.RightTrigger) : InputUtils.FALSE;
    }
  }
}
