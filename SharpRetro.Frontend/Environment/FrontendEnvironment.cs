using SharpRetro.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Frontend.Environment
{
  public class FrontendEnvironment : IEnvironmentHandler
  {
    public int Rotation { get; protected set; }
    public bool Overscan { get; set; }
    public bool CanDupe { get; set; }
    public int PerformanceLevel { get; protected set; }
    public string SystemDirectory { get; set; }
    public int PixelFormat { get; protected set; }
    public bool SupportNoGame { get; protected set; }
    public string LibretroPath { get; set; }
    public string CoreAssetsDirectory { get; set; }
    public string SaveDirectory { get; set; }
    public retro_game_geometry Geometry { get; protected set; }

    protected IEnvironmentManager _environmentManager;

    public void Attach(IEnvironmentManager environmentManager)
    {
      _environmentManager = environmentManager;
      _environmentManager.AddDelegate((int)RETRO_ENVIRONMENT.SET_ROTATION, SetRotation);
      _environmentManager.AddDelegate((int)RETRO_ENVIRONMENT.GET_OVERSCAN, GetOverscan);
      _environmentManager.AddDelegate((int)RETRO_ENVIRONMENT.GET_CAN_DUPE, GetCanDupe);
      _environmentManager.AddDelegate((int)RETRO_ENVIRONMENT.SET_PERFORMANCE_LEVEL, SetPerformanceLevel);
      _environmentManager.AddDelegate((int)RETRO_ENVIRONMENT.GET_SYSTEM_DIRECTORY, GetSystemDirectory);
      _environmentManager.AddDelegate((int)RETRO_ENVIRONMENT.SET_PIXEL_FORMAT, SetPixelFormat);
      _environmentManager.AddDelegate((int)RETRO_ENVIRONMENT.SET_SUPPORT_NO_GAME, SetSupportNoGame);
      _environmentManager.AddDelegate((int)RETRO_ENVIRONMENT.GET_LIBRETRO_PATH, GetLibretroPath);
      _environmentManager.AddDelegate((int)RETRO_ENVIRONMENT.GET_CORE_ASSETS_DIRECTORY, GetCoreAssetsDirectory);
      _environmentManager.AddDelegate((int)RETRO_ENVIRONMENT.GET_SAVE_DIRECTORY, GetSaveDirectory);
      _environmentManager.AddDelegate((int)RETRO_ENVIRONMENT.SET_GEOMETRY, SetGeometry);
    }

    public void Detach(IEnvironmentManager environmentManager)
    {
      _environmentManager.RemoveDelegate((int)RETRO_ENVIRONMENT.SET_ROTATION, SetRotation);
      _environmentManager.RemoveDelegate((int)RETRO_ENVIRONMENT.GET_OVERSCAN, GetOverscan);
      _environmentManager.RemoveDelegate((int)RETRO_ENVIRONMENT.GET_CAN_DUPE, GetCanDupe);
      _environmentManager.RemoveDelegate((int)RETRO_ENVIRONMENT.SET_PERFORMANCE_LEVEL, SetPerformanceLevel);
      _environmentManager.RemoveDelegate((int)RETRO_ENVIRONMENT.GET_SYSTEM_DIRECTORY, GetSystemDirectory);
      _environmentManager.RemoveDelegate((int)RETRO_ENVIRONMENT.SET_PIXEL_FORMAT, SetPixelFormat);
      _environmentManager.RemoveDelegate((int)RETRO_ENVIRONMENT.SET_SUPPORT_NO_GAME, SetSupportNoGame);
      _environmentManager.RemoveDelegate((int)RETRO_ENVIRONMENT.GET_LIBRETRO_PATH, GetLibretroPath);
      _environmentManager.RemoveDelegate((int)RETRO_ENVIRONMENT.GET_CORE_ASSETS_DIRECTORY, GetCoreAssetsDirectory);
      _environmentManager.RemoveDelegate((int)RETRO_ENVIRONMENT.GET_SAVE_DIRECTORY, GetSaveDirectory);
      _environmentManager.RemoveDelegate((int)RETRO_ENVIRONMENT.SET_GEOMETRY, SetGeometry);
      _environmentManager = null;
    }

    protected bool SetRotation(int cmd, IntPtr data)
    {
      Rotation = Marshal.ReadInt32(data);
      return true;
    }

    protected bool GetOverscan(int cmd, IntPtr data)
    {
      Marshal.WriteIntPtr(data, (IntPtr)(Overscan ? 1 : 0));
      return true;
    }

    protected bool GetCanDupe(int cmd, IntPtr data)
    {
      Marshal.WriteIntPtr(data, (IntPtr)(CanDupe ? 1 : 0));
      return true;
    }

    protected bool SetPerformanceLevel(int cmd, IntPtr data)
    {
      PerformanceLevel = Marshal.ReadInt32(data);
      return true;
    }

    protected bool GetSystemDirectory(int cmd, IntPtr data)
    {
      Marshal.WriteIntPtr(data, _environmentManager.AllocateString(SystemDirectory));
      return true;
    }

    protected bool SetPixelFormat(int cmd, IntPtr data)
    {
      PixelFormat = Marshal.ReadInt32(data);
      return true;
    }

    protected bool SetSupportNoGame(int cmd, IntPtr data)
    {
      SupportNoGame = true;
      return true;
    }

    protected bool GetLibretroPath(int cmd, IntPtr data)
    {
      Marshal.WriteIntPtr(data, _environmentManager.AllocateString(LibretroPath));
      return true;
    }

    protected bool GetCoreAssetsDirectory(int cmd, IntPtr data)
    {
      Marshal.WriteIntPtr(data, _environmentManager.AllocateString(CoreAssetsDirectory));
      return true;
    }

    protected bool GetSaveDirectory(int cmd, IntPtr data)
    {
      Marshal.WriteIntPtr(data, _environmentManager.AllocateString(SaveDirectory));
      return true;
    }

    protected bool SetGeometry(int cmd, IntPtr data)
    {
      Geometry = Marshal.PtrToStructure<retro_game_geometry>(data);
      return true;
    }
  }
}
