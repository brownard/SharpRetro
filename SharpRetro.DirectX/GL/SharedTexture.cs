using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.DirectX.GL
{
  public class SharedTexture : Texture2d
  {
    protected OpenGLEx _glEx;
    protected IntPtr _glDeviceHandle;
    protected IntPtr _glTextureHandle;

    public SharedTexture(OpenGLEx glEx, IntPtr glDeviceHandle)
      : base(glEx)
    {
      _glEx = glEx;
      _glDeviceHandle = glDeviceHandle;
    }

    public void AddSharedTexture(IntPtr textureHandle, IntPtr sharedResourceHandle)
    {
      _glTextureHandle = _glEx.DXRegisterObjectNV(_glDeviceHandle, textureHandle, Id, OpenGL.GL_TEXTURE_2D, OpenGLEx.WGL_ACCESS_WRITE_DISCARD_NV);
    }

    public void Lock()
    {
      _glEx.DXLockObjectsNV(_glDeviceHandle, new[] { _glTextureHandle });
    }

    public void Unlock()
    {
      _glEx.DXUnlockObjectsNV(_glDeviceHandle, new[] { _glTextureHandle });
    }

    protected override void Destroy(uint id)
    {
      Unlock();
      _glEx.DXUnregisterObjectNV(_glDeviceHandle, _glTextureHandle);
      base.Destroy(id);
    }
  }
}
