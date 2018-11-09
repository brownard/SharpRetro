using SharpDX.Direct3D9;
using SharpGL.Version;
using SharpRetro.DirectX.Video;
using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Environment;
using System;
using System.Runtime.InteropServices;

namespace SharpRetro.DirectX.GL
{
  public class DXRenderContext : IRenderContext, IDisposable
  {
    protected retro_hw_get_current_framebuffer_t _getCurrentFramebufferDlgt;
    protected retro_hw_get_proc_address_t _getProcAddressDlgt;
    protected retro_hw_context_reset_t _contextReset;
    protected retro_hw_context_reset_t _contextDestroy;

    protected Device _device;
    protected OpenGLEx _glEx;
    protected DXRenderContextProvider _glContext;
    protected Geometry _geometry;

    public DXRenderContext(Device device)
    {
      _device = device;
    }

    public bool Init(ref retro_hw_render_callback renderCallback)
    {
      if (_glContext != null)
        _glContext.Destroy();

      _glEx = new OpenGLEx();
      _glContext = new DXRenderContextProvider(_device, _glEx, renderCallback.depth, renderCallback.stencil, renderCallback.bottom_left_origin);

      _getCurrentFramebufferDlgt = new retro_hw_get_current_framebuffer_t(_glContext.GetCurrentFramebuffer);
      renderCallback.get_current_framebuffer = Marshal.GetFunctionPointerForDelegate(_getCurrentFramebufferDlgt);

      _getProcAddressDlgt = new retro_hw_get_proc_address_t(_glContext.GetProcAddress);
      renderCallback.get_proc_address = Marshal.GetFunctionPointerForDelegate(_getProcAddressDlgt);

      if (renderCallback.context_reset != IntPtr.Zero)
        _contextReset = Marshal.GetDelegateForFunctionPointer<retro_hw_context_reset_t>(renderCallback.context_reset);
      if(renderCallback.context_destroy != IntPtr.Zero)
        _contextDestroy = Marshal.GetDelegateForFunctionPointer<retro_hw_context_reset_t>(renderCallback.context_reset);

      if (_geometry != null)
      {
        Create();
        _contextReset?.Invoke();
      }
      return true;
    }

    public void SetGeometry(Geometry geometry)
    {
      _geometry = geometry;
      if (_glContext == null)
        return;

      if (!_glContext.Created)
        Create();
      else
        _glContext.SetDimensions(geometry.MaxWidth, geometry.MaxHeight);
      _contextReset?.Invoke();
    }

    public bool OnFramebufferReady(ID3DContext textureProvider, int width, int height, int pitch)
    {
      _glContext.Render(textureProvider, width, height, pitch);
      return true;
    }

    protected bool Create()
    {
       return _glContext.Create(OpenGLVersion.OpenGL2_1, _glEx, _geometry.MaxWidth, _geometry.MaxHeight, 32, null);
    }

    public void Dispose()
    {
      if (_glContext != null)
      {
        _glContext.Destroy();
        _glContext = null;
      }
    }
  }
}
