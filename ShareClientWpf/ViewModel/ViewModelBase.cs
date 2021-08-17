using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ShareClientWpf
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<ShowMessageBoxEventArgs> ShowMessageBox;

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value, () => OnPropertyChanged(nameof(IsNotBusy)));
        }

        public bool IsNotBusy
        {
            get => !IsBusy;
        }

        public virtual bool PostProcces()
        {
            return false;
        }

        protected void SetProperty<T>(ref T prop, T value, Action postCallMethod = null, [CallerMemberName] string name = "")
        {
            if (prop?.Equals(value) ?? value != null)
            {
                prop = value;
                OnPropertyChanged(name);
                postCallMethod?.Invoke();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new(name));
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
