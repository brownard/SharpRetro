using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRetro.Libretro.Cores;

namespace SharpRetro.Libretro.Input
{
  public interface IKeyboard
  {
    bool IsKeyPressed(RETRO_KEY key);
  }
}
