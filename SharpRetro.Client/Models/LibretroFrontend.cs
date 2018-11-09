using SharpRetro.DirectX.GL;
using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Emulators;
using SharpRetro.Libretro.Interop;
using SharpRetro.Libretro.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Client.Models
{
  class LibretroFrontend : IDisposable
  {
    protected ILibrary _library;
    protected LibretroEmulator _emulator;

    public void LoadCore(string path)
    {
      UnloadCore();

      _library = new Library(path);
    }


    public void UnloadCore()
    {
      if (_emulator == null)
        return;
      _emulator.Deinit();
      _emulator = null;

      _library?.Dispose();
      _library = null;
    }

    public void Dispose()
    {
      UnloadCore();
    }
  }
}
