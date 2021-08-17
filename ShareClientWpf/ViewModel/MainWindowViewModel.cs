using System;
using System.Collections.Generic;
using System.Linq;
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

        public MainWindowViewModel()
        {
            sendCommand = new Command(SendExecute);
            recieveCommand = new Command(RecieveExecute);
            moreCommand = new Command(MoreExecute);
        }

        private void SendExecute()
        {
            OnShowWindow(typeof(SendWindow));
        }

        private void RecieveExecute()
        {
            OnShowWindow(typeof(SendWindow));
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
