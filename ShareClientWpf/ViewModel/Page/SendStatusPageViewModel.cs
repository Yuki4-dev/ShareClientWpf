using System.Net;

namespace ShareClientWpf
{
    public class SendStatusPageViewModel : ModelBase
    {
        private string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        private WindowInfo windowInfo;
        public WindowInfo WindowInfo
        {
            get => windowInfo;
            set => SetProperty(ref windowInfo, value);
        }

        private IPEndPoint iPEndPoint;
        public IPEndPoint IPEndPoint
        {
            get => iPEndPoint;
            set => SetProperty(ref iPEndPoint, value);
        }

        private string byteSizeText;
        public string ByteSizeText
        {
            get => byteSizeText;
            set => SetProperty(ref byteSizeText, value);
        }

        private Command stopCommand;
        public Command StopCommand
        {
            get => stopCommand;
            set => SetProperty(ref stopCommand, value);
        }

        public void SetSendViewMoelState(SendViewModelState state, SendContext context = null)
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
