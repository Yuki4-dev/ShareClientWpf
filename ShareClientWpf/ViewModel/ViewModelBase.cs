using System;
using System.ComponentModel;
using System.Windows;

namespace ShareClientWpf
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<ShowMessageBoxEventArgs> ShowMessageBox;

        private bool _IsBusy = false;
        public bool IsBusy
        {
            get => _IsBusy;
            set => OnPropertyChanged(ref _IsBusy, value, nameof(IsBusy), () => OnPropertyChanged(nameof(IsNotBusy)));
        }

        public bool IsNotBusy
        {
            get => !_IsBusy;
        }

        public virtual void DisplayProcces()
        {
        }

        public virtual bool PostProcces()
        {
            return false;
        }

        protected void OnPropertyChanged<T>(ref T prop, T value, string name, Action postCallMethod = null)
        {
            if ((prop == null && value != null) || (prop != null && !prop.Equals(value)))
            {
                prop = value;
                OnPropertyChanged(name, postCallMethod);
            }
        }

        protected void OnPropertyChanged(string name, Action postCallMethod = null)
        {
            PropertyChanged?.Invoke(this, new(name));
            postCallMethod?.Invoke();
        }

        protected void OnShowMessageBox(string msg)
        {
            ShowMessageBox?.Invoke(this, new(msg));
        }

        protected void OnShowMessageBox(ShowMessageBoxEventArgs args)
        {
            ShowMessageBox?.Invoke(this, args);
        }
    }

    public class ShowMessageBoxEventArgs : EventArgs
    {
        public MessageBoxButton Button { get; set; }
        public MessageBoxResult Result { get; set; }
        public string Message { get; }

        public ShowMessageBoxEventArgs(string msg)
        {
            Message = msg;
        }
    }
}
