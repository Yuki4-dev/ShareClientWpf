using System;
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

        public ICommand ReceiveCommand { get; }

        public RecieveWindowViewModel()
        {
            ReceiveCommand = new Command(RecieveExecute);
#if DEBUG
            PortText = "2002";
#endif
        }

        public override void LoadedProcess(object paramater, Action<object> executeCallback)
        {
            callback = executeCallback;
        }

        public void RecieveExecute()
        {
            if (!string.IsNullOrEmpty(PortText))
            {
                callback.Invoke(int.Parse(PortText));
                OnCloseWindow();
                Message = "";
            }
            else
            {
                Message = "Portを入力してください。";
            }
        }
    }
}
