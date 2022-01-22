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
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly Profile profile = new Profile();
        private SettingContext settingContext = new SettingContext();
        private readonly IClientController clientController = new ShareClientController();
        private readonly SendStatusPageViewModel sendStatusPageViewModel = new SendStatusPageViewModel();
        private readonly RecieveStatusPageViewModel recieveStatusPageViewModel = new RecieveStatusPageViewModel();

        private ImageSource source;
        public ImageSource Source
        {
            get => source;
            set => SetProperty(ref source, value);
        }

        private ModelBase rightPageContent;
        public ModelBase RightPageContent
        {
            get => rightPageContent;
            set => SetProperty(ref rightPageContent, value);
        }

        public ICommand SelectedCommand { get; }

        public HeaderMenuCommands HeaderCommands { get; }

        public MainWindowViewModel()
        {
            SelectedCommand = new Command(SelectExecute);
            HeaderCommands = new HeaderMenuCommands()
            {
                ProfileCommand = new Command(ProfileExecute),
                SendCommand = new Command(SendExecute),
                RecieveCommand = new Command(RecieveExecute),
                MoreCommand = new Command(MoreExecute)
            };

            sendStatusPageViewModel.StopCommand = new Command(StopSendExecute);
            sendStatusPageViewModel.SetSendState(SendViewModelState.None);
            recieveStatusPageViewModel.StopCommand = new Command(StopReceiveExecute);
            recieveStatusPageViewModel.SetReceiveState(ReceiveViewModelState.None);

            RightPageContent = sendStatusPageViewModel;

            settingContext.SendWidth = 0;
            settingContext.SendDelay = 30;
            settingContext.Format = ImageFormat.Jpeg;

#if DEBUG
            profile.Name = "Test1";
#endif
        }

        public MainWindowViewModel(IClientController clientController,
                                   SendStatusPageViewModel sendStatusPageViewModel,
                                   RecieveStatusPageViewModel receiveStatusPageViewModel) : this()
        {
            this.clientController = clientController;
            this.sendStatusPageViewModel = sendStatusPageViewModel;
            this.recieveStatusPageViewModel = receiveStatusPageViewModel;
        }

        private void SelectExecute(object parameter)
        {
            if (parameter.ToString() == "0")
            {
                RightPageContent = sendStatusPageViewModel;
            }
            else
            {
                RightPageContent = recieveStatusPageViewModel;
            }
        }

        private void ProfileExecute()
        {
            OnShowWindow(typeof(ProfileWindow), paramater: profile);
        }

        private async void SendExecute()
        {
            HeaderCommands.SendCommand.CanExecuteValue = false;

            SendContext context = null;
            await OnShowWindow(typeof(SendWindow), executeCall: (paramater) => context = (SendContext)paramater);

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
            HeaderCommands.SendCommand.CanExecuteValue = true;
        }

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

        private async void StopSendExecute()
        {
            var result = await OnShowMessageBox("送信処理を中止しますか？", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                clientController.CancelConnect();
                clientController.CloseSendWindow();
            }
        }

        private void RecieveExecute()
        {
            OnShowWindow(typeof(RecieveWindow), executeCall: async (paramater) =>
            {
                HeaderCommands.RecieveCommand.CanExecuteValue = false;

                ReceiveContext context = null;
                try
                {
                    recieveStatusPageViewModel.SetReceiveState(ReceiveViewModelState.Accept);
                    context = await AcceptAsync((int)paramater);
                }
                catch (Exception ex)
                {
                    await OnShowMessageBox(ex.Message);
                }

                if (context != null)
                {
                    try
                    {
                        recieveStatusPageViewModel.SetReceiveState(ReceiveViewModelState.Receiving, context);
                        await clientController.ReceiveWindowAsync((img) => Source = img, () => OnShowMessageBox("切断されました。"));
                    }
                    catch (Exception ex)
                    {
                        await OnShowMessageBox(ex.Message);
                    }
                }

                recieveStatusPageViewModel.SetReceiveState(ReceiveViewModelState.None);
                HeaderCommands.RecieveCommand.CanExecuteValue = true;
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
                    Profile = Profile.FromJson(Encoding.UTF8.GetString(data.MetaData)),
                    IPEndPoint = ip
                };
                OnShowWindow(typeof(ConnectionWindow), true, context, (p) => reqConnect = (bool)p).Wait();

                if (!reqConnect)
                {
                    context = null;
                }
                return new ConnectionResponse(reqConnect, new ConnectionData(data.CleintSpec));
            });

            return context;
        }

        private async void StopReceiveExecute()
        {
            var result = await OnShowMessageBox("受信処理を中止しますか？", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                clientController.CancelAccept();
                clientController.CloseReceiveWindow();
            }
        }

        private void MoreExecute()
        {
            OnShowWindow(typeof(MoreWindow), paramater: settingContext, executeCall: (context) => settingContext = (SettingContext)context);
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
