using SharpRetro.Libretro.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Tests.Mock
{
  class MockLibrary : ILibrary
  {
    protected IDictionary<string, Delegate> _delegates;

    public MockLibrary()
    {
      _delegates = new Dictionary<string, Delegate>();
    }

    public IDictionary<string, Delegate> Delegates
    {
      get { return _delegates; }
    }

    public Delegate GetProcDelegate(string procName, Type type)
    {
      if (_delegates.TryGetValue(procName, out Delegate dlgt))
        return dlgt;
      return null;
    }

    public void Dispose()
    {

    }
  }
}
