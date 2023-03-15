using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows.Input;

namespace ShareClientWpf
{

    [ObservableObject]
    public partial class SendWindowViewModel : ViewModelBase
    {
        private Action<object> callback;

        public ICollection<WindowInfo> WindowInfos { get; } = new ObservableCollection<WindowInfo>();

        [ObservableProperty]
        private WindowInfo selectWindowInfo;

        [ObservableProperty]
        private string message;

        private string ipText;
        public string IpText
        {
            get => ipText;
            set
            {
                if (IPAddress.TryParse(value, out var _))
                {
                    Message = "";
                    SetProperty(ref ipText, value);
                }
                else
                {
                    Message = "有効なIPを入力してください。";
                }
            }
        }

        private string portText;
        public string PortText
        {
            get => portText;
            set
            {
                if (int.TryParse(value, out var _))
                {
                    Message = "";
                    SetProperty(ref portText, value);
                }
                else
                {
                    Message = "Portには数字を入れてください。";
                }
            }
        }

        public ICommand SelectedCommand { get; }

        public SendWindowViewModel()
        {
            SelectedCommand = new RelayCommand<WindowInfo>(p => SelectWindowInfo = p);
#if DEBUG
            IpText = "127.0.0.1";
            PortText = "2002";
#endif
        }

        public override void Loaded(object parameter, Action<object> executeCallback)
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
        }

        [RelayCommand]
        private void Send()
        {
            if (string.IsNullOrEmpty(IpText) && string.IsNullOrEmpty(PortText))
            {
                Message = "IpまたはPortを入力してください。";
                return;
            }

            if (SelectWindowInfo == null)
            {
                Message = "Windowを選択してください。";
                return;
            }

            var context = new SendContext()
            {
                WindowInfo = SelectWindowInfo,
                IPEndPoint = new IPEndPoint(IPAddress.Parse(IpText), int.Parse(PortText))
            };

            callback.Invoke(context);
            OnCloseWindow();
        }

    }
}
