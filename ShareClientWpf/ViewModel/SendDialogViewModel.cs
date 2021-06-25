using ShareClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShareClientWpf
{
    public class SendDialogViewModel : ViewModelBase, ISendDialogService
    {
        public Action<IWindowCaputure, Connection> SendExecute { get; set; }
        public ObservableCollection<WindowImageInfo> WindowsImages { get; } = new();
        public object SelectedWindow { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }

        public SendDialogViewModel()
        {
        }

        public override async void DisplayProcces()
        {
            IsBusy = true;
            await SetWIndowInfo();
            IsBusy = false;
        }

        public override bool PostProcces()
        {
            IsBusy = false;
            WindowsImages.Clear();
            return true;
        }

        private async Task SetWIndowInfo()
        {
            WindowsImages.Clear();

            var windowImageInfos = await Task.Run(() =>
            {
                var windowList = new List<WindowImageInfo>();
                foreach (Process p in Process.GetProcesses().Where(p => p.MainWindowTitle.Length != 0))
                {
                    if (ImageHelper.TryGetPrintWindowBmp(p.MainWindowHandle, out var img))
                    {
                        windowList.Add(new WindowImageInfo()
                        {
                            WindowImage = img,
                            Title = p.MainWindowTitle,
                            WindowHandle = p.MainWindowHandle
                        });
                    }
                }

                return windowList;
            });

            if (IsNotBusy)
            {
                return;
            }

            foreach (var wInfo in windowImageInfos)
            {
                WindowsImages.Add(wInfo);
            }
        }
    }
}