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
        public event Func<string, MessageBoxButton, Task<MessageBoxResult>> ShowMessageBox;
        public event Func<Type, bool, object, Action<object>, Task> ShowWindow;
        public event Func<bool> CloseWindow;

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

        private ICommand closeCommand;
        public ICommand CloseCommand
        {
            get => closeCommand;
            set => SetProperty(ref closeCommand, value);
        }

        public ViewModelBase()
        {
            CloseCommand = new Command(CloseExecute);
        }

        protected virtual void CloseExecute()
        {
            OnCloseWindow();
        }

        public virtual void LoadedProcces(object paramater, Action<object> executeCallback)
        {
            return;
        }

        public virtual bool PostProcces()
        {
            return false;
        }

        protected bool SetProperty<T>(ref T prop, T value, Action postCallMethod = null, [CallerMemberName] string name = "")
        {
            if (!prop?.Equals(value) ?? value != null)
            {
                prop = value;
                OnPropertyChanged(name);
                postCallMethod?.Invoke();

                return true;
            }

            return false;
        }

        protected Task OnShowWindow(Type windowType, bool isModal = true, object paramater = null, Action<object> executeCall = null)
            => ShowWindow?.Invoke(windowType, isModal, paramater, executeCall) ?? Task.CompletedTask;

        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new(name));

        protected Task<MessageBoxResult> OnShowMessageBox(string msg) =>
            OnShowMessageBox(msg, MessageBoxButton.OK);

        protected Task<MessageBoxResult> OnShowMessageBox(string msg, MessageBoxButton button) =>
            ShowMessageBox?.Invoke(msg, button) ?? Task.FromResult(MessageBoxResult.None);

        protected bool OnCloseWindow() => CloseWindow?.Invoke() ?? false;
    }


    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> callCommand;
        private Action postCallMethod;

        private bool canExecuteValue = true;
        public bool CanExecuteValue
        {
            get => canExecuteValue;
            set
            {
                if (!canExecuteValue.Equals(value))
                {
                    canExecuteValue = value;
                    CanExecuteChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public Command(Action action, Action postCall = null)
        {
            callCommand = (p) => action.Invoke();
            postCallMethod = postCall;
        }

        public Command(Action<object> action, Action postCall = null)
        {
            callCommand = action;
            postCallMethod = postCall;
        }

        public bool CanExecute(object parameter) => CanExecuteValue;

        public void Execute(object parameter)
        {
            callCommand.Invoke(parameter);
            postCallMethod?.Invoke();
        }
    }

}
