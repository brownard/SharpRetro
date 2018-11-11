using NUnit.Framework;
using SharpRetro.Frontend.Environment;
using SharpRetro.Native;
using System;
using System.Runtime.InteropServices;

namespace SharpRetro.Tests
{
  public class Environment
  {
    EnvironmentManager _manager;
    FrontendEnvironment _environment;

    [SetUp]
    public void Setup()
    {
      _manager = new EnvironmentManager();
      _environment = new FrontendEnvironment();
      _environment.Attach(_manager);
    }

    [TearDown]
    public void Teardown()
    {
      _manager.Dispose();
    }

    [Test]
    public void SetRotation()
    {
      int value = 90;
      IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
      try
      {
        Marshal.WriteInt32(ptr, value);
        _manager.Invoke((int)RETRO_ENVIRONMENT.SET_ROTATION, ptr);
      }
      finally
      {
        Marshal.FreeHGlobal(ptr);
      }
      Assert.AreEqual(value, _environment.Rotation);
    }

    [Test]
    public void SetPerformanceLevel()
    {
      int value = 15;
      IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
      try
      {
        Marshal.WriteInt32(ptr, value);
        _manager.Invoke((int)RETRO_ENVIRONMENT.SET_PERFORMANCE_LEVEL, ptr);
      }
      finally
      {
        Marshal.FreeHGlobal(ptr);
      }
      Assert.AreEqual(value, _environment.PerformanceLevel);
    }

    [Test]
    public void SetPixelFormat()
    {
      int value = (int)RETRO_PIXEL_FORMAT.XRGB8888;
      IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
      try
      {
        Marshal.WriteInt32(ptr, value);
        _manager.Invoke((int)RETRO_ENVIRONMENT.SET_PIXEL_FORMAT, ptr);
      }
      finally
      {
        Marshal.FreeHGlobal(ptr);
      }
      Assert.AreEqual(value, _environment.PixelFormat);
    }

    [Test]
    public void SetSupportNoGame()
    {
      IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
      try
      {
        _manager.Invoke((int)RETRO_ENVIRONMENT.SET_SUPPORT_NO_GAME, ptr);
      }
      finally
      {
        Marshal.FreeHGlobal(ptr);
      }
      Assert.AreEqual(true, _environment.SupportNoGame);
    }

    [Test]
    public void SetGeometry()
    {
      retro_game_geometry value = new retro_game_geometry { base_height = 100 };
      IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<retro_game_geometry>());
      try
      {
        Marshal.StructureToPtr(value, ptr, false);
        _manager.Invoke((int)RETRO_ENVIRONMENT.SET_GEOMETRY, ptr);
      }
      finally
      {
        Marshal.FreeHGlobal(ptr);
      }
      Assert.AreEqual(value.base_height, _environment.Geometry.base_height);
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void GetOverscan(bool value)
    {
      _environment.Overscan = value;
      int result;
      IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
      try
      {
        _manager.Invoke((int)RETRO_ENVIRONMENT.GET_OVERSCAN, ptr);
        result = Marshal.ReadInt32(ptr);
      }
      finally
      {
        Marshal.FreeHGlobal(ptr);
      }
      Assert.AreEqual(_environment.Overscan, result == 1);
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void GetCanDupe(bool value)
    {
      _environment.CanDupe = value;
      int result;
      IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
      try
      {
        _manager.Invoke((int)RETRO_ENVIRONMENT.GET_CAN_DUPE, ptr);
        result = Marshal.ReadInt32(ptr);
      }
      finally
      {
        Marshal.FreeHGlobal(ptr);
      }
      Assert.AreEqual(_environment.CanDupe, result == 1);
    }

    [Test]
    public void GetSystemDirectory()
    {
      _environment.SystemDirectory = @"c:\test\system";
      string result;
      IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
      try
      {
        _manager.Invoke((int)RETRO_ENVIRONMENT.GET_SYSTEM_DIRECTORY, ptr);
        IntPtr resultPtr = Marshal.ReadIntPtr(ptr);
        result = Marshal.PtrToStringAnsi(resultPtr);
      }
      finally
      {
        Marshal.FreeHGlobal(ptr);
      }
      Assert.AreEqual(_environment.SystemDirectory, result);
    }

    [Test]
    public void GetLibretroPath()
    {
      _environment.LibretroPath = @"c:\test\libretro";
      string result;
      IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
      try
      {
        _manager.Invoke((int)RETRO_ENVIRONMENT.GET_LIBRETRO_PATH, ptr);
        IntPtr resultPtr = Marshal.ReadIntPtr(ptr);
        result = Marshal.PtrToStringAnsi(resultPtr);
      }
      finally
      {
        Marshal.FreeHGlobal(ptr);
      }
      Assert.AreEqual(_environment.LibretroPath, result);
    }

    [Test]
    public void GetCoreAssetsDirectory()
    {
      _environment.CoreAssetsDirectory = @"c:\test\coreassets";
      string result;
      IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
      try
      {
        _manager.Invoke((int)RETRO_ENVIRONMENT.GET_CORE_ASSETS_DIRECTORY, ptr);
        IntPtr resultPtr = Marshal.ReadIntPtr(ptr);
        result = Marshal.PtrToStringAnsi(resultPtr);
      }
      finally
      {
        Marshal.FreeHGlobal(ptr);
      }
      Assert.AreEqual(_environment.CoreAssetsDirectory, result);
    }

    [Test]
    public void GetSaveDirectory()
    {
      _environment.SaveDirectory = @"c:\test\save";
      string result;
      IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
      try
      {
        _manager.Invoke((int)RETRO_ENVIRONMENT.GET_SAVE_DIRECTORY, ptr);
        IntPtr resultPtr = Marshal.ReadIntPtr(ptr);
        result = Marshal.PtrToStringAnsi(resultPtr);
      }
      finally
      {
        Marshal.FreeHGlobal(ptr);
      }
      Assert.AreEqual(_environment.SaveDirectory, result);
    }
  }
}
