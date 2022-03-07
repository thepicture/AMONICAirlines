using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AMONICAirlinesDesktopApp.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public bool IsBusy { get; set; }
        public event Action OnRequestClose;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?
                .Invoke(this,
                        new PropertyChangedEventArgs(nameof(propertyName)));
        }
    }
}
