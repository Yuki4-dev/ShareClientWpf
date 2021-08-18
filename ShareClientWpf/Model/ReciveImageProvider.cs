using ShareClient.Component;
using ShareClient.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class ReciveImageProvider : IReceiveDataProvider
    {
        private Action<ImageSource> push;

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
