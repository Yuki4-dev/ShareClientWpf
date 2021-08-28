using ShareClient.Component;
using System;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class ReciveImageProvider : IReceiveDataProvider
    {
        private readonly Action<ImageSource> push;

        public ReciveImageProvider(Action<ImageSource> push)
        {
            this.push = push;
        }

        public void Receive(byte[] data)
        {
            push.Invoke(ImageHelper.Byte2ImageSourse(data));
        }
    }
}
