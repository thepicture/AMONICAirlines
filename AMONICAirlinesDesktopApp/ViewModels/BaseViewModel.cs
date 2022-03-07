using AMONICAirlinesDesktopApp.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AMONICAirlinesDesktopApp.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public bool IsBusy { get; set; }
        public IFeedbackService FeedbackService =>
            DependencyService.Get<IFeedbackService>();
        public event Action OnRequestClose;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?
                .Invoke(this,
                        new PropertyChangedEventArgs(nameof(propertyName)));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
    }
}
