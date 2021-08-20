using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShareClientWpf
{
    public class SendWindowViewModel : ViewModelBase
    {
        private Action<object> callback;

        private string ipText;
        public string IptText
        {
            get => ipText;
            set => SetProperty(ref ipText, value);
        }

        private string portText;
        public string PortText
        {
            get => portText;
            set => SetProperty(ref portText, value);
        }

        private string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        private ICommand sendCommand;
        public ICommand SendCommand
        {
            get => sendCommand;
            set => SetProperty(ref sendCommand, value);
        }


        public SendWindowViewModel()
        {
            SendCommand = new Command(SendExecute);
        }

        public override void LoadedProcces(object paramater, Action<object> executeCallback)
        {
            callback = executeCallback;
        }

        public void SendExecute()
        {
            if (int.TryParse(PortText, out var port))
            {
                var context = new SendContext()
                {
                    Ip = IptText,
                    Port = port
                };

                callback?.Invoke(context);
                OnCloseWindow();
            }
            else
            {
                Message = "※数字を入力してください。";
            }
        }

    }
}
