using ShareClient.Model;
using ShareClient.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing.Imaging;

namespace ShareClientWpf
{
    public class ShreClientController : IClientController
    {
        private Connection receiveConnection;
        private Connection sendConnection;

        private IConnectionManager manager;
        private IConnectionManager Manager
        {
            get => manager;
            set
            {
                if (manager != value)
                {
                    manager?.Dispose();
                    manager = value;
                }
            }
        }

        private ShareClientSender sender;
        private ShareClientSender Sender
        {
            get => sender;
            set
            {
                if (sender != value)
                {
                    sender?.Dispose();
                    sender = value;
                }
            }
        }

        private ShareClientReceiver reciever;
        private ShareClientReceiver Reciever
        {
            get => reciever;
            set
            {
                if (reciever != value)
                {
                    reciever?.Dispose();
                    reciever = value;
                }
            }
        }

        private IClientSocket socket;
        private IClientSocket Socket
        {
            get => socket;
            set
            {
                if (socket != value)
                {
                    socket?.Dispose();
                    socket = value;
                }
            }
        }

        private WindowImageCaputure caputure;
        private WindowImageCaputure Caputure
        {
            get => caputure;
            set
            {
                if (caputure != value)
                {
                    caputure?.Dispose();
                    caputure = value;
                }
            }
        }


        public async Task<bool> AcceptAsync(int port, Func<IPEndPoint, ConnectionData, ConnectionResponse> acceptCallback)
        {
            Manager = new ConnectionManager();
            receiveConnection = await manager.AcceptAsync(new IPEndPoint(IPAddress.Any, port), acceptCallback);
            return receiveConnection != null;
        }


        public async Task ReceiveWindowAsync(Action<ImageSource> pushImage)
        {
            if (receiveConnection == null)
            {
                throw new Exception($"{nameof(receiveConnection)} is null.");
            }

            Socket = ShareClientSocket.CreateUdpSocket();
            Socket.Open(receiveConnection);

            Reciever = new ShareClientReceiver(new ShareClientManager(receiveConnection.ClientSpec),
                Socket,
                new ReciveImageProvider(pushImage));

            await Reciever.ReceiveAsync();
        }

        public async Task<bool> ConnectAsync(IPEndPoint iPEndPoint, ConnectionData connectionData, Func<ConnectionResponse, bool> connectCallback)
        {
            Manager = new ConnectionManager();
            sendConnection = await Manager.ConnectAsync(iPEndPoint, connectionData, connectCallback);
            return sendConnection != null;
        }

        public async Task SendWindowAsync(SendContext sendContext, SettingContext settingContext)
        {
            if (sendConnection == null)
            {
                throw new Exception($"{nameof(sendConnection)} is null.");
            }

            var SendSocket = ShareClientSocket.CreateUdpSocket();
            SendSocket.Open(sendConnection);

            Sender = new ShareClientSender(new ShareClientManager(sendConnection.ClientSpec), SendSocket);
            Caputure = new WindowImageCaputure(sendContext.WindowInfo.WindowHandle,
                                                settingContext.SendDelay,
                                                settingContext.Format,
                                                settingContext.SendWidth);
            Caputure.CaputureImage += Caputure_CaputureImage;
            await Caputure.StartAsync();
            Caputure.CaputureImage -= Caputure_CaputureImage;
        }

        private void Caputure_CaputureImage(byte[] obj)
        {
            Sender.Send(obj);
        }

        public void Dispose()
        {
            Cancel();
        }

        public void Cancel()
        {
            Caputure?.Stop();
            Reciever?.Close();
            Socket?.Close();
            Manager?.Cancel();
        }
    }
}
