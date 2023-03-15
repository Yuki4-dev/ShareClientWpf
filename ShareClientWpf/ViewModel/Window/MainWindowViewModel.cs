using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShareClient.Model.Connect;
using System;
using System.Drawing.Imaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ShareClientWpf
{
    [ObservableObject]
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Profile profile = new();
        private SettingContext settingContext = new();
        private readonly IClientController clientController = new ShareClientController();
        private readonly SendStatusPageViewModel sendStatusPageViewModel = new();
        private readonly ReceiveStatusPageViewModel receiveStatusPageViewModel = new();

        [ObservableProperty]
        private ImageSource source;

        [ObservableProperty]
        private object rightPageContent;

        public MainWindowViewModel()
        {
            sendStatusPageViewModel.StopCommand = StopSendCommand;
            sendStatusPageViewModel.SetSendState(SendViewModelState.None);
            receiveStatusPageViewModel.StopCommand = StopReceiveCommand;
            receiveStatusPageViewModel.SetReceiveState(ReceiveViewModelState.None);

            RightPageContent = sendStatusPageViewModel;

            settingContext.SendWidth = 0;
            settingContext.SendDelay = 30;
            settingContext.Format = ImageFormat.Jpeg;

#if DEBUG
            profile.Name = "Test1";
#endif
        }

        public MainWindowViewModel(IClientController clientController) : this()
        {
            this.clientController = clientController;
        }

        [RelayCommand]
        private void Select(object parameter)
        {
            if (parameter.ToString() == "0")
            {
                RightPageContent = sendStatusPageViewModel;
            }
            else
            {
                RightPageContent = receiveStatusPageViewModel;
            }
        }

        [RelayCommand]
        private void Profile()
        {
            OnShowWindow(typeof(ProfileWindow), parameter: profile);
        }

        [RelayCommand]
        private async void Send()
        {
            SendContext context = null;
            await OnShowWindow(typeof(SendWindow), executeCall: (parameter) => context = (SendContext)parameter);

            if (context != null)
            {
                try
                {
                    await SendWindow(context);
                }
                catch (Exception ex)
                {
                    await OnShowMessageBox(ex.Message);
                }
            }

            sendStatusPageViewModel.SetSendState(SendViewModelState.None);
        }

        [RelayCommand]
        private async Task SendWindow(SendContext context)
        {
            sendStatusPageViewModel.SetSendState(SendViewModelState.Connect, context);

            var data = new ConnectionData(new(), Encoding.UTF8.GetBytes(profile.GetJsonString()));
            var isConnected = await clientController.ConnectAsync(context.IPEndPoint, data);
            if (isConnected)
            {
                sendStatusPageViewModel.SetSendState(SendViewModelState.Sending, context);
                await clientController.SendWindowAsync(context, settingContext);
            }
        }

        [RelayCommand]
        private async void StopSend()
        {
            var result = await OnShowMessageBox("送信処理を中止しますか？", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                clientController.CancelConnect();
                clientController.CloseSendWindow();
            }
        }

        [RelayCommand]
        private void Receive()
        {
            OnShowWindow(typeof(RecieveWindow), executeCall: async (parameter) =>
            {
                ReceiveContext context = null;
                try
                {
                    receiveStatusPageViewModel.SetReceiveState(ReceiveViewModelState.Accept);
                    context = await AcceptAsync((int)parameter);
                }
                catch (Exception ex)
                {
                    await OnShowMessageBox(ex.Message);
                }

                if (context != null)
                {
                    try
                    {
                        receiveStatusPageViewModel.SetReceiveState(ReceiveViewModelState.Receiving, context);
                        await clientController.ReceiveWindowAsync((img) => Source = img, () => OnShowMessageBox("切断されました。"));
                    }
                    catch (Exception ex)
                    {
                        await OnShowMessageBox(ex.Message);
                    }
                }

                receiveStatusPageViewModel.SetReceiveState(ReceiveViewModelState.None);
            });
        }

        private async Task<ReceiveContext> AcceptAsync(int port)
        {
            ReceiveContext context = null;
            await clientController.AcceptAsync(port, (ip, data) =>
            {
                var reqConnect = false;
                context = new ReceiveContext()
                {
                    Profile = ShareClientWpf.Profile.FromJson(Encoding.UTF8.GetString(data.MetaData)),
                    IPEndPoint = ip
                };
                OnShowWindow(typeof(ConnectionWindow), true, context, (p) => reqConnect = (bool)p).Wait();

                if (!reqConnect)
                {
                    context = null;
                }
                return new ConnectionResponse(reqConnect, new ConnectionData(data.ClientSpec));
            });

            return context;
        }


        [RelayCommand]
        private async void StopReceive()
        {
            var result = await OnShowMessageBox("受信処理を中止しますか？", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                clientController.CancelAccept();
                clientController.CloseReceiveWindow();
            }
        }

        [RelayCommand]
        private void More()
        {
            OnShowWindow(typeof(MoreWindow), parameter: settingContext, executeCall: (context) => settingContext = (SettingContext)context);
        }

        protected override void Close(object parameter)
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
