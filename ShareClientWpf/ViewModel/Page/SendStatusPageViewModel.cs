using CommunityToolkit.Mvvm.ComponentModel;
using System.Net;
using System.Windows.Input;

namespace ShareClientWpf
{
    [ObservableObject]
    public partial class SendStatusPageViewModel
    {
        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private bool stopEnable;

        [ObservableProperty]
        private WindowInfo windowInfo;

        [ObservableProperty]
        private IPEndPoint iPEndPoint;

        [ObservableProperty]
        private string byteSizeText;

        [ObservableProperty]
        private ICommand stopCommand;

        public void SetSendState(SendViewModelState state, SendContext context = null)
        {
            if (state == SendViewModelState.None)
            {
                Message = "未接続";
                WindowInfo = null;
                IPEndPoint = null;
                stopEnable = false;
            }
            else if (state == SendViewModelState.Connect)
            {
                Message = "接続要求中";
                WindowInfo = context.WindowInfo;
                IPEndPoint = context.IPEndPoint;
                stopEnable = true;
            }
            else if (state == SendViewModelState.Sending)
            {
                Message = "送信中";
                WindowInfo = context.WindowInfo;
                IPEndPoint = context.IPEndPoint;
                stopEnable = true;
            }
        }
    }

    public enum SendViewModelState
    {
        None, Connect, Sending
    }
}
