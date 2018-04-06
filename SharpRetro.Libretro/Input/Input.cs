using SharpRetro.Libretro.Cores;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpRetro.Libretro.Input
{
  public class Input : IInput
  {
    protected const short FALSE = 0;
    protected const short TRUE = 1;

    protected retro_set_rumble_state_t _rumbleCallback;

    protected readonly IDictionary<int, IPollable> _pollables;
    protected readonly IDictionary<int, IRetroPad> _retroPads;
    protected readonly IDictionary<int, IAnalog> _analogs;
    protected readonly IDictionary<int, IRumble> _rumbles;
    protected IPointer _pointer;
    protected IKeyboard _keyboard;

    public Input()
    {
      _rumbleCallback = new retro_set_rumble_state_t(SetRumbleState);
      _pollables = new Dictionary<int, IPollable>(8);
      _retroPads = new Dictionary<int, IRetroPad>(8);
      _analogs = new Dictionary<int, IAnalog>(8);
      _rumbles = new Dictionary<int, IRumble>(8);
    }

    public void AddDevice(int port, IInputDevice device)
    {
      if (device is IPollable pollable)
        _pollables[port] = pollable;
      if (device is IRetroPad retroPad)
        _retroPads[port] = retroPad;
      if (device is IAnalog analog)
        _analogs[port] = analog;
      if (device is IRumble rumble)
        _rumbles[port] = rumble;
      if (device is IKeyboard keyboard)
        _keyboard = keyboard;
      if (device is IPointer pointer)
        _pointer = pointer;
    }

    public void OnInputPoll()
    {
      foreach (IPollable pollable in _pollables.Values)
        pollable.Poll();
    }

    public short OnInputState(uint port, uint device, uint index, uint id)
    {
      switch ((RETRO_DEVICE)device)
      {
        case RETRO_DEVICE.POINTER:
          return GetPointerStatus((RETRO_DEVICE_ID_POINTER)id);
        case RETRO_DEVICE.KEYBOARD:
          return GetKeyboardStatus((RETRO_KEY)id);
        case RETRO_DEVICE.JOYPAD:
          return GetRetroPadStatus((int)port, (RETRO_DEVICE_ID_JOYPAD)id);
        case RETRO_DEVICE.ANALOG:
          return GetAnalogStatus((int)port, (RETRO_DEVICE_INDEX_ANALOG)index, (RETRO_DEVICE_ID_ANALOG)id);
        default:
          return FALSE;
      }
    }

    public bool TrySetRumbleInterface(ref retro_rumble_interface value)
    {
      value.set_rumble_state = Marshal.GetFunctionPointerForDelegate(_rumbleCallback);
      return true;
    }

    protected short GetKeyboardStatus(RETRO_KEY key)
    {
      return _keyboard != null && _keyboard.IsKeyPressed(key) ? TRUE : FALSE;
    }

    protected short GetRetroPadStatus(int port, RETRO_DEVICE_ID_JOYPAD button)
    {
      return _retroPads.TryGetValue(port, out IRetroPad retroPad) && retroPad.IsButtonPressed(button) ? TRUE : FALSE;
    }

    protected short GetAnalogStatus(int port, RETRO_DEVICE_INDEX_ANALOG analogIndex, RETRO_DEVICE_ID_ANALOG analogDirection)
    {
      return _analogs.TryGetValue(port, out IAnalog analog) ? analog.GetAnalog(analogIndex, analogDirection) : FALSE;
    }

    protected bool SetRumbleState(uint port, retro_rumble_effect effect, ushort strength)
    {
      return _rumbles.TryGetValue((int)port, out IRumble rumble) && rumble.SetRumbleState(effect, strength);
    }

    protected short GetPointerStatus(RETRO_DEVICE_ID_POINTER id)
    {
      if (_pointer == null)
        return FALSE;

      switch (id)
      {
        case RETRO_DEVICE_ID_POINTER.X:
          return _pointer.GetPointerX();
        case RETRO_DEVICE_ID_POINTER.Y:
          return _pointer.GetPointerY();
        case RETRO_DEVICE_ID_POINTER.PRESSED:
          return _pointer.IsPointerPressed() ? TRUE : FALSE;
        default:
          return FALSE;
      }
    }
  }
}
