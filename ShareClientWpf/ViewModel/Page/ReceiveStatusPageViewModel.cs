using CommunityToolkit.Mvvm.ComponentModel;
using System.Net;
using System.Windows.Input;

namespace ShareClientWpf
{
    [ObservableObject]
    public partial class ReceiveStatusPageViewModel
    {
        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private bool stopEnable;

        [ObservableProperty]
        private IPEndPoint iPEndPoint;

        [ObservableProperty]
        private Profile profile;

        [ObservableProperty]
        private string byteSizeText;

        [ObservableProperty]
        private ICommand stopCommand;

        public void SetReceiveState(ReceiveViewModelState state, ReceiveContext context = null)
        {
            if (state == ReceiveViewModelState.None)
            {
                Message = "未接続";
                IPEndPoint = null;
                Profile = null;
                StopEnable = false;
            }
            else if (state == ReceiveViewModelState.Accept)
            {
                Message = "受信待機中";
                IPEndPoint = null;
                Profile = null;
                StopEnable = true;
            }
            else if (state == ReceiveViewModelState.Receiving)
            {
                Message = "受信中";
                IPEndPoint = context.IPEndPoint;
                Profile = context.Profile;
                StopEnable = true;
            }
        }
    }

    public enum ReceiveViewModelState
    {
        None, Accept, Receiving
    }
}
