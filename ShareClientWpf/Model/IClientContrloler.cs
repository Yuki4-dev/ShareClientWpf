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
        public Task AcceptAsync(int port, Func<IPEndPoint, ConnectionData, bool> acceptCallback);
        Task ReceiveAsync(Action<ImageSource> pushImage);
        void Cancel();
    }
}
