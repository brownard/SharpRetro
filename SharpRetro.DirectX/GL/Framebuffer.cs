using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.DirectX.GL
{
  public class Framebuffer : AbstractBuffer
  {
    public Framebuffer(OpenGL gl) : base(gl)
    { }

    public void AddTexture(uint attachment, Texture2d texture)
    {
      _gl.FramebufferTexture(OpenGL.GL_FRAMEBUFFER_EXT, attachment, texture.Id, 0);
    }

    public void AddRenderbuffer(uint attachment, Renderbuffer buffer)
    {
      _gl.FramebufferRenderbufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, attachment,
          OpenGL.GL_RENDERBUFFER_EXT, buffer.Id);
    }

    protected override void Create(uint count, uint[] ids)
    {
      _gl.GenFramebuffersEXT(count, ids);
    }

    protected override void Bind(uint id)
    {
      _gl.BindFramebufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, id);
    }

    protected override void Destroy(uint id)
    {
      _gl.DeleteFramebuffersEXT(1, new[] { id });
    }
  }
}
