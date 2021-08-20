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
        private SettingContext settingContext = new SettingContext();

        private ImageSource source;
        public ImageSource Source
        {
            get => source;
            set => SetProperty(ref source, value);
        }

        private string recieveStatus;
        public string RecieveStatus
        {
            get => recieveStatus;
            set => SetProperty(ref recieveStatus, value);
        }

        private string sendStatus;
        public string SendStatus
        {
            get => sendStatus;
            set => SetProperty(ref sendStatus, value);
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

        private ICommand stopReceiveCommand;
        public ICommand StopReceiveCommand
        {
            get => stopReceiveCommand;
            set => SetProperty(ref stopReceiveCommand, value);
        }

        private ICommand stopSendCommand;
        public ICommand StopSendCommand
        {
            get => stopSendCommand;
            set => SetProperty(ref stopSendCommand, value);
        }

        public MainWindowViewModel()
        {
            SendCommand = new Command(SendExecute);
            RecieveCommand = new Command(RecieveExecute);
            MoreCommand = new Command(MoreExecute);
            StopReceiveCommand = new Command(StopReceiveExecute);
        }

        private async void SendExecute()
        {
            ((Command)SendCommand).CanExecuteValue = false;
            SendStatus = "送信：接続要求";

            await OnShowWindow(typeof(SendWindow), executeCall: SendProcess);

            ((Command)SendCommand).CanExecuteValue = true;
            SendStatus = "";
        }

        private void SendProcess(object context)
        {

        }

        private void RecieveExecute()
        {
            OnShowWindow(typeof(RecieveWindow), executeCall: RecieveProcess);
        }

        private async void RecieveProcess(object port)
        {
            ((Command)RecieveCommand).CanExecuteValue = false;
            RecieveStatus = "受信：待機中";

            IPEndPoint iPEndPoint = null;
            await clientContrloler.AcceptAsync((int)port, (ip, data) =>
            {
                bool reqConnect = false;
                OnShowWindow(typeof(ConnectionWindow),
                    paramater: Tuple.Create(ip, data),
                    executeCall: (p) => reqConnect = (bool)p).Wait();

                iPEndPoint = ip;
                return reqConnect;
            });

            RecieveStatus = $"受信：{iPEndPoint?.Address}";
            await clientContrloler.ReceiveAsync((img) => Source = img);

            ((Command)RecieveCommand).CanExecuteValue = true;
            RecieveStatus = "";
        }

        private void MoreExecute()
        {
            OnShowWindow(typeof(MoreWindow), paramater: settingContext, executeCall: MoreProcess);
        }

        private void MoreProcess(object context)
        {
            settingContext = (SettingContext)context;
        }

        private async void StopReceiveExecute()
        {

            var result = await OnShowMessageBox("受信処理を中止しますか？", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                clientContrloler.Dispose();
                ((Command)RecieveCommand).CanExecuteValue = true;
                RecieveStatus = "";
            }
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
