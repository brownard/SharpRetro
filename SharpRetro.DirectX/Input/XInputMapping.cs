using SharpDX.XInput;
using SharpRetro.Libretro.Cores;
using System.Collections.Generic;

namespace SharpRetro.DirectX.Input
{
  public class XInputMapping : IXInputMapping
  {
    protected const short ZERO = 0;

    protected class AnalogIndexMapping
    {
      protected AnalogDirectionMapping[] _axis = new AnalogDirectionMapping[2];

      public AnalogIndexMapping()
      {
        _axis = new AnalogDirectionMapping[2];
        //x axis
        _axis[0] = new AnalogDirectionMapping();
        //y axis
        _axis[1] = new AnalogDirectionMapping();
      }

      public short GetAnalog(RETRO_DEVICE_ID_ANALOG analogDirection, Gamepad gamepad)
      {
        return _axis[(int)analogDirection].GetAnalog(gamepad);
      }

      public void AddMapping(RETRO_DEVICE_ID_ANALOG analogDirection, bool positive, IAnalogMapping analogMapping)
      {
        AnalogDirectionMapping mapping = _axis[(int)analogDirection];
        if (positive)
          mapping.Positive = analogMapping;
        else
          mapping.Negative = analogMapping;
      }
    }

    protected class AnalogDirectionMapping
    {
      public IAnalogMapping Positive { get; set; }
      public IAnalogMapping Negative { get; set; }

      public short GetAnalog(Gamepad gamepad)
      {
        short positive = Positive != null ? Positive.GetAnalog(gamepad) : ZERO;
        short negative = Negative != null ? Negative.GetAnalog(gamepad) : ZERO;
        
        if (positive != 0)
        {
          if (negative != 0)
            //Both directions pressed, just return 0
            return 0;
          //The mapped input might return negative values so make them positive
          return positive > 0 ? positive : (short)(-positive - 1);
        }
        if (negative != 0)
          //The mapped input might return positive values, so make them negative
          return negative < 0 ? negative : (short)(-negative - 1);
        return 0;
      }
    }

    protected IDictionary<RETRO_DEVICE_ID_JOYPAD, IButtonMapping> _buttonMappings = new Dictionary<RETRO_DEVICE_ID_JOYPAD, IButtonMapping>();
    protected IDictionary<RETRO_DEVICE_INDEX_ANALOG, AnalogIndexMapping> _analogMappings = new Dictionary<RETRO_DEVICE_INDEX_ANALOG, AnalogIndexMapping>();

    public bool IsButtonPressed(RETRO_DEVICE_ID_JOYPAD button, Gamepad gamepad)
    {
      return _buttonMappings.TryGetValue(button, out IButtonMapping mapping) && mapping.IsPressed(gamepad);
    }

    public short GetAnalog(RETRO_DEVICE_INDEX_ANALOG analogIndex, RETRO_DEVICE_ID_ANALOG analogDirection, Gamepad gamepad)
    {
      return _analogMappings.TryGetValue(analogIndex, out AnalogIndexMapping mapping) ? mapping.GetAnalog(analogDirection, gamepad) : ZERO;
    }

    public void MapButton(RETRO_DEVICE_ID_JOYPAD button, IButtonMapping buttonMapping)
    {
      _buttonMappings[button] = buttonMapping;
    }

    public void MapAnalog(RETRO_DEVICE_INDEX_ANALOG analogIndex, RETRO_DEVICE_ID_ANALOG analogDirection, bool positive, IAnalogMapping analogMapping)
    {
      if (!_analogMappings.TryGetValue(analogIndex, out AnalogIndexMapping mapping))
        _analogMappings[analogIndex] = mapping = new AnalogIndexMapping();
      mapping.AddMapping(analogDirection, positive, analogMapping);
    }
  }
}
