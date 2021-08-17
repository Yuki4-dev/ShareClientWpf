using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ShareClientWpf
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Func<string, MessageBoxButton, MessageBoxResult> ShowMessageBox;
        public event Action<Type, bool, object, Action<object>> ShowWindow;

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

        protected bool SetProperty<T>(ref T prop, T value, Action postCallMethod = null, [CallerMemberName] string name = "")
        {
            if (prop?.Equals(value) ?? value != null)
            {
                prop = value;
                OnPropertyChanged(name);
                postCallMethod?.Invoke();

                return true;
            }

            return false;
        }

        protected void OnShowWindow(Type windowType, bool isModal = true, object paramater = null, Action<object> callBack = null)
            => ShowWindow?.Invoke(windowType, isModal, paramater, callBack);

        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new(name));

        protected MessageBoxResult OnShowMessageBox(string msg) =>
            OnShowMessageBox(msg, MessageBoxButton.OK);

        protected MessageBoxResult OnShowMessageBox(string msg, MessageBoxButton button) =>
            ShowMessageBox?.Invoke(msg, button) ?? MessageBoxResult.None;
    }


    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> callCommand;

        private bool can = true;
        public bool Can 
        {
            get => can;
            set
            {
                if(!can.Equals(value))
                {
                    can = value;
                    CanExecuteChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public Command(Action action)
        {
            callCommand = (p) => action.Invoke();
        }

        public Command(Action<object> action)
        {
            callCommand = action;
        }

        public bool CanExecute(object parameter) => Can;

        public void Execute(object parameter)
        {
            callCommand.Invoke(parameter);
        }
    }

}
