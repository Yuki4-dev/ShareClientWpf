using CommunityToolkit.Mvvm.ComponentModel;
using System.Net;

namespace ShareClientWpf
{
    [ObservableObject]
    public partial class SendContext
    {
        [ObservableProperty]
        private IPEndPoint iPEndPoint;

        [ObservableProperty]
        private WindowInfo windowInfo;
    }
}
