using SharpDX.XInput;
using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Input;
using System;

namespace SharpRetro.DirectX.Input
{
  public class XInputDevice : IPollable, IRetroPad, IAnalog, IRumble
  {
    //Constantly polling a disconnected controller causes high CPU load so
    //reduce polling to the specified interval if the controller becomes disconnected
    protected static readonly TimeSpan DISCONNECTED_POLL_INTERVAL = TimeSpan.FromSeconds(2);
    protected DateTime _lastPoll = DateTime.MinValue;
    protected bool _connected = false;

    protected IXInputMapping _mapping;
    protected Controller _controller;
    protected Gamepad _gamepad;
    protected Vibration _vibration;

    public XInputDevice(IXInputMapping mapping)
    {
      _mapping = mapping;
      _controller = new Controller(UserIndex.One);
      _vibration = new Vibration();
    }

    public void Poll()
    {
      DateTime now = DateTime.Now;
      if (!_connected && now - _lastPoll < DISCONNECTED_POLL_INTERVAL)
        return;

      if (_controller.GetState(out State state))
      {
        _connected = true;
        _gamepad = state.Gamepad;
      }
      else
      {
        _connected = false;
        _lastPoll = now;
      }
    }

    public bool IsButtonPressed(RETRO_DEVICE_ID_JOYPAD button)
    {
      return _connected && _mapping.IsButtonPressed(button, _gamepad);
    }

    public short GetAnalog(RETRO_DEVICE_INDEX_ANALOG analogIndex, RETRO_DEVICE_ID_ANALOG analogDirection)
    {
      return _connected ? _mapping.GetAnalog(analogIndex, analogDirection, _gamepad) : (short)0;
    }

    public bool SetRumbleState(retro_rumble_effect effect, ushort strength)
    {      
      //Consider the low frequency (left) motor the "strong" one
      if (effect == retro_rumble_effect.RETRO_RUMBLE_STRONG)
        _vibration.LeftMotorSpeed = strength;
      else if (effect == retro_rumble_effect.RETRO_RUMBLE_WEAK)
        _vibration.RightMotorSpeed = strength;
      if (_connected)
        _controller.SetVibration(_vibration);
      return true;
    }
  }
}
