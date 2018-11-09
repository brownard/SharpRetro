using System;

namespace SharpRetro.Libretro.Native
{
  public interface ILibrary : IDisposable
  {
    Delegate GetProcDelegate(string procName, Type type);
  }
}