using ShareClient.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IClientContrloler clientContrloler = new ShreClientController();

        private ImageSource source;
        public ImageSource Source
        {
            get => source;
            set => SetProperty(ref source, value);
        }

        private string statusText;
        public string StatusText
        {
            get => statusText;
            set => SetProperty(ref statusText, value);
        }

        private ICommand sendCommand;
        public ICommand SendCommand
        {
            get => sendCommand;
            set => SetProperty(ref sendCommand, value);
        }

        private ICommand recieveCommand;
        public ICommand RecieveCommand
        {
            get => recieveCommand;
            set => SetProperty(ref recieveCommand, value);
        }

        private ICommand moreCommand;
        public ICommand MoreCommand
        {
            get => moreCommand;
            set => SetProperty(ref moreCommand, value);
        }

        public MainWindowViewModel()
        {
            SendCommand = new Command(SendExecute);
            RecieveCommand = new Command(RecieveExecute);
            MoreCommand = new Command(MoreExecute);
        }

        private async void SendExecute()
        {
            ((Command)SendCommand).CanExecuteValue = false;
            StatusText = "送信中";

            await OnShowWindow(typeof(SendWindow), executeCall: SendProcess);

            ((Command)SendCommand).CanExecuteValue = true;
            StatusText = "";
        }

        private void SendProcess(object handle)
        {

        }

        private void RecieveExecute()
        {
            OnShowWindow(typeof(RecieveWindow), executeCall: RecieveProcess);
        }

        private async void RecieveProcess(object port)
        {
            ((Command)RecieveCommand).CanExecuteValue = false;
            StatusText = "受信中";

            await clientContrloler.AcceptAsync((int)port, (ip, data) =>
            {
                bool reqConnect = false;
                OnShowWindow(typeof(ConnectionWindow),
                    paramater: Tuple.Create(ip, data),
                    executeCall: (p) => reqConnect = (bool)p).Wait();

                return reqConnect;
            });

            await clientContrloler.ReceiveAsync((img) => Source = img);

            ((Command)RecieveCommand).CanExecuteValue = true;
            StatusText = "";
        }

        private void MoreExecute()
        {
            OnShowWindow(typeof(SendWindow));
        }

        protected override void CloseExecute()
        {
            Closing();
        }

        private async void Closing()
        {
            var result = await OnShowMessageBox("終了しますか？", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                clientContrloler.Dispose();
                OnCloseWindow();
            }
        }
    }
}
