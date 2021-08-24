using ShareClient.Model;
using System;
using System.Drawing.Imaging;
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
        private readonly Profile profile = new();
        private readonly IClientController clientController = new ShreClientController();
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

        private ICommand profileCommand;
        public ICommand ProfileCommand
        {
            get => profileCommand;
            set => SetProperty(ref profileCommand, value);
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
            ProfileCommand = new Command(ProfileExecute);
            SendCommand = new Command(SendExecute);
            RecieveCommand = new Command(RecieveExecute);
            MoreCommand = new Command(MoreExecute);
            StopReceiveCommand = new Command(StopReceiveExecute);
            StopSendCommand = new Command(StopSendExecute);

            settingContext.SendWidth = 0;
            settingContext.SendDelay = 30;
            settingContext.Format = ImageFormat.Jpeg;

#if DEBUG
            profile.Name = "Test1";
#endif
        }

        public MainWindowViewModel(IClientController controller, SettingContext context, Profile profile) : this()
        {
            clientController = controller;
            settingContext = context;
            this.profile = profile;
        }

        private void ProfileExecute() => OnShowWindow(typeof(ProfileWindow), paramater: profile);

        private async void SendExecute()
        {
            SendStatusChange(false, "送信：接続要求");
            SendContext context = null;
            await OnShowWindow(typeof(SendWindow), executeCall: (paramater) => context = (SendContext)paramater);

            if (context != null)
            {
                SendStatusChange(false, $"送信：{context.IPEndPoint.Address}");
                try
                {
                    await SendWindow(context);
                }
                catch (Exception ex)
                {
                    await OnShowMessageBox(ex.Message);
                }

            }
            SendStatusChange(true);
        }

        private async Task SendWindow(SendContext context)
        {
            var data = new ConnectionData(new(), Encoding.UTF8.GetBytes(profile.GetJsonString()));
            var isConnected = await clientController.ConnectAsync(context.IPEndPoint, data, (responese) => responese.IsConnect);

            if (isConnected)
            {
                await clientController.SendWindowAsync(context, settingContext);
            }
        }

        private async void StopSendExecute()
        {
            var result = await OnShowMessageBox("送信処理を中止しますか？", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                clientController.CancelConnect();
                clientController.CloseSendWindow();
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
            ReceiveStatusChange(false, "受信：待機中");
            OnShowWindow(typeof(RecieveWindow), executeCall: async (paramater) =>
            {
                var iPEndPoint = await AcceptAsync((int)paramater); ;
                if (iPEndPoint != null)
                {
                    ReceiveStatusChange(false, $"受信：{iPEndPoint.Address}");
                    await clientController.ReceiveWindowAsync((img) => Source = img);
                }

                ReceiveStatusChange(true);
            });
        }

        private async Task<IPEndPoint> AcceptAsync(int port)
        {
            IPEndPoint iPEndPoint = null;
            await clientController.AcceptAsync(port, (ip, data) =>
            {
                bool reqConnect = false;
                OnShowWindow(typeof(ConnectionWindow), true, Tuple.Create(ip, data), (p) => reqConnect = (bool)p).Wait();

                if (reqConnect)
                {
                    iPEndPoint = ip;
                }
                return new ConnectionResponse(reqConnect, new(data.CleintSpec));
            });

            return iPEndPoint;
        }

        private async void StopReceiveExecute()
        {
            var result = await OnShowMessageBox("受信処理を中止しますか？", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                clientController.CancelAccept();
                clientController.CloseReceiveWindow();
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

        protected override void CloseExecute(object paramater)
        {
            Closing();
        }

        private async void Closing()
        {
            var result = await OnShowMessageBox("終了しますか？", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                clientController.Dispose();
                OnCloseWindow();
            }
        }
    }
}
