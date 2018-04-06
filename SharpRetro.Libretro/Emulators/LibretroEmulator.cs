using SharpRetro.Emulators;
using SharpRetro.Games;
using SharpRetro.Libretro.Audio;
using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Environment;
using SharpRetro.Libretro.Input;
using SharpRetro.Libretro.Interop;
using SharpRetro.Libretro.Log;
using SharpRetro.Libretro.Video;
using SharpRetro.Log;
using System;
using System.Runtime.InteropServices;

namespace SharpRetro.Libretro.Emulators
{
  /// <summary>
  /// 
  /// </summary>
  public unsafe class LibretroEmulator : IEmulator
  {
    protected ILibretroCore _core;
    protected IEnvironmentHandler _environment;
    protected IVideoOutput _videoOutput;
    protected IAudioOutput _audioOutput;
    protected IInput _input;
    protected IInteropHandler _interop;
    protected ILogger _logger;

    protected retro_log_printf_t _logPrintfDelegate;
    protected SystemInfo _systemInfo;
    protected Geometry _geometry;
    protected Timing _timing;
    protected retro_system_av_info _avInfo;

    public LibretroEmulator(ILibretroCore core, IEnvironmentHandler environment, IVideoOutput videoOutput, IAudioOutput audioOutput,
      IInput input, IInteropHandler interop, ILogger logger)
    {
      _core = core;
      _environment = environment;
      _videoOutput = videoOutput;
      _audioOutput = audioOutput;
      _input = input;
      _interop = interop;
      _logger = logger;
    }

    #region Init

    protected void InitCore()
    {
      InitInterfaces();
      InitCallbacks();
      _core.Init();
    }

    protected void InitInterfaces()
    {
      _logPrintfDelegate = new retro_log_printf_t(OnLogPrintf);
    }

    protected void InitCallbacks()
    {
      _core.SetEnvironment(OnEnvironment);
      _core.SetAudioSample(OnAudioSample);
      _core.SetAudioSampleBatch(OnAudioSampleBatch);
      _core.SetVideoRefresh(OnVideoRefresh);
      _core.SetInputPoll(OnInputPoll);
      _core.SetInputStateCallback(OnInputState);
    }

    #endregion

    #region System/AV info

    public SystemInfo SystemInfo
    {
      get { return _systemInfo; }
    }

    public Geometry Geometry
    {
      get { return _geometry; }
    }

    public Timing Timing
    {
      get { return _timing; }
    }

    protected void GetSystemInfo()
    {
      retro_system_info info = new retro_system_info();
      _core.GetSystemInfo(ref info);
      SetSystemInfo(info);
    }

    protected void GetAVInfo()
    {
      retro_system_av_info info = new retro_system_av_info();
      _core.GetSystemAVInfo(ref info);
      SetAVInfo(info);
    }

    protected void SetSystemInfo(retro_system_info info)
    {
      _systemInfo = new SystemInfo
      {
        LibraryName = Marshal.PtrToStringAnsi(info.library_name),
        LibraryVersion = Marshal.PtrToStringAnsi(info.library_version),
        ValidExtensions = Marshal.PtrToStringAnsi(info.valid_extensions),
        NeedFullPath = info.need_fullpath,
        BlockExtract = info.block_extract
      };
    }

    protected void SetAVInfo(retro_system_av_info avInfo)
    {
      SetGeometry(avInfo.geometry);
      SetTiming(avInfo.timing);
    }

    protected void SetGeometry(retro_game_geometry gameGeometry)
    {
      Geometry geometry = new Geometry
      {
        AspectRatio = gameGeometry.aspect_ratio,
        BaseWidth = (int)gameGeometry.base_width,
        BaseHeight = (int)gameGeometry.base_height,
        MaxWidth = (int)gameGeometry.max_width,
        MaxHeight = (int)gameGeometry.max_height
      };

      _geometry = geometry;
      _videoOutput.SetGeometry(geometry);
    }

    protected void SetTiming(retro_system_timing systemTiming)
    {
      Timing timing = new Timing
      {
        FramesPerSecond = systemTiming.fps,
        SampleRate = systemTiming.sample_rate
      };

      _timing = timing;
      _videoOutput.SetTiming(timing);
      _audioOutput.SetTiming(timing);
    }

    #endregion

    private void OnAudioSample(short left, short right)
    {
      _audioOutput.OnAudioSample(left, right);
    }

    private uint OnAudioSampleBatch(IntPtr data, uint frames)
    {
      return _audioOutput.OnAudioSampleBatch(data, frames);
    }

    private void OnVideoRefresh(IntPtr data, uint width, uint height, uint pitch)
    {
      if (data.ToInt32() == retro_hw_render_callback.RETRO_HW_FRAME_BUFFER_VALID)
        _videoOutput.OnFramebufferReady((int)width, (int)height, (int)pitch);
      else
        _videoOutput.OnVideoRefresh(data, (int)width, (int)height, (int)pitch);
    }

    private void OnInputPoll()
    {
      _input.OnInputPoll();
    }

    private short OnInputState(uint port, uint device, uint index, uint id)
    {
      return _input.OnInputState(port, device, index, id);
    }

    #region Environment

    private bool OnEnvironment(RETRO_ENVIRONMENT cmd, IntPtr data)
    {
      switch (cmd)
      {
        case RETRO_ENVIRONMENT.SET_ROTATION:
          return _environment.SetRotation(_interop.ReadUint(data));
        case RETRO_ENVIRONMENT.GET_OVERSCAN:
          return _interop.TryBoolToPtr(data, _environment.GetOverscan);
        case RETRO_ENVIRONMENT.GET_CAN_DUPE:
          return _interop.TryBoolToPtr(data, _environment.GetCanDupe);
        case RETRO_ENVIRONMENT.SET_MESSAGE:
          return _environment.SetMessage(Marshal.PtrToStructure<retro_message>(data));
        case RETRO_ENVIRONMENT.SHUTDOWN:
          return Shutdown();
        case RETRO_ENVIRONMENT.SET_PERFORMANCE_LEVEL:
          return _environment.SetPerformanceLevel(_interop.ReadUint(data));
        case RETRO_ENVIRONMENT.GET_SYSTEM_DIRECTORY:
          return _interop.TryStringToPtr(data, _environment.GetSystemDirectory);
        case RETRO_ENVIRONMENT.SET_PIXEL_FORMAT:
          return _videoOutput.SetPixelFormat((RETRO_PIXEL_FORMAT)_interop.ReadInt(data));
        case RETRO_ENVIRONMENT.SET_INPUT_DESCRIPTORS:
          return _environment.SetInputDescriptors(Marshal.PtrToStructure<retro_input_descriptor>(data));
        case RETRO_ENVIRONMENT.SET_KEYBOARD_CALLBACK:
          return _environment.SetKeyboardCallback(Marshal.PtrToStructure<retro_keyboard_callback>(data));
        case RETRO_ENVIRONMENT.SET_DISK_CONTROL_INTERFACE:
          return false;
        case RETRO_ENVIRONMENT.SET_HW_RENDER:
          return _interop.TrySetHWRenderCallback(data, _videoOutput.TrySetHardwareRenderer);
        case RETRO_ENVIRONMENT.GET_VARIABLE:
          return _interop.TryGetVariable(data, _environment.GetVariable);
        case RETRO_ENVIRONMENT.SET_VARIABLES:
          return _environment.SetVariables(_interop.PtrToVariables(data));
        case RETRO_ENVIRONMENT.GET_VARIABLE_UPDATE:
          return _environment.GetVariableUpdate();
        case RETRO_ENVIRONMENT.SET_SUPPORT_NO_GAME:
          return _environment.SetSupportNoGame();
        case RETRO_ENVIRONMENT.GET_LIBRETRO_PATH:
          return _interop.TryStringToPtr(data, _environment.GetLibretroPath);
        case RETRO_ENVIRONMENT.SET_AUDIO_CALLBACK:
          return false;
        case RETRO_ENVIRONMENT.SET_FRAME_TIME_CALLBACK:
          return false;
        case RETRO_ENVIRONMENT.GET_RUMBLE_INTERFACE:
          return _interop.TrySetRumbleInterface(data, _input.TrySetRumbleInterface);
        case RETRO_ENVIRONMENT.GET_INPUT_DEVICE_CAPABILITIES:
          return false;
        case RETRO_ENVIRONMENT.GET_LOG_INTERFACE:
          return _interop.TryPtrToPtr(data, Marshal.GetFunctionPointerForDelegate(_logPrintfDelegate));
        case RETRO_ENVIRONMENT.GET_PERF_INTERFACE:
          return false;
        case RETRO_ENVIRONMENT.GET_LOCATION_INTERFACE:
          return false;
        case RETRO_ENVIRONMENT.GET_CORE_ASSETS_DIRECTORY:
          return false;
        case RETRO_ENVIRONMENT.GET_SAVE_DIRECTORY:
          return _interop.TryStringToPtr(data, _environment.GetSaveDirectory);
        case RETRO_ENVIRONMENT.SET_SYSTEM_AV_INFO:
          SetAVInfo(Marshal.PtrToStructure<retro_system_av_info>(data));
          return true;
        case RETRO_ENVIRONMENT.SET_CONTROLLER_INFO:
          return true;
        case RETRO_ENVIRONMENT.SET_MEMORY_MAPS:
          return false;
        case RETRO_ENVIRONMENT.SET_GEOMETRY:
          SetGeometry(Marshal.PtrToStructure<retro_game_geometry>(data));
          return true;
        case RETRO_ENVIRONMENT.SET_SUBSYSTEM_INFO:
          return _environment.SetSubsytemInfo(Marshal.PtrToStructure<retro_subsystem_info>(data));
        default:
          return _environment.OnUnhandledEnvironmentCommand((int)cmd, data);
      }
    }

    protected bool Shutdown()
    {
      return true;
    }

    #endregion

    #region Game loading

    protected bool LoadGame(string path, byte[] data, string meta)
    {
      retro_game_info gameInfo = new retro_game_info();
      gameInfo.path = path;
      gameInfo.meta = meta;
      if (data == null || data.Length == 0)
        return _core.LoadGame(ref gameInfo);

      fixed (byte* p = &data[0])
      {
        gameInfo.data = (IntPtr)p;
        gameInfo.size = (uint)data.Length;
        return _core.LoadGame(ref gameInfo);
      }
    }

    #endregion

    #region Logging

    protected void OnLogPrintf(RETRO_LOG_LEVEL level, string fmt, IntPtr a0, IntPtr a1, IntPtr a2, IntPtr a3, IntPtr a4, IntPtr a5, IntPtr a6, IntPtr a7, IntPtr a8, IntPtr a9, IntPtr a10, IntPtr a11, IntPtr a12, IntPtr a13, IntPtr a14, IntPtr a15)
    {
      //avert your eyes, these things were not meant to be seen in c#
      //I'm not sure this is a great idea. It would suck for silly logging to be unstable. But.. I dont think this is unstable. The sprintf might just print some garbledy stuff.
      IntPtr[] args = new IntPtr[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15 };
      string message = Printf(fmt, args);

      switch (level)
      {
        case RETRO_LOG_LEVEL.DEBUG:
          _logger.Debug(message);
          break;
        case RETRO_LOG_LEVEL.INFO:
          _logger.Info(message);
          break;
        case RETRO_LOG_LEVEL.WARN:
          _logger.Warn(message);
          break;
        case RETRO_LOG_LEVEL.ERROR:
          _logger.Error(message);
          break;
      }
    }

    protected string Printf(string format, IntPtr[] args)
    {
      int idx = 0;
      string message;
      try
      {
        message = Sprintf.sprintf(format, () => args[idx++]);
      }
      catch (Exception ex)
      {
        message = string.Format("Error in sprintf - {0}", ex);
      }
      return message;
    }

    #endregion

    #region IEmulator

    public void Init()
    {
      InitCore();
      GetSystemInfo();
    }

    public bool Load(IGame game)
    {
      if (!LoadGame(game.Path, game.Data, string.Empty))
        return false;
      GetAVInfo();
      return true;
    }

    public void Unload()
    {
      _core.UnloadGame();
    }

    public void Run()
    {
      _core.Run();
    }

    public void Reset()
    {
      _core.Reset();
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
      IDisposable disposable = _core as IDisposable;
      if (disposable != null)
        disposable.Dispose();
      if (_interop != null)
        _interop.Dispose();
      _core = null;
      _interop = null;
    }

    #endregion
  }
}
