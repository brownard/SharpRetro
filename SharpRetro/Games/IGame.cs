using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Games
{
  public interface IGame
  {
    string Path { get; }
    byte[] Data { get; }
  }
}
