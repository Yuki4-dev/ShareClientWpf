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

        public ICollection<WindowInfo> WindowInfos { get; } = new ObservableCollection<WindowInfo>();

        private WindowInfo selectWindowInfo;
        public WindowInfo SelectWindowInfo
        {
            get => selectWindowInfo;
            set
            {
                if (selectWindowInfo != null)
                {
                    selectWindowInfo.IsSelected = false;
                }
                if (value != null)
                {
                    value.IsSelected = true;
                }

                SetProperty(ref selectWindowInfo, value);
            }
        }

        private string ipText;
        public string IpText
        {
            get => ipText;
            set => SetProperty(ref ipText,
                                value,
                                Validate<string>((_) => IPAddress.TryParse(value, out var __),
                                    (_) => Message = "有効なIPを入力してください。"),
                                () => Message = "");
        }

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

        private ICommand selectedCommand;
        public ICommand SelectedCommand
        {
            get => selectedCommand;
            set => SetProperty(ref selectedCommand, value);
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
            SelectedCommand = new Command(p => SelectWindowInfo = (WindowInfo)p);
#if DEBUG
            IpText = "127.0.0.1";
            PortText = "2002";
#endif
        }

        public override void LoadedProcess(object paramater, Action<object> executeCallback)
        {
            callback = executeCallback;
            LoadWindows();
        }

        private void LoadWindows()
        {
            WindowInfos.Clear();
            Process.GetProcesses()
                   .Where(p => p.MainWindowTitle.Length != 0)
                   .Select(p => new WindowInfo() { Title = p.MainWindowTitle, WindowHandle = p.MainWindowHandle })
                   .ToList()
                   .ForEach(info => WindowInfos.Add(info));

            if (WindowInfos.Count > 0)
            {
                SelectWindowInfo = WindowInfos.First();
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
