using ShareClient.Component;
using ShareClient.Model;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class ShreClientController : IClientController
    {
        private readonly object obj = new();
        private bool isDiposed = false;

        private bool isSenderConnect = false;
        private Connection senderConnection;
        private ShareClientSender sender;
        private IConnectionManager senderManager;

        private bool receiverConnect = false;
        private Connection receiverConnection;
        private ShareClientReceiver reciever;
        private IConnectionManager receiverManager;

        private WindowImageCaputure caputure;

        public async Task<bool> AcceptAsync(int port, Func<IPEndPoint, ConnectionData, ConnectionResponse> acceptCallback)
        {
            ThrowIfDisposed();
            lock (obj)
            {
                if (receiverConnect)
                {
                    throw new Exception($"Already Run Connecting.");
                }
                receiverConnect = true;
            }

            try
            {

                receiverManager = new ConnectionManager();
                receiverConnection = await receiverManager.AcceptAsync(new IPEndPoint(IPAddress.Any, port), acceptCallback);
            }
            finally
            {
                receiverManager.Dispose();
                receiverConnect = false;
            }

            return receiverConnection != null;
        }


        public async Task ReceiveWindowAsync(Action<ImageSource> pushImage)
        {
            ThrowIfDisposed();
            if (receiverConnection == null)
            {
                throw new Exception($"No Accept, Connection is null");
            }

            lock (obj)
            {
                if (reciever != null && reciever.Socket.IsOpen)
                {
                    throw new Exception($"Already Run Receiving.");
                }
                reciever = new ShareClientReceiver(new ShareClientManager(receiverConnection.ClientSpec), GetSocketAndOpen(receiverConnection),
                                                    new ReciveImageProvider(pushImage));
            }

            try
            {
                await reciever.ReceiveAsync();
            }
            finally
            {
                CloseReceiveWindow();
            }
        }

        public async Task<bool> ConnectAsync(IPEndPoint iPEndPoint, ConnectionData connectionData, Action<ConnectionResponse> connectCallback)
        {
            ThrowIfDisposed();
            lock (obj)
            {
                if (isSenderConnect)
                {
                    throw new Exception($"Already Run Connecting.");
                }
                isSenderConnect = true;
            }

            try
            {
                senderManager = new ConnectionManager();
                senderConnection = await senderManager.ConnectAsync(iPEndPoint, connectionData, connectCallback);
            }
            finally
            {
                senderManager.Dispose();
                isSenderConnect = false;
            }

            return senderConnection != null;
        }

        public async Task SendWindowAsync(SendContext sendContext, SettingContext settingContext)
        {
            ThrowIfDisposed();

            if (senderConnection == null)
            {
                throw new Exception($"No Connect, Connection is null");
            }

            lock (obj)
            {
                if (sender != null && sender.Socket.IsOpen)
                {
                    throw new Exception($"Already Run Sending.");
                }
                sender = new ShareClientSender(new ShareClientManager(senderConnection.ClientSpec), GetSocketAndOpen(senderConnection));
            }

            caputure = new WindowImageCaputure(sendContext.WindowInfo.WindowHandle,
                                                settingContext.SendDelay,
                                                settingContext.Format,
                                                settingContext.SendWidth);
            caputure.CaputureImage += (img) => sender.Send(img);

            try
            {
                await caputure.StartAsync();
            }
            finally
            {
                CloseSendWindow();
            }
        }

        public void CancelAccept()
        {
            receiverConnection = null;
            receiverManager?.Cancel();
            receiverConnect = false;
        }

        public void CloseReceiveWindow()
        {
            receiverConnection = null;
            reciever?.Close();
        }

        public void CancelConnect()
        {
            senderConnection = null;
            senderManager?.Cancel();
            isSenderConnect = false;
        }

        public void CloseSendWindow()
        {
            senderConnection = null;
            caputure?.Stop();
            sender?.Close();
        }

        private void ThrowIfDisposed()
        {
            if (isDiposed)
            {
                throw new ObjectDisposedException(GetType().ToString());
            }
        }

        private IClientSocket GetSocketAndOpen(Connection connection)
        {
            IClientSocket socket = ShareClientSocket.Udp;
            try
            {
                socket.Open(connection.LocalEndPoint, connection.RemoteEndPoint);
            }
            catch
            {
                socket.Dispose();
                throw;
            }

            return socket;
        }

        public void Dispose()
        {
            isDiposed = true;
            caputure?.Stop();
            reciever?.Close();
            sender?.Close();
            senderManager?.Cancel();
            receiverManager?.Cancel();
        }
    }
}
