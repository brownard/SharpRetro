using System;

namespace SharpRetro.Libretro.Cores
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public sealed class LibretroAttribute : Attribute
  {
    private string _entryPoint;

    public LibretroAttribute(string entryPoint)
    {
      _entryPoint = entryPoint;
    }

    public string EntryPoint
    {
      get { return _entryPoint; }
      set { _entryPoint = value; }
    }
  }
}
