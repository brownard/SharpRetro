using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRetro.Native;

namespace SharpRetro.Frontend.Video
{
  public interface IHardwareRenderer
  {
    void FramebufferValid();
    uint GetCurrentFramebuffer();
    IntPtr GetProcAddress(IntPtr symbol);
    void SetContextReset(retro_hw_context_reset_t retro_hw_context_reset_t);
    void SetContextDestroy(retro_hw_context_reset_t retro_hw_context_reset_t);
  }
}
