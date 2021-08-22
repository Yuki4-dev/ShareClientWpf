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
            set => SetProperty(ref portText,
                                value,
                                IntValidate<string>((_) => Message = "Portには数字を入れてください。"),
                                () => Message = "");
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
#if DEBUG
            PortText = "2002";
#endif
        }

        public override void LoadedProcces(object paramater, Action<object> executeCallback)
        {
            callback = executeCallback;
        }

        public void RecieveExecute()
        {
            callback?.Invoke(int.Parse(PortText));
            OnCloseWindow();
            Message = "";
        }
    }
}
