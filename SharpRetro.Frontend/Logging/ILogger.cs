using SharpRetro.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Frontend.Logging
{
  public interface ILogger
  {
    void Log(RETRO_LOG_LEVEL level, string message);
  }
}
