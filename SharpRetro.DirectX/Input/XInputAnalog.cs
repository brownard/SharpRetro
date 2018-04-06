using SharpDX.XInput;
using System;

namespace SharpRetro.DirectX.Input
{
  public enum XInputThumb
  {
    LeftX,
    LeftY,
    RightX,
    RightY
  }

  public class XInputAnalog : IAnalogMapping, IButtonMapping
  {
    protected XInputThumb _thumb;
    protected bool _positive;
    protected short _deadzone;
    protected Func<Gamepad, short> _analogDelegate;

    public XInputAnalog(XInputThumb thumb, bool positive)
      : this(thumb, positive,
          thumb == XInputThumb.LeftX || thumb == XInputThumb.LeftY ? Gamepad.LeftThumbDeadZone : Gamepad.RightThumbDeadZone)
    { }

    public XInputAnalog(XInputThumb thumb, bool positive, short deadzone)
    {
      _thumb = thumb;
      _positive = positive;
      _deadzone = deadzone;
      Init(thumb, positive);
    }

    public short GetAnalog(Gamepad gamepad)
    {
      return _analogDelegate(gamepad);
    }

    public bool IsPressed(Gamepad gamepad)
    {
      return _analogDelegate(gamepad) != 0;
    }

    protected void Init(XInputThumb thumb, bool positive)
    {
      if (thumb == XInputThumb.LeftX)
        _analogDelegate = positive ? (Func<Gamepad, short>)GetLeftThumbXPositive : GetLeftThumbXNegative;
      else if(thumb == XInputThumb.LeftY)
        _analogDelegate = positive ? (Func<Gamepad, short>)GetLeftThumbYPositive : GetLeftThumbYNegative;
      else if(thumb == XInputThumb.RightX)
        _analogDelegate = positive ? (Func<Gamepad, short>)GetRightThumbXPositive : GetRightThumbXNegative;
      else //thumb == XInputThumb.RightY
        _analogDelegate = positive ? (Func<Gamepad, short>)GetRightThumbYPositive : GetRightThumbYNegative;
    }

    protected short GetLeftThumbXPositive(Gamepad gamepad)
    {
      return gamepad.LeftThumbX > _deadzone ? gamepad.LeftThumbX : (short)0;
    }

    protected short GetLeftThumbXNegative(Gamepad gamepad)
    {
      return gamepad.LeftThumbX < -_deadzone ? gamepad.LeftThumbX : (short)0;
    }

    protected short GetLeftThumbYPositive(Gamepad gamepad)
    {
      return gamepad.LeftThumbY > _deadzone ? gamepad.LeftThumbY : (short)0;
    }

    protected short GetLeftThumbYNegative(Gamepad gamepad)
    {
      return gamepad.LeftThumbY < -_deadzone ? gamepad.LeftThumbY : (short)0;
    }

    protected short GetRightThumbXPositive(Gamepad gamepad)
    {
      return gamepad.RightThumbX > _deadzone ? gamepad.RightThumbX : (short)0;
    }

    protected short GetRightThumbXNegative(Gamepad gamepad)
    {
      return gamepad.RightThumbX < -_deadzone ? gamepad.RightThumbX : (short)0;
    }

    protected short GetRightThumbYPositive(Gamepad gamepad)
    {
      return gamepad.RightThumbY > _deadzone ? gamepad.RightThumbY : (short)0;
    }

    protected short GetRightThumbYNegative(Gamepad gamepad)
    {
      return gamepad.RightThumbY < -_deadzone ? gamepad.RightThumbY : (short)0;
    }
  }
}
