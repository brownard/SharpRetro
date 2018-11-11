using Moq;
using NUnit.Framework;
using SharpRetro.Frontend.Environment;
using SharpRetro.Frontend.Video;
using SharpRetro.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Tests
{
  public class VideoOutput
  {
    [Test]
    public void RenderCallback_GetCurrentFramebuffer()
    {
      EnvironmentManager manager = new EnvironmentManager();

      Mock<IHardwareRenderer> mockRenderer = new Mock<IHardwareRenderer>();
      mockRenderer.Setup(r => r.GetCurrentFramebuffer()).Returns(1);

      RenderCallbackHandler handler = new RenderCallbackHandler(mockRenderer.Object);
      handler.Attach(manager);

      retro_hw_render_callback cb = new retro_hw_render_callback();
      IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(cb));
      try
      {
        Marshal.StructureToPtr(cb, ptr, false);
        manager.Invoke((int)RETRO_ENVIRONMENT.SET_HW_RENDER, ptr);
        cb = Marshal.PtrToStructure<retro_hw_render_callback>(ptr);
      }
      finally
      {
        Marshal.FreeHGlobal(ptr);
      }

      retro_hw_get_current_framebuffer_t dlgt = Marshal.GetDelegateForFunctionPointer<retro_hw_get_current_framebuffer_t>(cb.get_current_framebuffer);
      Assert.AreEqual(1, dlgt.Invoke());
    }
  }
}
