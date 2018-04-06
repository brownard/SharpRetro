using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRetro.Libretro.Cores;

namespace SharpRetro.Libretro.Input
{
  public interface IPointer : IInputDevice
  {
    short GetPointerX();
    short GetPointerY();
    bool IsPointerPressed();
  }
}
