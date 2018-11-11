using SharpRetro.Frontend.Environment;
using SharpRetro.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Frontend.Video
{
  public class VideoOutput : IVideoOutput, IHardwareRender, IEnvironmentHandler
  {
    protected RenderCallbackHandler _renderCallbackHandler;

    public VideoOutput()
    {
      _renderCallbackHandler = new RenderCallbackHandler(this);
    }

    public void OnVideoRefresh(IntPtr data, uint width, uint height, uint pitch)
    {
      //if (data.ToInt32() == retro_hw_render_callback.RETRO_HW_FRAME_BUFFER_VALID)
    }

    public void Attach(IEnvironmentManager environmentManager)
    {
      _renderCallbackHandler.Attach(environmentManager);
    }

    public void Detach(IEnvironmentManager environmentManager)
    {
      _renderCallbackHandler.Detach(environmentManager);
    }

    public uint GetCurrentFramebuffer()
    {
      throw new NotImplementedException();
    }

    public IntPtr GetProcAddress(IntPtr symbol)
    {
      throw new NotImplementedException();
    }

    public void SetContextReset(retro_hw_context_reset_t retro_hw_context_reset_t)
    {
      throw new NotImplementedException();
    }

    public void SetContextDestroy(retro_hw_context_reset_t retro_hw_context_reset_t)
    {
      throw new NotImplementedException();
    }
  }
}
