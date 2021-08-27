using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace ShareClientWpf
{
    public class WindowImageCaputure : IDisposable
    {
        private readonly object obj = new();
        private IntPtr handle;
        private int delay;
        private int windowWidth;
        private ImageFormat format;
        private bool isCaputure = false;

        public event Action<byte[]> CaputureImage;

        public WindowImageCaputure(IntPtr hmdl, int delay, ImageFormat format, int windowWidth = 0)
        {
            this.handle = hmdl;
            this.delay = delay;
            this.windowWidth = windowWidth;
            this.format = format;
        }

        public void Caputure()
        {
            if (ImageHelper.TryGetWindowImage(handle, out Image image))
            {
                if (windowWidth > 0)
                {
                    image = ImageHelper.ResizeImage(image, windowWidth);
                }

                CaputureImage?.Invoke(ImageHelper.Image2Byte(image, format));
                image.Dispose();
            }
        }

        public void Start()
        {
            lock (obj)
            {
                if (isCaputure)
                    throw new Exception("Do Caputure.");

                isCaputure = true;
            }

            while (isCaputure)
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
            isCaputure = false;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
