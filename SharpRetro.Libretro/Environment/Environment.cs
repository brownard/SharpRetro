using SharpRetro.Libretro.Cores;
using System;
using System.Collections.Generic;

namespace SharpRetro.Libretro.Environment
{
  public class Environment : IEnvironmentHandler
  {
    protected IDictionary<string, IVariable> _variables = new Dictionary<string, IVariable>();
    protected bool _variablesChanged;

    public bool CanDupe { get; set; }
    public bool Overscan { get; set; }
    public string LibretroPath { get; set; }
    public string SaveDirectory { get; set; }
    public string SystemDirectory { get; set; }

    public uint PerformanceLevel { get; private set; }
    public uint Rotation { get; private set; }
    public retro_subsystem_info SubsystemInfo { get; private set; }
    public bool SupportsNoGame { get; private set; }

    public bool GetCanDupe(out bool canDupe)
    {
      canDupe = CanDupe;
      return true;
    }

    public bool GetLibretroPath(out string path)
    {
      path = LibretroPath;
      return !string.IsNullOrEmpty(path);
    }

    public bool GetOverscan(out bool overscan)
    {
      overscan = Overscan;
      return true;
    }

    public bool GetSaveDirectory(out string directory)
    {
      directory = SaveDirectory;
      return !string.IsNullOrEmpty(directory);
    }

    public bool GetSystemDirectory(out string directory)
    {
      directory = SystemDirectory;
      return !string.IsNullOrEmpty(directory);
    }

    public bool GetVariable(string key, out string value)
    {
      if (_variables.TryGetValue(key, out IVariable variable))
      {
        value = variable.SelectedValue;
        return true;
      }
      value = null;
      return false;
    }

    public bool GetVariableUpdate()
    {
      if (!_variablesChanged)
        return false;
      _variablesChanged = false;
      return true;
    }

    public bool OnUnhandledEnvironmentCommand(int command, IntPtr data)
    {
      return false;
    }

    public bool SetInputDescriptors(retro_input_descriptor descriptor)
    {
      return true;
    }

    public bool SetKeyboardCallback(retro_keyboard_callback callback)
    {
      return true;
    }

    public bool SetMessage(retro_message message)
    {
      return true;
    }

    public bool SetPerformanceLevel(uint level)
    {
      PerformanceLevel = level;
      return true;
    }
    
    public bool SetRotation(uint rotation)
    {
      Rotation = rotation;
      return true;
    }

    public bool SetSubsytemInfo(retro_subsystem_info info)
    {
      SubsystemInfo = info;
      return true;
    }

    public bool SetSupportNoGame()
    {
      SupportsNoGame = true;
      return true;
    }

    public bool SetVariables(IEnumerable<IVariable> variables)
    {
      foreach (IVariable variable in variables)
        _variables[variable.Key] = variable;
      return true;
    }
  }
}
