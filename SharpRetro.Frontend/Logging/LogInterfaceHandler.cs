using SharpRetro.Frontend.Environment;
using SharpRetro.Native;
using System;
using System.Runtime.InteropServices;

namespace SharpRetro.Frontend.Logging
{
  public class LogInterfaceHandler : IEnvironmentHandler
  {
    protected retro_log_printf_t _logInterface;
    protected ILogger _logger;

    public LogInterfaceHandler(ILogger logger)
    {
      _logInterface = new retro_log_printf_t(OnLogPrintf);
      _logger = logger;
    }

    public void Attach(IEnvironmentManager environmentManager)
    {
      environmentManager.AddDelegate((int)RETRO_ENVIRONMENT.GET_LOG_INTERFACE, GetLogInterface);
    }

    public void Detach(IEnvironmentManager environmentManager)
    {
      environmentManager.RemoveDelegate((int)RETRO_ENVIRONMENT.GET_LOG_INTERFACE, GetLogInterface);
    }

    protected bool GetLogInterface(int cmd, IntPtr data)
    {
      IntPtr ptr = Marshal.GetFunctionPointerForDelegate(_logInterface);
      Marshal.WriteIntPtr(data, ptr);
      return true;
    }

    private void OnLogPrintf(RETRO_LOG_LEVEL level, string fmt, IntPtr a0, IntPtr a1, IntPtr a2, IntPtr a3, IntPtr a4, IntPtr a5, IntPtr a6, IntPtr a7, IntPtr a8, IntPtr a9, IntPtr a10, IntPtr a11, IntPtr a12, IntPtr a13, IntPtr a14, IntPtr a15)
    {
      IntPtr[] args = new IntPtr[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15 };
      string message = Printf(fmt, args);
      _logger.Log(level, message);
    }

    protected string Printf(string format, IntPtr[] args)
    {
      string message;
      try
      {
        int idx = 0;
        message = Sprintf.sprintf(format, () => args[idx++]);
      }
      catch (Exception ex)
      {
        message = string.Format("Error in sprintf - {0}", ex);
      }
      return message;
    }
  }
}
