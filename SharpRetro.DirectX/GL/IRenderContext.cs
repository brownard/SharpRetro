using SharpRetro.DirectX.Video;
using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Environment;

namespace SharpRetro.DirectX.GL
{
  public interface IRenderContext
  {
    bool Init(ref retro_hw_render_callback renderCallback);
    void SetGeometry(Geometry geometry);
    bool OnFramebufferReady(ID3DContext textureProvider, int width, int height, int pitch);
  }
}
