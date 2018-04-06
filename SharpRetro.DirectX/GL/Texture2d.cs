using SharpGL;

namespace SharpRetro.DirectX.GL
{
  public class Texture2d : AbstractBuffer
  {
    public Texture2d(OpenGL gl) : base(gl)
    { }

    public void AddStorage(uint internalFormat, int width, int height)
    {
      _gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, internalFormat, width, height, 0, internalFormat, OpenGL.GL_UNSIGNED_BYTE, null);
      _gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, new[] { OpenGL.GL_NEAREST });
      _gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, new[] { OpenGL.GL_NEAREST });
    }

    protected override void Create(uint count, uint[] ids)
    {
      _gl.GenTextures((int)count, ids);
    }

    protected override void Bind(uint id)
    {
      _gl.BindTexture(OpenGL.GL_TEXTURE_2D, id);
    }

    protected override void Destroy(uint id)
    {
      _gl.DeleteTextures(1, new[] { id });
    }
  }
}
