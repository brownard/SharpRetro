using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Environment;
using System;
using System.Collections.Generic;

namespace SharpRetro.Libretro.Interop
{
  public delegate bool TryGetDelegate<T>(out T value);
  public delegate bool TrySetDelegate<T>(ref T value);
  public delegate bool TryGetDelegate<TKey, TValue>(TKey key, out TValue value);

  public interface IInteropHandler : IDisposable
  {
    bool TryBoolToPtr(IntPtr ptr, TryGetDelegate<bool> valueDlgt);
    bool TryStringToPtr(IntPtr ptr, TryGetDelegate<string> valueDlgt);
    bool TryPtrToPtr(IntPtr ptr, IntPtr value);
    IList<IVariable> PtrToVariables(IntPtr ptr);
    bool TryGetVariable(IntPtr ptr, TryGetDelegate<string, string> valueDlgt);
    bool TrySetHWRenderCallback(IntPtr ptr, TrySetDelegate<retro_hw_render_callback> valueDlgt);
    bool TrySetRumbleInterface(IntPtr ptr, TrySetDelegate<retro_rumble_interface> valueDlgt);
    uint ReadUint(IntPtr ptr);
    int ReadInt(IntPtr ptr);
  }
}