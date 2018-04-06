using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Environment;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpRetro.Libretro.Interop
{
  public class SafeInteropHandler : AbstractInteropHandler
  {
    public override IList<IVariable> PtrToVariables(IntPtr ptr)
    {
      IList<IVariable> variables = new List<IVariable>();
      retro_variable variable = Marshal.PtrToStructure<retro_variable>(ptr);
      while (variable.key != IntPtr.Zero)
      {
        variables.Add(new Variable(Marshal.PtrToStringAnsi(variable.key), Marshal.PtrToStringAnsi(variable.value)));
        ptr = IntPtr.Add(ptr, Marshal.SizeOf(variable));
        variable = Marshal.PtrToStructure<retro_variable>(ptr);
      }
      return variables;
    }

    public override bool TryGetVariable(IntPtr ptr, TryGetDelegate<string, string> valueDlgt)
    {
      retro_variable variable = Marshal.PtrToStructure<retro_variable>(ptr);
      string value;
      if (!valueDlgt(Marshal.PtrToStringAnsi(variable.key), out value))
        return false;
      variable.value = AllocateString(value);
      Marshal.StructureToPtr(variable, ptr, false);
      return true;
    }

    public override uint ReadUint(IntPtr ptr)
    {
      return (uint)ReadInt(ptr);
    }

    public override int ReadInt(IntPtr ptr)
    {
      return Marshal.ReadInt32(ptr);
    }

    protected override void WriteBoolToPtr(IntPtr ptr, bool value)
    {
      WritePtrToPtr(ptr, (IntPtr)(value ? 1 : 0));
    }

    protected override void WritePtrToPtr(IntPtr ptr, IntPtr value)
    {
      Marshal.WriteIntPtr(ptr, value);
    }

    protected override void UpdateHWRenderCallback(IntPtr ptr, retro_hw_render_callback value)
    {
      Marshal.StructureToPtr(value, ptr, false);
    }

    protected override void UpdateRumbleInterface(IntPtr ptr, retro_rumble_interface value)
    {
      Marshal.StructureToPtr(value, ptr, false);
    }
  }
}
