using SharpRetro.Cores;
using SharpRetro.Frontend.Audio;
using SharpRetro.Frontend.Environment;
using SharpRetro.Frontend.Input;
using SharpRetro.Frontend.Logging;
using SharpRetro.Frontend.Video;
using SharpRetro.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Frontend
{
  public class Frontend : IDisposable
  {
    protected ICore _core;
    protected IEnvironmentManager _environmentManager;
    protected FrontendEnvironment _environment;

    protected IAudioOutput _audioOutput;
    protected IVideoOutput _videoOutput;
    protected IInput _input;

    protected LogInterfaceHandler _logInterfaceHandler;
    protected ILogger _logger;

    public Frontend()
    {
      _environmentManager = new EnvironmentManager();
      _environment = new FrontendEnvironment();
      _environment.Attach(_environmentManager);

      _logger = new ConsoleLogger();
      _logInterfaceHandler = new LogInterfaceHandler(_logger);
      _logInterfaceHandler.Attach(_environmentManager);
    }

    public IAudioOutput AudioOutput
    {
      get { return _audioOutput; }
      set
      {
        DetachEnvironmentHandler(_audioOutput as IEnvironmentHandler);
        AttachEnvironmentHandler(value as IEnvironmentHandler);
        _audioOutput = value;
      }
    }

    public IVideoOutput VideoOutput
    {
      get { return _videoOutput; }
      set
      {
        DetachEnvironmentHandler(_videoOutput as IEnvironmentHandler);
        AttachEnvironmentHandler(value as IEnvironmentHandler);
        _videoOutput = value;
      }
    }

    public IInput Input
    {
      get { return _input; }
      set
      {
        DetachEnvironmentHandler(_input as IEnvironmentHandler);
        AttachEnvironmentHandler(value as IEnvironmentHandler);
        _input = value;
      }
    }

    public void LoadCore(ICore core)
    {
      UnloadCore();
      _core = core;
      InitCallbacks();
      _core.Init();
    }

    public void UnloadCore()
    {
      if (_core == null)
        return;
      _core.Deinit();
      _core.Dispose();
      _core = null;
    }

    public void Run()
    {

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

    private bool OnEnvironment(RETRO_ENVIRONMENT cmd, IntPtr data)
    {
      return _environmentManager.Invoke((int)cmd, data);
    }

    private void OnAudioSample(short left, short right)
    {
      _audioOutput?.OnAudioSample(left, right);
    }

    private uint OnAudioSampleBatch(IntPtr data, uint frames)
    {
      return _audioOutput?.OnAudioSampleBatch(data, frames) ?? 0;
    }

    private void OnVideoRefresh(IntPtr data, uint width, uint height, uint pitch)
    {
      _videoOutput?.OnVideoRefresh(data, width, height, pitch);
    }

    private void OnInputPoll()
    {
      _input?.OnInputPoll();
    }

    private short OnInputState(uint port, uint device, uint index, uint id)
    {
      return _input?.OnInputState(port, device, index, id) ?? 0;
    }

    public void AttachEnvironmentHandler(IEnvironmentHandler handler)
    {
      if (handler != null)
        handler.Attach(_environmentManager);
    }

    public void DetachEnvironmentHandler(IEnvironmentHandler handler)
    {
      if (handler != null)
        handler.Detach(_environmentManager);
    }

    public void Dispose()
    {
      _environmentManager.Dispose();
    }
  }
}
