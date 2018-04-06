using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Games
{
  public class Game : IGame
  {
    public string Path { get; set; }
    public byte[] Data { get; set; }
  }
}
