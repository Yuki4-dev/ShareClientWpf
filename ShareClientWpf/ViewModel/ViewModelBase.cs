using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ShareClientWpf
{
    public class ViewModelBase : ModelBase
    {
        public event Func<string, MessageBoxButton, Task<MessageBoxResult>> ShowMessageBox;
        public event Func<Type, bool, object, Action<object>, Task> ShowWindow;
        public event Func<Type, Action<CommonDialog>, Action<CommonDialog>, Task<bool>> ShowCommonDialog;
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

        protected virtual void CloseExecute(object paramater)
        {
            OnCloseWindow();
        }

        public virtual void LoadedProcess(object paramater, Action<object> executeCallback)
        {
            return;
        }

        public virtual bool PostProcces()
        {
            return false;
        }

        protected Task OnShowWindow(Type windowType, bool isModal = true, object paramater = null, Action<object> executeCall = null) =>
            ShowWindow?.Invoke(windowType, isModal, paramater, executeCall) ?? Task.CompletedTask;

        protected Task<bool> OnShowCommonDialog(Type dialogType, Action<CommonDialog> preCallback = null, Action<CommonDialog> succesCallback = null) =>
            ShowCommonDialog?.Invoke(dialogType, preCallback, succesCallback) ?? Task.FromResult(false);

        protected Task<MessageBoxResult> OnShowMessageBox(string msg, MessageBoxButton button = MessageBoxButton.OK) =>
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

        public Command(Action action, Action postCall = null) : this((p) => action.Invoke(), postCall)
        {
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
