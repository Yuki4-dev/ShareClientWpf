using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShareClientWpf
{
    public class RecieveWindowViewModel : ViewModelBase
    {
        private Action<object> callback;

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

        private ICommand recieveCommand;
        public ICommand RecieveCommand
        {
            get => recieveCommand;
            set => SetProperty(ref recieveCommand, value);
        }

        public RecieveWindowViewModel()
        {
            RecieveCommand = new Command(RecieveExecute);
        }

        public override void LoadedProcces(object paramater, Action<object> executeCallback)
        {
            callback = executeCallback;
        }

        public void RecieveExecute()
        {
            if(int.TryParse(PortText, out var port))
            {
                callback?.Invoke(port);
                OnCloseWindow();
            }

            Message = "※数字を入力してください。";
        }
    }
}
