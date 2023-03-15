using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        public ReceiveWindowViewModel()
        {
#if DEBUG
            PortText = "2002";
#endif
        }

        public override void Loaded(object parameter, Action<object> executeCallback)
        {
            callback = executeCallback;
        }

        [RelayCommand]
        private void Receive()
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
