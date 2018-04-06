using System;

namespace SharpRetro.Log
{
  public class ConsoleLogger : ILogger
  {
    public void Debug(string format, params object[] args)
    {
      WriteLine("Debug: ", format, args);
    }

    public void Error(string format, params object[] args)
    {
      WriteLine("Error: ", format, args);
    }

    public void Info(string format, params object[] args)
    {
      WriteLine("Info: ", format, args);
    }

    public void Warn(string format, params object[] args)
    {
      WriteLine("Warn: ", format, args);
    }

    protected void WriteLine(string prefix, string format, params object[] args)
    {
      if (args != null && args.Length > 0)
        Console.WriteLine(prefix + format, args);
      else
        Console.WriteLine(prefix + format);
    }
  }
}
