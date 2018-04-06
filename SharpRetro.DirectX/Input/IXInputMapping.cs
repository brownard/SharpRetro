using SharpDX.XInput;
using SharpRetro.Libretro.Cores;

namespace SharpRetro.DirectX.Input
{
  public interface IXInputMapping
  {
    void MapButton(RETRO_DEVICE_ID_JOYPAD button, IButtonMapping mapping);
    void MapAnalog(RETRO_DEVICE_INDEX_ANALOG analogIndex, RETRO_DEVICE_ID_ANALOG analogDirection, bool positive, IAnalogMapping mapping);
    bool IsButtonPressed(RETRO_DEVICE_ID_JOYPAD button, Gamepad gamepad);
    short GetAnalog(RETRO_DEVICE_INDEX_ANALOG analogIndex, RETRO_DEVICE_ID_ANALOG analogDirection, Gamepad gamepad);
  }

  public interface IButtonMapping
  {
    bool IsPressed(Gamepad gamepad);
  }

  public interface IAnalogMapping
  {
    short GetAnalog(Gamepad gamepad);
  }
}
