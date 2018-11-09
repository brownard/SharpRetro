using SharpDX.Direct3D9;
using SharpRetro.Client.Audio;
using SharpRetro.Client.Models;
using SharpRetro.Client.Video;
using SharpRetro.DirectX.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SharpRetro.Client.ViewModels
{
  class EmulatorViewModel : ID3DContext, IDisposable
  {
    Direct3DContext _d3dContext;
    LibretroModel _libretro;

    public void InitLibretro()
    {
      _libretro = new LibretroModel();
      _libretro.LoadCore(@"E:\Games\Cores\mupen64plus_libretro_singlethread.dll", this, new NAudioOutput());
      _libretro.LoadGame(@"E:\Games\N64\Super Smash Bros. (E) (M3) [!].z64");
    }

    public void InitDirect3D(IntPtr controlHandle)
    {
      if (_d3dContext == null)
      {
        _d3dContext = Direct3DContext.Create(controlHandle);
        InitLibretro();
      }
    }

    public void Run()
    {
      if (_libretro != null)
        _libretro.Run();
    }

    public IntPtr GetBackBuffer(out Int32Rect dirtyRect)
    {
      if (_d3dContext != null)
        return _d3dContext.GetBackBuffer(out dirtyRect).NativePointer;
      dirtyRect = new Int32Rect();
      return IntPtr.Zero;
    }

    public Device Device
    {
      get { return _d3dContext?.Device; }
    }

    public Texture GetTexture(int width, int height, Usage usage)
    {
      if (_d3dContext == null)
        return null;
      return _d3dContext.GetTexture(width, height, usage);
    }

    public void Dispose()
    {
      if (_libretro != null)
        _libretro.UnloadCore();

      if (_d3dContext != null)
      {
        _d3dContext.Dispose();
        _d3dContext = null;
      }
    }
  }
}
