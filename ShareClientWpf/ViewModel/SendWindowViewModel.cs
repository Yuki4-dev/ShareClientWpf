using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShareClientWpf
{
    public class SendWindowViewModel : ViewModelBase
    {
        private Action<object> callback;

        public ICollection<WindowInfo> WindowImageInfos { get; } = new ObservableCollection<WindowInfo>();

        public WindowInfo SelectWindowInfo { get; set; }

        private string ipText;
        public string IpText
        {
            get => ipText;
            set
            {
                SetProperty(ref ipText,
                            value,
                            ModelBase.Validate<string>((_) => IPAddress.TryParse(value, out var __),
                                (_) => Message = "有効なIPを入力してください。"),
                            () => Message = "");
            }
        }

        private string portText;
        public string PortText
        {
            get => portText;
            set
            {
                SetProperty(ref portText,
                            value,
                             ModelBase.IntValidate<string>((_) => Message = "Portには数字を入れてください。"),
                            () => Message = "");
            }
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
#if DEBUG
            IpText = "127.0.0.1";
            PortText = "2002";
#endif
        }

        public override void LoadedProcces(object paramater, Action<object> executeCallback)
        {
            callback = executeCallback;
            LoadWindows();
        }

        private void LoadWindows()
        {
            WindowImageInfos.Clear();
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowTitle.Length != 0)
                {
                    var w = new WindowInfo()
                    {
                        Title = p.MainWindowTitle,
                        WindowHandle = p.MainWindowHandle
                    };
                    WindowImageInfos.Add(w);
                }
            }
        }

        private void SendExecute()
        {
            if (!string.IsNullOrEmpty(IpText) && !string.IsNullOrEmpty(PortText))
            {
                var context = new SendContext()
                {
                    WindowInfo = SelectWindowInfo,
                    IPEndPoint = new IPEndPoint(IPAddress.Parse(IpText), int.Parse(PortText))
                };

                callback?.Invoke(context);
                OnCloseWindow();
            }
        }

    }
}
