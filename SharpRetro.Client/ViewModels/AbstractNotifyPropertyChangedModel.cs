using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SharpRetro.Client.ViewModels
{
  abstract class AbstractNotifyPropertyChangedModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
