using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Client.ViewModels
{
  class ConfigurationViewModel : AbstractNotifyPropertyChangedModel
  {
    private string _corePath;
    private string _gamePath;

    public string CorePath
    {
      get { return _corePath; }
      set
      {
        _corePath = value;
        NotifyPropertyChanged();
      }
    }

    public string GamePath
    {
      get { return _gamePath; }
      set
      {
        _gamePath = value;
        NotifyPropertyChanged();
      }
    }
  }
}
