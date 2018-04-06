using SharpDX.XInput;
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
    protected Func<Gamepad, byte> _triggerDelegate;

    public XInputTrigger(XInputTriggerIndex trigger)
      : this(trigger, Gamepad.TriggerThreshold)
    { }

    public XInputTrigger(XInputTriggerIndex trigger, byte deadzone)
    {
      _trigger = trigger;
      _deadzone = deadzone;
      _triggerDelegate = _trigger == XInputTriggerIndex.Left ? (Func<Gamepad, byte>)GetLeftTrigger : GetRightTrigger;
    }

    public short GetAnalog(Gamepad gamepad)
    {
      byte value = _triggerDelegate(gamepad);
      if (value == 0)
        return 0;
      //Scale byte to short
      return (short)(((value << 8) | value) >> 1);
    }

    public bool IsPressed(Gamepad gamepad)
    {
      return _triggerDelegate(gamepad) != 0;
    }

    protected byte GetLeftTrigger(Gamepad gamepad)
    {
      return gamepad.LeftTrigger > _deadzone ? gamepad.LeftTrigger : (byte)0;
    }

    protected byte GetRightTrigger(Gamepad gamepad)
    {
      return gamepad.RightTrigger > _deadzone ? gamepad.RightTrigger : (byte)0;
    }
  }
}
