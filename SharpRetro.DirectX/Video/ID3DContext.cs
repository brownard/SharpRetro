using SharpDX.Direct3D9;

namespace SharpRetro.DirectX.Video
{
  public interface ID3DContext
  {
    Device Device { get; }
    Texture GetTexture(int width, int height, Usage usage);
  }
}
