using SharpRetro.Native;
using System;

namespace SharpRetro.Frontend.Logging
{
  public class ConsoleLogger : ILogger
  {
    public void Log(RETRO_LOG_LEVEL level, string message)
    {
      string levelName = Enum.GetName(typeof(RETRO_LOG_LEVEL), level);
      Console.WriteLine("{0}: {1}", levelName, message);
    }
  }
}
