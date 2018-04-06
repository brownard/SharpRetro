using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D9;
using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Environment;

namespace SharpRetro.DirectX.GL
{
  /// <summary>
  /// 
  /// </summary>
  public interface IRenderContext
  {
    void Init(retro_hw_render_callback renderCallback);
    void SetGeometry(Geometry geometry);
    uint GetCurrentFramebuffer();
    IntPtr GetProcAddress(IntPtr sym);
    bool OnFramebufferReady(Texture texture);
  }
}
