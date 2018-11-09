using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Libretro.Input
{
  public static class InputUtils
  {
    public const short TRUE = 1;
    public const short FALSE = 0;
    public const short MAX = short.MaxValue;

    public static short Negate(short value)
    {
      return (short)(-value - 1);
    }

    public static short Scale(byte value)
    {
      if (value == 0)
        return 0;
      return (short)(((value << 8) | value) >> 1);
    }
  }
}
