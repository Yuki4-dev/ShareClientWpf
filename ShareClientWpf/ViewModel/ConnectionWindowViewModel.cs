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

        public override void LoadedProcces(object paramater, Action<object> executeCallback)
        {
            if (paramater is Tuple<IPEndPoint, ConnectionData> data)
            {
                SetConnectText(data.Item1, data.Item2);
            }

            callback = executeCallback;
        }

        private void SetConnectText(IPEndPoint iPEndPoint, ConnectionData connectionData)
        {
            ConnectText = iPEndPoint.Address.ToString();
        }

        private void Execute(object paramater)
        {
            callback?.Invoke(paramater.ToString().Equals("1"));
        }
    }
}
