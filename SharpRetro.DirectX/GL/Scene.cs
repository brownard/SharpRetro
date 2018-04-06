using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.DirectX.GL
{
  class Scene
  {
    public static void RenderQuad(OpenGL gl, Framebuffer output, Texture2d texture, int width, int height, bool bottomLeftOrigin)
    {
      gl.PushAttrib(OpenGL.GL_TEXTURE_BIT | OpenGL.GL_DEPTH_TEST | OpenGL.GL_LIGHTING);
      gl.Disable(OpenGL.GL_DEPTH_TEST);
      gl.Disable(OpenGL.GL_LIGHTING);
      gl.UseProgram(0);

      gl.MatrixMode(SharpGL.Enumerations.MatrixMode.Projection);
      gl.PushMatrix();
      gl.LoadIdentity();

      if (bottomLeftOrigin)
        gl.Scale(1, -1, 1);

      gl.Ortho(0, width, 0, height, -1, 1);

      gl.MatrixMode(SharpGL.Enumerations.MatrixMode.Modelview);
      gl.PushMatrix();
      gl.LoadIdentity();
      
      gl.BindFramebufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, output.Id);

      gl.Enable(OpenGL.GL_TEXTURE_2D);
      gl.BindTexture(OpenGL.GL_TEXTURE_2D, texture.Id);

      // Draw a textured quad
      gl.Begin(OpenGL.GL_QUADS);
      gl.TexCoord(0, 0); gl.Vertex(0, 0, 0);
      gl.TexCoord(0, 1); gl.Vertex(0, height, 0);
      gl.TexCoord(1, 1); gl.Vertex(width, height, 0);
      gl.TexCoord(1, 0); gl.Vertex(width, 0, 0);
      gl.End();

      gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
      gl.Disable(OpenGL.GL_TEXTURE_2D);

      gl.BindFramebufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, 0);

      gl.MatrixMode(SharpGL.Enumerations.MatrixMode.Projection);
      gl.PopMatrix();

      gl.MatrixMode(SharpGL.Enumerations.MatrixMode.Modelview);
      gl.PopMatrix();

      gl.PopAttrib();
    }
  }
}
