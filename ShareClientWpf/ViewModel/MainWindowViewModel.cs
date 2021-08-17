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
        private ImageSource source;
        public ImageSource Source
        {
            get => source;
            set => SetProperty(ref source, value);
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

        private string statusText;
        public string StatusText
        {
            get => statusText;
            set => SetProperty(ref statusText, value);
        }


        public MainWindowViewModel()
        {
            sendCommand = new Command(SendExecute);
            recieveCommand = new Command(RecieveExecute);
            moreCommand = new Command(MoreExecute);
        }

        private async void SendExecute()
        {
            ((Command)SendCommand).Can = false;

            await OnShowWindow(typeof(SendWindow));
            StatusText = "送信";
        }

        private void RecieveExecute()
        {

            OnShowWindow(typeof(RecieveWindow), executeCall: (port) => RecieveProcess((string)port));
        }

        private async void RecieveProcess(string port)
        {
            ((Command)RecieveCommand).Can = false;
            StatusText = "受信中";

            using var con = new ConnectionManager();
            var connection = await con.AcceptAsync(new IPEndPoint(IPAddress.Any, int.Parse(port)), (ip, data) =>
            {
                var t = OnShowWindow(typeof(ConnectionWindow));
                t.Wait();
                return true;
            });

        }

        private void MoreExecute()
        {
            OnShowWindow(typeof(SendWindow));
        }

        public override bool PostProcces()
        {
            return OnShowMessageBox("終了しますか？", MessageBoxButton.YesNo) != MessageBoxResult.Yes;
        }
    }
}
