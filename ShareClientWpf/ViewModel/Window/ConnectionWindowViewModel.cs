using System;

namespace ShareClientWpf
{
    public class ConnectionWindowViewModel : ViewModelBase
    {
        private Action<object> callback;

        private Profile profile;
        public Profile Profile
        {
            get => profile;
            set => SetProperty(ref profile, value);
        }

        private string connectText;
        public string ConnectText
        {
            get => connectText;
            set => SetProperty(ref connectText, value);
        }

        public override void LoadedProcess(object paramater, Action<object> executeCallback)
        {
            callback = executeCallback;
            var context = (ReceiveContext)paramater;
            ConnectText = context.IPEndPoint.Address.ToString();
            Profile = context.Profile;
        }

        protected override void CloseExecute(object paramater)
        {
            callback.Invoke(paramater.ToString().Equals("1"));
            base.CloseExecute(paramater);
        }
    }
}
