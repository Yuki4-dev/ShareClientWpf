using ShareClient.Component.Algorithm;
using ShareClient.Component.Connect;
using ShareClient.Model.Connect;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class ShareClientController : IClientController
    {
        private bool isDiposed = false;

        private bool isCancelConnect = false;
        private Connection senderConnection;
        private ISendAlgorithm sender;

        private bool isCancelAccept = false;
        private Connection receiverConnection;
        private IRecieveAlgorithm reciever;

        private WindowImageCaputure caputure;

        public async Task<bool> AcceptAsync(int port, Func<IPEndPoint, ConnectionData, ConnectionResponse> acceptCallback)
        {
            ThrowIfDisposed();

            isCancelAccept = false;
            receiverConnection = await Task.Run(() => Connection.Builder()
                                                                .SetCancellation(() => isCancelAccept)
                                                                .SetAcceptRequest(acceptCallback)
                                                                .Accept(new IPEndPoint(IPAddress.Any, port)));

            return receiverConnection != null;
        }

        public async Task ReceiveWindowAsync(Action<ImageSource> pushImage, Action closed)
        {
            ThrowIfDisposed();

            if (receiverConnection == null)
            {
                throw new Exception($"No Accept, Connection is null");
            }
            else if (reciever != null && !reciever.IsClosed)
            {
                throw new Exception($"Already Run Receiving.");
            }

            reciever = ShareAlgorithmBuilder.NewBuilder()
                                            .SetShareClientSpec(receiverConnection.ClientSpec)
                                            .SetConnectEndoPoint(receiverConnection.RemoteEndPoint)
                                            .BuildRecieve(receiverConnection.LocalEndPoint);
            reciever.ShareAlgorithmClosed += (_, __) => closed.Invoke();

            try
            {
                await reciever.RecieveAsync((data) =>
                {
                    pushImage.Invoke(ImageHelper.Byte2ImageSourse(data));
                });
            }
            finally
            {
                CloseReceiveWindow();
            }
        }

        public async Task<bool> ConnectAsync(IPEndPoint iPEndPoint, ConnectionData connectionData)
        {
            ThrowIfDisposed();

            isCancelConnect = false;
            senderConnection = await Task.Run(() => Connection.Builder()
                                                              .SetCancellation(() => isCancelConnect)
                                                              .Connect(iPEndPoint, connectionData));
            return senderConnection != null;
        }

        public async Task SendWindowAsync(SendContext sendContext, SettingContext settingContext)
        {
            ThrowIfDisposed();

            if (senderConnection == null)
            {
                throw new Exception($"No Connect, Connection is null");
            }
            else if (sender != null && !sender.IsClosed)
            {
                throw new Exception($"Already Run Sending.");
            }

            sender = ShareAlgorithmBuilder.NewBuilder()
                                          .SetShareClientSpec(senderConnection.ClientSpec)
                                          .SetLocalEndoPoint(senderConnection.LocalEndPoint)
                                          .BuildSend(senderConnection.RemoteEndPoint);

            caputure = new WindowImageCaputure(sendContext.WindowInfo.WindowHandle,
                                               settingContext.SendDelay,
                                               settingContext.Format,
                                               settingContext.SendWidth);

            caputure.CaputureImage += (img) =>
            {
                if (!sender.IsClosed)
                {
                    sender.Send(img);
                }
            };

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
            isCancelAccept = true;
        }

        public void CancelConnect()
        {
            isCancelConnect = true;
        }

        public void CloseReceiveWindow()
        {
            receiverConnection = null;
            reciever?.Dispose();
        }

        public void CloseSendWindow()
        {
            senderConnection = null;
            caputure?.Stop();
            sender?.Dispose();
        }

        private void ThrowIfDisposed()
        {
            if (isDiposed)
            {
                throw new ObjectDisposedException(GetType().ToString());
            }
        }

        public void Dispose()
        {
            isDiposed = true;
            CloseReceiveWindow();
            CloseSendWindow();
            CancelAccept();
            CancelConnect();
        }
    }
}
