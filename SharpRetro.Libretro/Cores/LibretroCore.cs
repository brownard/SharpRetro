using SharpRetro.Libretro.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SharpRetro.Libretro.Cores
{
  public class LibretroCore : ILibretroCore
  {    
    // Entry point delegates, hooked up by reflection
    [Libretro("retro_set_environment")]
    protected epretro_set_environment _setEnvironment;
    [Libretro("retro_set_video_refresh")]
    protected epretro_set_video_refresh _setVideoRefresh;
    [Libretro("retro_set_audio_sample")]
    protected epretro_set_audio_sample _setAudioSample;
    [Libretro("retro_set_audio_sample_batch")]
    protected epretro_set_audio_sample_batch _setAudioSampleBatch;
    [Libretro("retro_set_input_poll")]
    protected epretro_set_input_poll _setInputPoll;
    [Libretro("retro_set_input_state")]
    protected epretro_set_input_state _setInputState;
    [Libretro("retro_init")]
    protected epretro_init _init;
    [Libretro("retro_deinit")]
    protected epretro_deinit _deinit;
    [Libretro("retro_api_version")]
    protected epretro_api_version _apiVersion;
    [Libretro("retro_get_system_info")]
    protected epretro_get_system_info _getSystemInfo;
    [Libretro("retro_get_system_av_info")]
    protected epretro_get_system_av_info _getSystemAVnfo;
    [Libretro("retro_set_controller_port_device")]
    protected epretro_set_controller_port_device _setControllerPortDevice;
    [Libretro("retro_reset")]
    protected epretro_reset _reset;
    [Libretro("retro_run")]
    protected epretro_run _run;
    [Libretro("retro_serialize_size")]
    protected epretro_serialize_size _serializeSize;
    [Libretro("retro_serialize")]
    protected epretro_serialize _serialize;
    [Libretro("retro_unserialize")]
    protected epretro_unserialize _unserialize;
    [Libretro("retro_cheat_reset")]
    protected epretro_cheat_reset _cheatReset;
    [Libretro("retro_cheat_set")]
    protected epretro_cheat_set _cheatSet;
    [Libretro("retro_load_game")]
    protected epretro_load_game _loadGame;
    [Libretro("retro_load_game_special")]
    protected epretro_load_game_special _loadGameSpecial;
    [Libretro("retro_unload_game")]
    protected epretro_unload_game _unloadGame;
    [Libretro("retro_get_region")]
    protected epretro_get_region _getRegion;
    [Libretro("retro_get_memory_data")]
    protected epretro_get_memory_data _getMemoryData;
    [Libretro("retro_get_memory_size")]
    protected epretro_get_memory_size _getMemorySize;

    //Callback delegates
    protected retro_environment_t _environmentCallback;
    protected retro_video_refresh_t _videoRefreshCallback;
    protected retro_audio_sample_t _audioSampleCallback;
    protected retro_audio_sample_batch_t _audioSampleBatchCallback;
    protected retro_input_poll_t _inputPollCallback;
    protected retro_input_state_t _inputStateCallback;

    protected ILibrary _library;

    public LibretroCore(ILibrary library)
    {
      _library = library;
      AttachEntryPoints();
    }

    protected void AttachEntryPoints()
    {
      foreach (FieldInfo entryPoint in GetAllEntryPoints())
        AttachEntryPoint(entryPoint);
    }

    protected void AttachEntryPoint(FieldInfo entryPoint)
    {
      LibretroAttribute attribute = entryPoint.GetCustomAttribute<LibretroAttribute>();
      Delegate dlgt = _library.GetProcDelegate(attribute.EntryPoint, entryPoint.FieldType);
      if (dlgt == null)
        throw new Exception($"Unable to attach to entry point {entryPoint.Name}");
      entryPoint.SetValue(this, dlgt);
    }

    protected IEnumerable<FieldInfo> GetAllEntryPoints()
    {
      return GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(f => f.IsDefined(typeof(LibretroAttribute)));
    }

    #region Libretro API

    public void SetEnvironment(Func<RETRO_ENVIRONMENT, IntPtr, bool> environmentCallback)
    {
      _environmentCallback = new retro_environment_t(environmentCallback);
      _setEnvironment(_environmentCallback);
    }

    public void SetVideoRefresh(Action<IntPtr, uint, uint, uint> videoRefreshCallback)
    {
      _videoRefreshCallback = new retro_video_refresh_t(videoRefreshCallback);
      _setVideoRefresh(_videoRefreshCallback);
    }

    public void SetAudioSample(Action<short, short> audioSampleCallback)
    {
      _audioSampleCallback = new retro_audio_sample_t(audioSampleCallback);
      _setAudioSample(_audioSampleCallback);
    }

    public void SetAudioSampleBatch(Func<IntPtr,uint,uint> audioSampleBatchCallback)
    {
      _audioSampleBatchCallback = new retro_audio_sample_batch_t(audioSampleBatchCallback);
      _setAudioSampleBatch(_audioSampleBatchCallback);
    }

    public void SetInputPoll(Action inputPollCallback)
    {
      _inputPollCallback = new retro_input_poll_t(inputPollCallback);
      _setInputPoll(_inputPollCallback);
    }

    public void SetInputStateCallback(Func<uint, uint, uint, uint, short> inputStateCallback)
    {
      _inputStateCallback = new retro_input_state_t(inputStateCallback);
      _setInputState(_inputStateCallback);
    }

    public void Init()
    {
      _init();
    }

    public void Deinit()
    {
      _deinit();
    }

    public uint ApiVersion()
    {
      return _apiVersion();
    }

    public void GetSystemInfo(ref retro_system_info info)
    {
      _getSystemInfo(ref info);
    }

    public void GetSystemAVInfo(ref retro_system_av_info avInfo)
    {
      _getSystemAVnfo(ref avInfo);
    }

    public void SetControllerPortDevice(uint port, uint device)
    {
      _setControllerPortDevice(port, device);
    }

    public void Reset()
    {
      _reset();
    }

    public void Run()
    {
      _run();
    }

    public uint SerializeSize()
    {
      return _serializeSize();
    }

    public bool Serialize(IntPtr data, uint size)
    {
      return _serialize(data, size);
    }

    public bool Unserialize(IntPtr data, uint size)
    {
      return _unserialize(data, size);
    }

    public void CheatReset()
    {
      _cheatReset();
    }

    public void CheatSet(uint index, bool enabled, string code)
    {
      _cheatSet(index, enabled, code);
    }

    public bool LoadGame(ref retro_game_info game)
    {
      return _loadGame(ref game);
    }

    public bool LoadGameSpecial(uint gameType, ref retro_game_info game, uint numInfo)
    {
      return _loadGameSpecial(gameType, ref game, numInfo);
    }

    public void UnloadGame()
    {
      _unloadGame();
    }

    public uint GetRegion()
    {
      return _getRegion();
    }

    public IntPtr GetMemoryData(RETRO_MEMORY id)
    {
      return _getMemoryData(id);
    }

    public uint GetMemorySize(RETRO_MEMORY id)
    {
      return _getMemorySize(id);
    }

    #endregion
  }
}
