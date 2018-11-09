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
    protected IDictionary<uint, AbstractBuffer> _attachments = new Dictionary<uint, AbstractBuffer>();

    public Framebuffer(OpenGL gl) : base(gl)
    { }

    public void AddTexture(uint attachment, Texture2d texture)
    {
      _attachments[attachment] = texture;
      _gl.FramebufferTexture2DEXT(OpenGL.GL_FRAMEBUFFER_EXT, attachment, OpenGL.GL_TEXTURE_2D, texture.Id, 0);
    }

    public void AddRenderbuffer(uint attachment, Renderbuffer buffer)
    {
      _attachments[attachment] = buffer;
      _gl.FramebufferRenderbufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, attachment,
          OpenGL.GL_RENDERBUFFER_EXT, buffer.Id);
    }

    public ICollection<AbstractBuffer> Attachments
    {
      get { return _attachments.Values; }
    }

    public T GetAttachment<T>(uint attachment) where T : AbstractBuffer
    {
      return _attachments.TryGetValue(attachment, out AbstractBuffer buffer) ? buffer as T : null;
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
      _attachments.Clear();
      _gl.DeleteFramebuffersEXT(1, new[] { id });
    }
  }
}
