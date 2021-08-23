using ShareClient.Model;
using ShareClient.Component;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class ShreClientController : IClientController
    {
        private Connection senderConnection;
        private ShareClientSender sender;
        private IConnectionManager senderManager;

        private Connection receiverConnection;
        private ShareClientReceiver reciever;
        private IConnectionManager receiverManager;

        private WindowImageCaputure caputure;

        public async Task<bool> AcceptAsync(int port, Func<IPEndPoint, ConnectionData, ConnectionResponse> acceptCallback)
        {
            receiverManager = new ConnectionManager();
            receiverConnection = await receiverManager.AcceptAsync(new IPEndPoint(IPAddress.Any, port), acceptCallback);
            receiverManager.Dispose();
            return receiverConnection != null;
        }

        public async Task ReceiveWindowAsync(Action<ImageSource> pushImage)
        {
            if (receiverConnection == null)
            {
                throw new Exception($"No Accept, Connection is null");
            }

            using var socket = ShareClientSocket.CreateUdpSocket();
            socket.Open(receiverConnection);

            reciever = new ShareClientReceiver(new ShareClientManager(receiverConnection.ClientSpec),
                                                socket,
                                                new ReciveImageProvider(pushImage));

            await reciever.ReceiveAsync();

            receiverConnection = null;
            reciever.Dispose();
        }

        public async Task<bool> ConnectAsync(IPEndPoint iPEndPoint, ConnectionData connectionData, Func<ConnectionResponse, bool> connectCallback)
        {
            senderManager = new ConnectionManager();
            senderConnection = await senderManager.ConnectAsync(iPEndPoint, connectionData, connectCallback);
            return senderConnection != null;
        }

        public async Task SendWindowAsync(SendContext sendContext, SettingContext settingContext)
        {
            if (senderConnection == null)
            {
                throw new Exception($"No Connect, Connection is null");
            }

            using var socket = ShareClientSocket.CreateUdpSocket();
            socket.Open(senderConnection);

            sender = new ShareClientSender(new ShareClientManager(senderConnection.ClientSpec), socket);
            caputure = new WindowImageCaputure(sendContext.WindowInfo.WindowHandle,
                                                settingContext.SendDelay,
                                                settingContext.Format,
                                                settingContext.SendWidth);

            caputure.CaputureImage += (img) => sender.Send(img); ;
            await caputure.StartAsync();

            senderConnection = null;
            caputure.Dispose();
            sender.Dispose();
        }

        public void CancelAccept() => receiverManager?.Cancel();
        public void CancelReceiveWindow() => reciever?.Close();
        public void CancelConnect() => senderManager?.Cancel();
        public void CancelSendWindow()
        {
            caputure?.Stop();
            sender?.Close();
        }

        public void Dispose()
        {
            caputure?.Stop();
            reciever?.Close();
            sender?.Close();
            senderManager?.Cancel();
            receiverManager?.Cancel();
        }
    }
}
