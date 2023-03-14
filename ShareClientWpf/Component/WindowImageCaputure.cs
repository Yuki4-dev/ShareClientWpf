using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace ShareClientWpf
{
    public class WindowImageCapture : IDisposable
    {
        private readonly IntPtr handle;
        private readonly int delay;
        private readonly int windowWidth;
        private readonly ImageFormat format;
        private bool isCapture = false;

        public event Action<byte[]> CaptureImage;

        public WindowImageCapture(IntPtr hmdl, int delay, ImageFormat format, int windowWidth = 0)
        {
            this.handle = hmdl;
            this.delay = delay;
            this.windowWidth = windowWidth;
            this.format = format;
        }

        public void Capture()
        {
            if (ImageHelper.TryGetWindowImage(handle, out Image image))
            {
                if (windowWidth > 0)
                {
                    image = ImageHelper.ResizeImage(image, windowWidth);
                }

                CaptureImage?.Invoke(ImageHelper.Image2Byte(image, format));
                image.Dispose();
            }
        }

        public void Start()
        {
            if (isCapture)
            {
                throw new Exception("Do Capture.");
            }
            isCapture = true;

            while (isCapture)
            {
                Capture();
                Thread.Sleep(delay);
            }
        }

        public async Task StartAsync()
        {
            await Task.Run(Start);
        }

        public void Stop()
        {
            isCapture = false;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
