using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Environment;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpRetro.Libretro.Interop
{
  public class UnsafeInteropHandler : AbstractInteropHandler
  {
    public override IList<IVariable> PtrToVariables(IntPtr ptr)
    {
      IList<IVariable> variables = new List<IVariable>();
      unsafe
      {
        void** variablesPtr = (void**)ptr.ToPointer();
        while (true)
        {
          IntPtr pKey = new IntPtr(*variablesPtr++);
          if (pKey == IntPtr.Zero)
            break;
          IntPtr pValue = new IntPtr(*variablesPtr++);
          variables.Add(new Variable(Marshal.PtrToStringAnsi(pKey), Marshal.PtrToStringAnsi(pValue)));
        }
      }
      return variables;
    }

    public override bool TryGetVariable(IntPtr ptr, TryGetDelegate<string, string> valueDlgt)
    {
      unsafe
      {
        retro_variable* variable = (retro_variable*)ptr.ToPointer();
        string key = Marshal.PtrToStringAnsi(variable->key);
        if (!valueDlgt(key, out string value))
          return false;
        variable->value = AllocateString(value);
        return true;
      }
    }

    public override uint ReadUint(IntPtr ptr)
    {
      unsafe
      {
        return *(uint*)ptr.ToPointer();
      }
    }

    public override int ReadInt(IntPtr ptr)
    {
      unsafe
      {
        return *(int*)ptr.ToPointer();
      }
    }

    protected override void WriteBoolToPtr(IntPtr ptr, bool value)
    {
      unsafe
      {
        *(bool*)ptr.ToPointer() = value;
      }
    }

    protected override void WritePtrToPtr(IntPtr ptr, IntPtr value)
    {
      unsafe
      {
        *((IntPtr*)ptr.ToPointer()) = value;
      }
    }

    protected override void UpdateHWRenderCallback(IntPtr ptr, retro_hw_render_callback value)
    {
      unsafe
      {
        retro_hw_render_callback* render = (retro_hw_render_callback*)ptr.ToPointer();
        render->get_current_framebuffer = value.get_current_framebuffer;
        render->get_proc_address = value.get_proc_address;
      }
    }

    protected override void UpdateRumbleInterface(IntPtr ptr, retro_rumble_interface value)
    {
      unsafe
      {
        retro_rumble_interface* rumble = (retro_rumble_interface*)ptr.ToPointer();
        rumble->set_rumble_state = value.set_rumble_state;
      }
    }
  }
}
