using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Frontend.Input
{
  public class Input : IInput
  {
    public void OnInputPoll()
    {
    }

    public short OnInputState(uint port, uint device, uint index, uint id)
    {
      return 0;
    }
  }
}
