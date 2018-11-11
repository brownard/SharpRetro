using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace SharpRetro.Native
{
  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
  public sealed class SafeStringHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    public SafeStringHandle(string value) : base(true)
    {
      SetHandle(Marshal.StringToHGlobalAnsi(value));
    }

    protected override bool ReleaseHandle()
    {
      Marshal.FreeHGlobal(handle);
      return true;
    }
  }
}
