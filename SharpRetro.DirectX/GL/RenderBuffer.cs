using SharpGL;

namespace SharpRetro.DirectX.GL
{
  public class Renderbuffer : AbstractBuffer
  {
    public Renderbuffer(OpenGL gl) : base(gl)
    { }

    public void AddStorage(uint internalFormat, int width, int height)
    {
      _gl.RenderbufferStorageEXT(OpenGL.GL_RENDERBUFFER_EXT, internalFormat, width, height);
    }

    protected override void Create(uint count, uint[] ids)
    {
      _gl.GenRenderbuffersEXT(count, ids);
    }

    protected override void Bind(uint id)
    {
      _gl.BindRenderbufferEXT(OpenGL.GL_RENDERBUFFER_EXT, id);
    }

    protected override void Destroy(uint id)
    {
      _gl.DeleteFramebuffersEXT(1, new uint[] { id });
    }
  }
}
