using System;

namespace SharpRetro.Libretro.Native
{
  public interface ILibrary : IDisposable
  {
    IntPtr GetProcAddress(string procName);
  }
}