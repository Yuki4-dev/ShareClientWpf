using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ShareClientWpf
{
    public partial class ViewModelBase
    {
        public event Func<string, MessageBoxButton, Task<MessageBoxResult>> ShowMessageBox;
        public event Func<Type, bool, object, Action<object>, Task> ShowWindow;
        public event Func<Type, Action<CommonDialog>, Action<CommonDialog>, Task<bool>> ShowCommonDialog;
        public event Func<bool> CloseWindow;

        [RelayCommand]
        protected virtual void Close(object parameter)
        {
            OnCloseWindow();
        }

        public virtual void Loaded(object parameter, Action<object> executeCallback)
        {
            return;
        }

        public virtual bool PostProcess()
        {
            return false;
        }

        protected Task OnShowWindow(Type windowType, bool isModal = true, object parameter = null, Action<object> executeCall = null)
        {
            return ShowWindow?.Invoke(windowType, isModal, parameter, executeCall) ?? Task.CompletedTask;
        }

        protected Task<bool> OnShowCommonDialog(Type dialogType, Action<CommonDialog> preCallback = null, Action<CommonDialog> successCallback = null)
        {
            return ShowCommonDialog?.Invoke(dialogType, preCallback, successCallback) ?? Task.FromResult(false);
        }

        protected Task<MessageBoxResult> OnShowMessageBox(string msg, MessageBoxButton button = MessageBoxButton.OK)
        {
            return ShowMessageBox?.Invoke(msg, button) ?? Task.FromResult(MessageBoxResult.None);
        }

        protected bool OnCloseWindow()
        {
            return CloseWindow?.Invoke() ?? false;
        }
    }

}
