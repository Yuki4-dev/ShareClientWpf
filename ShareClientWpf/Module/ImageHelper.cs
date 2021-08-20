using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;

namespace ShareClientWpf
{
    public class ImageHelper
    {
        public static ImageSource Byte2ImageSourse(byte[] data)
        {
            using var ms = new WrappingStream(new MemoryStream(data));
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.None;
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }

        public static byte[] BitMap2Byte(Bitmap bitmap, ImageFormat format)
        {
            using var ms = new MemoryStream();
            bitmap.Save(ms, format);
            return ms.GetBuffer();
        }

        public static bool TryGetWindowBmp(IntPtr hWnd, out Bitmap bmp)
        {
            if (!NativeMethod.GetWindowRect(hWnd, out var rect))
            {
                bmp = null;
                return false;
            }

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;
            if (width <= 0 || height <= 0)
            {
                bmp = null;
                return false;
            }

            var rectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            bmp = new Bitmap(rectangle.Width, rectangle.Height);

            using var g = Graphics.FromImage(bmp);
            g.CopyFromScreen(new Point(rectangle.X, rectangle.Y), new Point(0, 0), rectangle.Size);

            return true;
        }

        public static bool TryGetWindow(IntPtr hWnd, out ImageSource image)
        {
            if (!NativeMethod.GetWindowRect(hWnd, out var rect))
            {
                image = null;
                return false;
            }

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;
            if (width <= 0 || height <= 0)
            {
                image = null;
                return false;
            }

            var rectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            var windowBmp = new Bitmap(rectangle.Width, rectangle.Height);

            using var g = Graphics.FromImage(windowBmp);
            g.CopyFromScreen(new Point(rectangle.X, rectangle.Y), new Point(0, 0), rectangle.Size);

            using var ms = new WrappingStream(new MemoryStream());
            windowBmp.Save(ms, ImageFormat.Png);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.None;
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            bitmap.Freeze();

            image = bitmap;
            return true;
        }

        public static Bitmap ResizeBmp(Bitmap baseBmp, int width, InterpolationMode mode = InterpolationMode.Low)
        {
            var height = (int)(baseBmp.Height * (width / (double)baseBmp.Width));
            return ResizeBmp(baseBmp, height, width, mode);
        }

        public static Bitmap ResizeBmp(Bitmap baseBmp, int height, int width, InterpolationMode mode = InterpolationMode.Low)
        {
            if (baseBmp == null)
            {
                throw new ArgumentNullException(nameof(baseBmp));
            }

            var resizeBmp = new Bitmap(width, height);
            using var g = Graphics.FromImage(resizeBmp);
            g.InterpolationMode = mode;
            g.DrawImage(baseBmp, 0, 0, width, height);

            return resizeBmp;
        }
    }
}
