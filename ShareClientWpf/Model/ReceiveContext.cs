using CommunityToolkit.Mvvm.ComponentModel;
using System.Net;

namespace ShareClientWpf
{
    [ObservableObject]
    public partial class ReceiveContext
    {
        [ObservableProperty]
        private IPEndPoint iPEndPoint;

        [ObservableProperty]
        private Profile profile;
    }
}
