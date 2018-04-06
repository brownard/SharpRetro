using SharpDX.Direct3D9;

namespace SharpRetro.DirectX.Video
{
  public interface ITextureProvider
  {
    Texture GetTexture(int width, int height, int pitch);
  }
}
