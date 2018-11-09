using SharpDX.XInput;
using SharpRetro.DirectX.GL;
using SharpRetro.DirectX.Input;
using SharpRetro.DirectX.Video;
using SharpRetro.Emulators;
using SharpRetro.Games;
using SharpRetro.Libretro.Audio;
using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Emulators;
using SharpRetro.Libretro.Environment;
using SharpRetro.Libretro.Input;
using SharpRetro.Libretro.Interop;
using SharpRetro.Libretro.Native;
using SharpRetro.Log;
using System;
using System.IO;

namespace SharpRetro.Client.Models
{
  class LibretroModel
  {
    protected ILibrary _coreLibrary;
    protected IRenderContext _renderContext;
    protected LibretroEmulator _emulator;

    public void LoadCore(string path, ID3DContext d3dContext, IAudioOutput audioOutput)
    {
      UnloadCore();

      _coreLibrary = new Library(path);
      _renderContext = new DXRenderContext(d3dContext.Device);

      ILibretroCore core = new LibretroCore(_coreLibrary);

      string coreDirectory = Path.GetDirectoryName(path);
      IEnvironmentHandler environment = new Libretro.Environment.Environment()
      {
        LibretroPath = coreDirectory,
        SystemDirectory = coreDirectory,
        SaveDirectory = coreDirectory
      };

      IXInputMapping mapping = GetMapping();
      XInputDevice controller1 = new XInputDevice(mapping);
      Input input = new Input();
      input.AddDevice(0, controller1);

      _emulator = new LibretroEmulator(core, environment, new TextureOutput(d3dContext, _renderContext), audioOutput, input, new ConsoleLogger());
      _emulator.Init();
    }

    public void UnloadCore()
    {
      if (_emulator != null)
      {
        _emulator.Deinit();
        _emulator = null;
      }
      if (_renderContext is IDisposable disposable)
      {
        disposable.Dispose();
        _renderContext = null;
      }
      if (_coreLibrary != null)
      {
        _coreLibrary.Dispose();
        _coreLibrary = null;
      }
    }

    public void Run()
    {
      if (_emulator != null)
        _emulator.Run();
    }

    public bool LoadGame(string path)
    {
      IGame game = new Game
      {
        Path = path,
        Data = _emulator.SystemInfo.NeedFullPath ? null : File.ReadAllBytes(path)
      };
      return _emulator.Load(game);
    }

    protected IXInputMapping GetMapping()
    {
      IXInputMapping mapping = new XInputMapping();
      mapping.MapAnalog(RETRO_DEVICE_INDEX_ANALOG.LEFT, RETRO_DEVICE_ID_ANALOG.X, true, new XInputAnalog(XInputThumb.LeftX, true));
      mapping.MapAnalog(RETRO_DEVICE_INDEX_ANALOG.LEFT, RETRO_DEVICE_ID_ANALOG.X, false, new XInputAnalog(XInputThumb.LeftX, false));
      mapping.MapAnalog(RETRO_DEVICE_INDEX_ANALOG.LEFT, RETRO_DEVICE_ID_ANALOG.Y, true, new XInputAnalog(XInputThumb.LeftY, true));
      mapping.MapAnalog(RETRO_DEVICE_INDEX_ANALOG.LEFT, RETRO_DEVICE_ID_ANALOG.Y, false, new XInputAnalog(XInputThumb.LeftY, false));

      mapping.MapAnalog(RETRO_DEVICE_INDEX_ANALOG.RIGHT, RETRO_DEVICE_ID_ANALOG.X, true, new XInputAnalog(XInputThumb.RightX, true));
      mapping.MapAnalog(RETRO_DEVICE_INDEX_ANALOG.RIGHT, RETRO_DEVICE_ID_ANALOG.X, false, new XInputAnalog(XInputThumb.RightX, false));
      mapping.MapAnalog(RETRO_DEVICE_INDEX_ANALOG.RIGHT, RETRO_DEVICE_ID_ANALOG.Y, true, new XInputAnalog(XInputThumb.RightY, true));
      mapping.MapAnalog(RETRO_DEVICE_INDEX_ANALOG.RIGHT, RETRO_DEVICE_ID_ANALOG.Y, false, new XInputAnalog(XInputThumb.RightY, false));

      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.START, new XInputButton(GamepadButtonFlags.Start));
      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.SELECT, new XInputButton(GamepadButtonFlags.Back));

      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.LEFT, new XInputButton(GamepadButtonFlags.DPadLeft));
      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.RIGHT, new XInputButton(GamepadButtonFlags.DPadRight));
      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.UP, new XInputButton(GamepadButtonFlags.DPadUp));
      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.DOWN, new XInputButton(GamepadButtonFlags.DPadDown));

      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.B, new XInputButton(GamepadButtonFlags.A));
      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.A, new XInputButton(GamepadButtonFlags.B));
      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.Y, new XInputButton(GamepadButtonFlags.X));
      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.X, new XInputButton(GamepadButtonFlags.Y));

      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.L, new XInputButton(GamepadButtonFlags.LeftShoulder));
      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.R, new XInputButton(GamepadButtonFlags.RightShoulder));
      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.L2, new XInputTrigger(XInputTriggerIndex.Left));
      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.R2, new XInputTrigger(XInputTriggerIndex.Right));
      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.L3, new XInputButton(GamepadButtonFlags.LeftThumb));
      mapping.MapButton(RETRO_DEVICE_ID_JOYPAD.R3, new XInputButton(GamepadButtonFlags.RightThumb));
      return mapping;
    }
  }
}
