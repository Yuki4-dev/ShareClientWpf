using System.Net;

namespace ShareClientWpf
{
    public class RecieveStatusPageViewModel : ModelBase
    {
        private string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        private IPEndPoint iPEndPoint;
        public IPEndPoint IPEndPoint
        {
            get => iPEndPoint;
            set => SetProperty(ref iPEndPoint, value);
        }

        private Profile profile;
        public Profile Profile
        {
            get => profile;
            set => SetProperty(ref profile, value);
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

        public void SetReceiveState(ReceiveViewModelState state, ReceiveContext context = null)
        {
            bool stopExecute = false;
            if (state == ReceiveViewModelState.None)
            {
                Message = "未接続";
                IPEndPoint = null;
                Profile = null;
            }
            else if (state == ReceiveViewModelState.Accept)
            {
                Message = "受信待機中";
                IPEndPoint = null;
                Profile = null;
                stopExecute = true;
            }
            else if (state == ReceiveViewModelState.Receiving)
            {
                Message = "受信中";
                IPEndPoint = context.IPEndPoint;
                Profile = context.Profile;
                stopExecute = true;
            }

            if (StopCommand != null)
            {
                StopCommand.CanExecuteValue = stopExecute;
            }
        }
    }

    public enum ReceiveViewModelState
    {
        None, Accept, Receiving
    }
}
