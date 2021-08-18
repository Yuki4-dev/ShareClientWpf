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
            var con = new ConnectionManager();
            connection = await con.AcceptAsync(new IPEndPoint(IPAddress.Any, port), acceptCallback);
            con.Dispose();
        }


        public async Task ReceiveAsync(Action<ImageSource> pushImage)
        {
            Socket = ShareClientSocket.CreateUdpSocket();
            Socket.Open(connection);

            reciever = new ShareClientReceiver(new ShareClientManager(connection.ClientSpec), 
                Socket, 
                new ReciveImageProvider(pushImage));

            await reciever.ReceiveAsync();
        }

        public void Dispose()
        {
            reciever?.Dispose();
        }

    }
}
