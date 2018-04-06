using SharpRetro.Libretro.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Libretro.Environment
{
  public class SystemInfo
  {
    public string LibraryName { get; set; }
    public string LibraryVersion { get; set; }
    public string ValidExtensions { get; set; }
    public bool NeedFullPath { get; set; }
    public bool BlockExtract { get; set; }
  }
}
