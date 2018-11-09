using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;
using SharpRetro.DirectX.Video;
using System;
using System.Windows;

namespace SharpRetro.Client.Video
{
  class Direct3DContext : IDisposable
  {
    public static Direct3DContext Create(IntPtr controlHandle)
    {
      Direct3DEx direct3D = new Direct3DEx();
      DeviceEx device = new DeviceEx(direct3D, 0, DeviceType.Hardware, controlHandle, CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded,
        new PresentParameters(512, 512));
      return new Direct3DContext(direct3D, device);
    }

    protected Direct3DEx _direct3D;
    protected DeviceEx _device;
    Surface _backBuffer;
    Texture _texture;

    public Direct3DContext(Direct3DEx direct3D, DeviceEx device)
    {
      _direct3D = direct3D;
      _device = device;
    }

    public Direct3DEx Direct3D
    {
      get { return _direct3D; }
    }

    public DeviceEx Device
    {
      get { return _device; }
    }

    public Texture GetTexture(int width, int height, Usage usage)
    {
      if (_texture != null)
      {
        SurfaceDescription surface = _texture.GetLevelDescription(0);
        if (surface.Width >= width && surface.Height >= height && surface.Usage == usage)
          return _texture;
        _texture.Dispose();
      }
      _texture = new Texture(_device, width, height, 1, usage, Format.X8R8G8B8, Pool.Default);
      return _texture;
    }

    public Surface GetBackBuffer(out Int32Rect dirtyRect)
    {
      if (_backBuffer == null)
        _backBuffer = Surface.CreateRenderTargetEx(_device, 512, 512, Format.X8R8G8B8, MultisampleType.None, 0, true, Usage.None);
      if (_texture != null)
      {
        var description = _texture.GetLevelDescription(0);
        _device.StretchRectangle(_texture.GetSurfaceLevel(0), null, _backBuffer, new RawRectangle(0, 0, _backBuffer.Description.Width, _backBuffer.Description.Height), TextureFilter.None);
      }
      dirtyRect = new Int32Rect(0, 0, 512, 512);
      return _backBuffer;
    }

    public void Dispose()
    {
      if (_texture != null)
      {
        _texture.Dispose();
        _texture = null;
      }
      if (_backBuffer != null)
      {
        _backBuffer.Dispose();
        _backBuffer = null;
      }
      if (_device != null)
      {
        _device.Dispose();
        _device = null;
      }
      if (_direct3D != null)
      {
        _direct3D.Dispose();
        _direct3D = null;
      }
    }
  }
}
