namespace SharpRetro.Libretro.Environment
{
  public class Variable : IVariable
  {
    protected string _key;
    protected string _description;
    protected string[] _values;
    protected string _selectedValue;

    public Variable(string key, string value)
    {
      _key = key;
      string[] parts = value.Split(';');
      _description = parts[0];
      _values = parts[1].TrimStart(' ').Split('|');
      _selectedValue = _values[0];
    }

    public string Key
    {
      get { return _key; }
    }

    public string[] Values
    {
      get { return _values; }
    }

    public string SelectedValue
    {
      get { return _selectedValue; }
      set { _selectedValue = value; }
    }

    public string DefaultValue
    {
      get { return _values != null && _values.Length > 0 ? _values[0] : null; }
    }
  }
}
