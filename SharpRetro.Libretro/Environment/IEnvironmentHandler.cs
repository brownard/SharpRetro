using System;
using System.Collections.Generic;
using SharpRetro.Libretro.Cores;

namespace SharpRetro.Libretro.Environment
{
  public interface IEnvironmentHandler
  {
    bool SetRotation(uint rotation);
    bool GetOverscan(out bool overscan);
    bool GetCanDupe(out bool canDupe);
    bool SetMessage(retro_message message);
    bool SetPerformanceLevel(uint level);
    bool GetSystemDirectory(out string directory);
    bool SetInputDescriptors(retro_input_descriptor descriptor);
    bool SetKeyboardCallback(retro_keyboard_callback callback);
    bool GetVariable(string key, out string value);
    bool SetVariables(IEnumerable<IVariable> variables);
    bool GetVariableUpdate();
    bool SetSupportNoGame();
    bool GetLibretroPath(out string path);
    bool GetSaveDirectory(out string directory);
    bool SetSubsytemInfo(retro_subsystem_info info);
    bool OnUnhandledEnvironmentCommand(int command, IntPtr data);
  }
}
