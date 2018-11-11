using SharpRetro.Native;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpRetro.Frontend.Environment
{
  public class EnvironmentManager : IEnvironmentManager
  {
    public delegate bool EnvironmentDelegate(int cmd, IntPtr data);

    protected IDictionary<int, IList<EnvironmentDelegate>> _delegates = new Dictionary<int, IList<EnvironmentDelegate>>();

    private IDictionary<string, SafeStringHandle> _allocated = new Dictionary<string, SafeStringHandle>();

    public void AddDelegate(int cmd, EnvironmentDelegate environmentDelegate)
    {
      if (!_delegates.TryGetValue(cmd, out IList<EnvironmentDelegate> delegateList))
        delegateList = _delegates[cmd] = new List<EnvironmentDelegate>();
      delegateList.Add(environmentDelegate);
    }

    public bool RemoveDelegate(int cmd, EnvironmentDelegate environmentDelegate)
    {
      return _delegates.TryGetValue(cmd, out IList<EnvironmentDelegate> delegateList) && delegateList.Remove(environmentDelegate);
    }

    public bool RemoveDelegates(int cmd)
    {
      return _delegates.Remove(cmd);
    }

    public bool Invoke(int cmd, IntPtr data)
    {
      if (!_delegates.TryGetValue(cmd, out IList<EnvironmentDelegate> delegateList))
        return false;
      bool result = false;
      foreach (EnvironmentDelegate environmentDelegate in delegateList)
        result |= environmentDelegate(cmd, data);
      return result;
    }

    public IntPtr AllocateString(string value)
    {
      SafeStringHandle handle;
      if (!_allocated.TryGetValue(value, out handle))
        handle = _allocated[value] = new SafeStringHandle(value);
      return handle.DangerousGetHandle();
    }

    public void DeallocateStrings()
    {
      foreach (var handle in _allocated.Values)
        if (!handle.IsClosed)
          handle.Close();
      _allocated.Clear();
    }

    public void Dispose()
    {
      if (_delegates == null)
        return;
      _delegates = null;
      DeallocateStrings();
    }
  }
}
