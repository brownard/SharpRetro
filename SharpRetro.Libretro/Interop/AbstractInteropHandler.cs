using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Environment;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpRetro.Libretro.Interop
{
  public abstract class AbstractInteropHandler : IInteropHandler
  {
    private IDictionary<string, IntPtr> _allocated;
    private bool _disposed = false;

    public AbstractInteropHandler()
    {
      _allocated = new Dictionary<string, IntPtr>();
    }

    public bool TryBoolToPtr(IntPtr ptr, TryGetDelegate<bool> valueDlgt)
    {
      bool value;
      if (!valueDlgt(out value))
        return false;
      WriteBoolToPtr(ptr, value);
      return true;
    }

    public bool TryStringToPtr(IntPtr ptr, TryGetDelegate<string> valueDlgt)
    {
      string value;
      if (!valueDlgt(out value))
        return false;
      WritePtrToPtr(ptr, AllocateString(value));
      return true;
    }

    public bool TryPtrToPtr(IntPtr ptr, IntPtr value)
    {
      WritePtrToPtr(ptr, value);
      return true;
    }

    public bool TrySetHWRenderCallback(IntPtr ptr, TrySetDelegate<retro_hw_render_callback> valueDlgt)
    {
      retro_hw_render_callback value = Marshal.PtrToStructure<retro_hw_render_callback>(ptr);
      if (!valueDlgt(ref value))
        return false;
      UpdateHWRenderCallback(ptr, value);
      return true;
    }

    public bool TrySetRumbleInterface(IntPtr ptr, TrySetDelegate<retro_rumble_interface> valueDlgt)
    {
      retro_rumble_interface value = Marshal.PtrToStructure<retro_rumble_interface>(ptr);
      if (!valueDlgt(ref value))
        return false;
      UpdateRumbleInterface(ptr, value);
      return true;
    }

    public abstract IList<IVariable> PtrToVariables(IntPtr ptr);

    public abstract bool TryGetVariable(IntPtr ptr, TryGetDelegate<string, string> valueDlgt);

    public abstract uint ReadUint(IntPtr ptr);

    public abstract int ReadInt(IntPtr ptr);

    protected abstract void WriteBoolToPtr(IntPtr ptr, bool value);

    protected abstract void WritePtrToPtr(IntPtr ptr, IntPtr value);

    protected abstract void UpdateHWRenderCallback(IntPtr ptr, retro_hw_render_callback value);

    protected abstract void UpdateRumbleInterface(IntPtr ptr, retro_rumble_interface value);

    protected IntPtr AllocateString(string value)
    {
      IntPtr ptr;
      if (!_allocated.TryGetValue(value, out ptr))
        _allocated[value] = ptr = Marshal.StringToHGlobalAnsi(value);
      return ptr;
    }

    #region IDisposable

    protected virtual void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        foreach (IntPtr ptr in _allocated.Values)
          Marshal.FreeHGlobal(ptr);
        _allocated.Clear();
        _disposed = true;
      }
    }
    
    ~AbstractInteropHandler()
    {
      Dispose(false);
    }
    
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion
  }
}
