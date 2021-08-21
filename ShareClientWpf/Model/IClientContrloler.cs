using ShareClient.Model;
using ShareClient.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShareClientWpf
{
    public interface IClientContrloler : IDisposable
    {
        public Task<bool> AcceptAsync(int port, Func<IPEndPoint, ConnectionData, ConnectionResponse> acceptCallback);
        public Task ReceiveWindowAsync(Action<ImageSource> pushImage);
        public Task<bool> ConnectAsync(IPEndPoint iPEndPoint, ConnectionData connectionData, Func<ConnectionResponse, bool> connectCallback);
        public Task SendWindowAsync(SendContext sendContext, SettingContext settingContext);
        void Cancel();
    }
}
