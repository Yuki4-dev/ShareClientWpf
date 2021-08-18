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

        private ICommand okCommand;
        public ICommand OkCommand
        {
            get => okCommand;
            set => SetProperty(ref okCommand, value);
        }

        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get => cancelCommand;
            set => SetProperty(ref cancelCommand, value);
        }

        public ConnectionWindowViewModel()
        {
            OkCommand = new Command(OkExecute, () => OnCloseWindow());
            CancelCommand = new Command(CancelExecute, () => OnCloseWindow());
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

        public void OkExecute()
        {
            callback?.Invoke(true);
        }

        public void CancelExecute()
        {
            callback?.Invoke(false);
        }
    }
}
