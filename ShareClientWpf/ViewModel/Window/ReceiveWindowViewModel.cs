using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Windows.Input;

namespace ShareClientWpf
{
    [ObservableObject]
    public partial class ReceiveWindowViewModel : ViewModelBase
    {
        private Action<object> callback;

        [ObservableProperty]
        private string message;

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

        public ICommand ReceiveCommand { get; }

        public ReceiveWindowViewModel()
        {
            ReceiveCommand = new Command(ReceiveExecute);
#if DEBUG
            PortText = "2002";
#endif
        }

        public override void LoadedProcess(object parameter, Action<object> executeCallback)
        {
            callback = executeCallback;
        }

        private void ReceiveExecute()
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
