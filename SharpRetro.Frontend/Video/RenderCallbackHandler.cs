using SharpRetro.Frontend.Environment;
using SharpRetro.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Frontend.Video
{
  public class RenderCallbackHandler : IEnvironmentHandler
  {
    IHardwareRender _render;

    protected retro_hw_get_current_framebuffer_t _getCurrentFramebufferDlgt;
    protected retro_hw_get_proc_address_t _getProcAddressDlgt;

    public RenderCallbackHandler(IHardwareRender render)
    {
      _render = render;
      _getCurrentFramebufferDlgt = new retro_hw_get_current_framebuffer_t(OnGetCurrentFramebuffer);
      _getProcAddressDlgt = new retro_hw_get_proc_address_t(OnGetProcAddress);
    }

    public void Attach(IEnvironmentManager environmentManager)
    {
      environmentManager.AddDelegate((int)RETRO_ENVIRONMENT.SET_HW_RENDER, SetHardwareRenderer);
    }

    public void Detach(IEnvironmentManager environmentManager)
    {
      environmentManager.RemoveDelegate((int)RETRO_ENVIRONMENT.SET_HW_RENDER, SetHardwareRenderer);
    }

    private bool SetHardwareRenderer(int cmd, IntPtr data)
    {
      unsafe
      {
        retro_hw_render_callback* renderCallback = (retro_hw_render_callback*)data.ToPointer();
        renderCallback->get_current_framebuffer = Marshal.GetFunctionPointerForDelegate(_getCurrentFramebufferDlgt);
        renderCallback->get_proc_address = Marshal.GetFunctionPointerForDelegate(_getProcAddressDlgt);
        if (renderCallback->context_reset != IntPtr.Zero)
          _render.SetContextReset(Marshal.GetDelegateForFunctionPointer<retro_hw_context_reset_t>(renderCallback->context_reset));
        if (renderCallback->context_destroy != IntPtr.Zero)
          _render.SetContextDestroy(Marshal.GetDelegateForFunctionPointer<retro_hw_context_reset_t>(renderCallback->context_destroy));
      }
      return true;
    }

    protected uint OnGetCurrentFramebuffer()
    {
      return _render.GetCurrentFramebuffer();
    }

    protected IntPtr OnGetProcAddress(IntPtr symbol)
    {
      return _render.GetProcAddress(symbol);
    }
  }
}
