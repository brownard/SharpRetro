using SharpRetro.Frontend.Environment;
using SharpRetro.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Frontend.Video
{
  public class VideoOutput : IVideoOutput, IEnvironmentHandler
  {
    protected RenderCallbackHandler _renderCallbackHandler;

    protected ISoftwareRenderer _softwareRenderer;
    protected IHardwareRenderer _hardwareRenderer;

    public VideoOutput(ISoftwareRenderer softwareRenderer, IHardwareRenderer hardwareRenderer)
    {
      _softwareRenderer = softwareRenderer;
      _hardwareRenderer = hardwareRenderer;

      if (_hardwareRenderer != null)
        _renderCallbackHandler = new RenderCallbackHandler(_hardwareRenderer);
    }

    public void OnVideoRefresh(IntPtr data, uint width, uint height, uint pitch)
    {
      if (data.ToInt32() == retro_hw_render_callback.RETRO_HW_FRAME_BUFFER_VALID)
      {
        if (_hardwareRenderer != null)
          _hardwareRenderer.FramebufferValid();
      }
      else
      {
        if (_softwareRenderer != null)
          _softwareRenderer.VideoRefresh(data, width, height, pitch);
      }
    }

    public void Attach(IEnvironmentManager environmentManager)
    {
      if (_renderCallbackHandler != null)
        _renderCallbackHandler.Attach(environmentManager);
    }

    public void Detach(IEnvironmentManager environmentManager)
    {
      if (_renderCallbackHandler != null)
        _renderCallbackHandler.Detach(environmentManager);
    }
  }
}
