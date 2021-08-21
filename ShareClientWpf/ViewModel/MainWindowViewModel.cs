using ShareClient.Model;
using System;
using System.Drawing.Imaging;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IClientContrloler clientContrloler = new ShreClientController();
        private SettingContext settingContext = new();

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
            StopSendCommand = new Command(StopSendExecute);

#if DEBUG
            settingContext.Name = "test";
            settingContext.SendWidth = 0;
            settingContext.SendDelay = 30;
            settingContext.Format = ImageFormat.Jpeg;
#endif
        }

        private async void SendExecute()
        {
            SendStatusChange(false, "送信：接続要求");

            bool doExecute = false;
            await OnShowWindow(typeof(SendWindow), executeCall: async (paramater) =>
            {
                doExecute = true;

                var sendContext = (SendContext)paramater;
                await clientContrloler.ConnectAsync(sendContext.IPEndPoint, new(new()), (responese) => responese.IsConnect);

                var length = sendContext.WindowInfo.Title.Length;
                SendStatusChange(false, $"送信：画面共有中【{sendContext.WindowInfo.Title.Substring(0, length > 12 ? 12 : length)}】");

                await clientContrloler.SendWindowAsync(sendContext, settingContext);

                SendStatusChange(true);
            });

            if (!doExecute)
            {
                SendStatusChange(true);
            }
        }

        private async void StopSendExecute()
        {
            var result = await OnShowMessageBox("送信処理を中止しますか？", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                clientContrloler.Dispose();
                SendStatusChange(true);
            }
        }

        private void SendStatusChange(bool execute, string message = "")
        {
            ((Command)SendCommand).CanExecuteValue = execute;
            SendStatus = message;
        }

        private void RecieveExecute()
        {
            OnShowWindow(typeof(RecieveWindow), executeCall: async (port) =>
            {
                ReceiveStatusChange(false, "受信：待機中");

                IPEndPoint iPEndPoint = null;
                await clientContrloler.AcceptAsync((int)port, (ip, data) =>
                {
                    bool reqConnect = false;
                    OnShowWindow(typeof(ConnectionWindow),
                        paramater: Tuple.Create(ip, data),
                        executeCall: (p) => reqConnect = (bool)p).Wait();

                    iPEndPoint = ip;
                    return new ConnectionResponse(reqConnect, new(data.CleintSpec));
                });

                if (iPEndPoint != null)
                {
                    ReceiveStatusChange(false, $"受信：{iPEndPoint.Address}");
                    await clientContrloler.ReceiveWindowAsync((img) => Source = img);
                }

                ReceiveStatusChange(true);
            });
        }

        private async void StopReceiveExecute()
        {
            var result = await OnShowMessageBox("受信処理を中止しますか？", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                clientContrloler.Dispose();
                ReceiveStatusChange(true);
            }
        }

        private void ReceiveStatusChange(bool execute, string message = "")
        {
            ((Command)RecieveCommand).CanExecuteValue = execute;
            RecieveStatus = message;
        }

        private void MoreExecute()
        {
            OnShowWindow(typeof(MoreWindow), paramater: settingContext, executeCall: (context) =>
            {
                settingContext = (SettingContext)context;
            });
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
