using CommunityToolkit.Mvvm.ComponentModel;
using System.Net;

namespace ShareClientWpf
{
    [ObservableObject]
    public partial class ReceiveStatusPageViewModel
    {
        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private IPEndPoint iPEndPoint;

        [ObservableProperty]
        private Profile profile;

        [ObservableProperty]
        private string byteSizeText;

        [ObservableProperty]
        private Command stopCommand;

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
