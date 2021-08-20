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
    public class ShreClientController : ModelBase, IClientContrloler
    {
        private Connection receiveConnection;
        private Connection sendConnection;

        private IConnectionManager manager;
        private IConnectionManager Manager
        {
            get => manager;
            set
            {
                manager?.Dispose();
                SetProperty(ref manager, value);
            }
        }

        private ShareClientSender sender;
        private ShareClientSender Sender
        {
            get => sender;
            set
            {
                sender?.Dispose();
                SetProperty(ref sender, value);
            }
        }

        private ShareClientReceiver reciever;
        private ShareClientReceiver Reciever
        {
            get => reciever;
            set
            {
                reciever?.Dispose();
                SetProperty(ref reciever, value);
            }
        }

        private IClientSocket socket;
        private IClientSocket Socket
        {
            get => socket;
            set
            {
                socket?.Dispose();
                SetProperty(ref socket, value);
            }
        }

        private WindowImageCaputure caputure;
        private WindowImageCaputure Caputure
        {
            get => caputure;
            set
            {
                caputure?.Dispose();
                SetProperty(ref caputure, value);
            }
        }


        public async Task AcceptAsync(int port, Func<IPEndPoint, ConnectionData, bool> acceptCallback)
        {
            Manager = new ConnectionManager();
            receiveConnection = await manager.AcceptAsync(new IPEndPoint(IPAddress.Any, port), acceptCallback);
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

        public async Task ConnectAsync(IPEndPoint iPEndPoint, ConnectionData connectionData)
        {
            Manager = new ConnectionManager();
            sendConnection = await Manager.ConnectAsync(iPEndPoint, connectionData);
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
