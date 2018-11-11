using System;

namespace SharpRetro.Frontend.Environment
{
  public interface IEnvironmentManager : IDisposable
  {
    void AddDelegate(int cmd, EnvironmentManager.EnvironmentDelegate environmentDelegate);
    bool Invoke(int cmd, IntPtr data);
    bool RemoveDelegate(int cmd, EnvironmentManager.EnvironmentDelegate environmentDelegate);
    bool RemoveDelegates(int cmd);
    IntPtr AllocateString(string value);
    void DeallocateStrings();
  }
}