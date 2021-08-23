using ShareClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        private ICommand executeCommand;
        public ICommand ExecuteCommand
        {
            get => executeCommand;
            set => SetProperty(ref executeCommand, value);
        }

        public ConnectionWindowViewModel()
        {
            ExecuteCommand = new Command(Execute, () => OnCloseWindow());
        }

        public override void LoadedProcess(object paramater, Action<object> executeCallback)
        {
            callback = executeCallback;
            var data = (Tuple<IPEndPoint, ConnectionData>)paramater;
            SetConnectionData(data.Item1, data.Item2);
        }

        private void SetConnectionData(IPEndPoint iPEndPoint, ConnectionData connectionData)
        {
            ConnectText = iPEndPoint.Address.ToString();
            Profile = Profile.FromJson(Encoding.UTF8.GetString(connectionData.MetaData));
        }

        private void Execute(object paramater)
        {
            callback.Invoke(paramater.ToString().Equals("1"));
        }
    }
}
