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
    public class ShreClientController : IClientContrloler
    {
        private IConnectionManager manager;
        private Connection connection;
        private ShareClientReceiver reciever;

        private IClientSocket socket;
        private IClientSocket Socket
        {
            get => socket;
            set
            {
                socket?.Dispose();
                socket = value;
            }
        }

        public async Task AcceptAsync(int port, Func<IPEndPoint, ConnectionData, bool> acceptCallback)
        {
            manager = new ConnectionManager();
            connection = await manager.AcceptAsync(new IPEndPoint(IPAddress.Any, port), acceptCallback);
            manager.Dispose();
        }


        public async Task ReceiveAsync(Action<ImageSource> pushImage)
        {
            if (connection == null)
            {
                return;
            }

            Socket = ShareClientSocket.CreateUdpSocket();
            Socket.Open(connection);

            reciever = new ShareClientReceiver(new ShareClientManager(connection.ClientSpec),
                Socket,
                new ReciveImageProvider(pushImage));

            await reciever.ReceiveAsync();
        }

        public void Dispose()
        {
            Cancel();
        }

        public void Cancel()
        {
            manager?.Cancel();
            reciever?.Close();
        }
    }
}
