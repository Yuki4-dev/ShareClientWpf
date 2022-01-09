using ShareClient.Model.Connect;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShareClientWpf
{
    public interface IClientController : IDisposable
    {
        public Task<bool> ConnectAsync(IPEndPoint iPEndPoint, ConnectionData connectionData);
        public Task SendWindowAsync(SendContext sendContext, SettingContext settingContext);
        public Task<bool> AcceptAsync(int port, Func<IPEndPoint, ConnectionData, ConnectionResponse> acceptCallback);
        public Task ReceiveWindowAsync(Action<ImageSource> pushImage);
        public void CancelConnect();
        public void CloseSendWindow();
        public void CancelAccept();
        public void CloseReceiveWindow();
    }
}
