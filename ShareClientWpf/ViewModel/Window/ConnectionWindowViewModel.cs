using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace ShareClientWpf
{
    [ObservableObject]
    public partial class ConnectionWindowViewModel : ViewModelBase
    {
        private Action<object> callback;

        [ObservableProperty]
        private Profile profile;

        [ObservableProperty]
        private string connectText;

        public override void Loaded(object parameter, Action<object> executeCallback)
        {
            callback = executeCallback;
            var context = (ReceiveContext)parameter;
            ConnectText = context.IPEndPoint.Address.ToString();
            Profile = context.Profile;
        }

        protected override void Close(object parameter)
        {
            callback.Invoke(parameter.ToString().Equals("1"));
            OnCloseWindow();
        }
    }
}
