using System.Collections.Generic;

namespace SharpRetro.Libretro.Environment
{
  public interface IVariable
  {
    string Key { get; }
    string[] Values { get; }
    string SelectedValue { get; set; }
    string DefaultValue { get; }
  }
}