using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ShareClientWpf
{
    public class WindowImageCaputure : IDisposable
    {
        private IntPtr hmdl;
        private int delay;
        private int windowWidth;
        private ImageFormat format;
        private CancellationTokenSource tokenSource;

        public event Action<byte[]> CaputureImage;

        public WindowImageCaputure(IntPtr hmdl, int delay, ImageFormat format, int windowWidth = 0)
        {
            this.hmdl = hmdl;
            this.delay = delay;
            this.windowWidth = windowWidth;
            this.format = format;
        }

        public void Caputure()
        {
            if (ImageHelper.TryGetWindowBmp(hmdl, out var bmp))
            {
                if (windowWidth > 0)
                {
                    bmp = ImageHelper.ResizeBmp(bmp, windowWidth);
                }
                CaputureImage?.Invoke(ImageHelper.BitMap2Byte(bmp, format));
            }
        }

        public void Start()
        {
            tokenSource?.Dispose();
            tokenSource = new CancellationTokenSource();

            var token = tokenSource.Token;
            while (!token.IsCancellationRequested)
            {
                Caputure();
                Thread.Sleep(delay);
            }
        }

        public async Task StartAsync()
        {
            await Task.Run(Start);
        }

        public void Stop()
        {
            tokenSource?.Cancel();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
