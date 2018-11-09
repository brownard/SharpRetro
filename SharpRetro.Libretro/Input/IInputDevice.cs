using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Libretro.Input
{
  public interface IInputDevice
  {
    void Poll();
  }
}
