using System;

namespace SharpRetro.Attributes
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public sealed class EntryPointAttribute : Attribute
  {
    private string _entryPoint;

    public EntryPointAttribute(string entryPoint)
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
