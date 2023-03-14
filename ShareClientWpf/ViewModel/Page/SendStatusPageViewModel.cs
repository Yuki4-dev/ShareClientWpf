using CommunityToolkit.Mvvm.ComponentModel;
using System.Net;

namespace ShareClientWpf
{
    [ObservableObject]
    public partial class SendStatusPageViewModel
    {
        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private WindowInfo windowInfo;

        [ObservableProperty]
        private IPEndPoint iPEndPoint;

        [ObservableProperty]
        private string byteSizeText;

        [ObservableProperty]
        private Command stopCommand;

        public void SetSendState(SendViewModelState state, SendContext context = null)
        {
            bool stopExecute = false;
            if (state == SendViewModelState.None)
            {
                Message = "未接続";
                WindowInfo = null;
                IPEndPoint = null;
            }
            else if (state == SendViewModelState.Connect)
            {
                Message = "接続要求中";
                WindowInfo = context.WindowInfo;
                IPEndPoint = context.IPEndPoint;
                stopExecute = true;
            }
            else if (state == SendViewModelState.Sending)
            {
                Message = "送信中";
                WindowInfo = context.WindowInfo;
                IPEndPoint = context.IPEndPoint;
                stopExecute = true;
            }

            if (StopCommand != null)
            {
                StopCommand.CanExecuteValue = stopExecute;
            }
        }
    }

    public enum SendViewModelState
    {
        None, Connect, Sending
    }
}
