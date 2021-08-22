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

        public static byte[] Image2Byte(Image image, ImageFormat format)
        {
            using var ms = new MemoryStream();
            image.Save(ms, format);
            return ms.GetBuffer();
        }


        public static Image Byte2Image(byte[] data, ImageFormat format)
        {
            var ms = new WrappingStream(new MemoryStream(data));
            return Image.FromStream(ms);
        }

        public static ImageSource Image2ImageSource(Image image, ImageFormat format)
        {
            var byteBmp = Image2Byte(image, format);
            return Byte2ImageSourse(byteBmp);
        }

        public static bool TryGetWindowImage(IntPtr hWnd, out Image image)
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
            image = new Bitmap(rectangle.Width, rectangle.Height);

            using var g = Graphics.FromImage(image);
            g.CopyFromScreen(new Point(rectangle.X, rectangle.Y), new Point(0, 0), rectangle.Size);

            return true;
        }

        public static bool TryGetWindowImageSource(IntPtr hWnd, out ImageSource image)
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

        public static Image ResizeImage(Image image, int width, InterpolationMode mode = InterpolationMode.Low)
        {
            var height = (int)(image.Height * (width / (double)image.Width));
            return ResizeImage(image, height, width, mode);
        }

        public static Image ResizeImage(Image image, int height, int width, InterpolationMode mode = InterpolationMode.Low)
        { 
            var resizeBmp = new Bitmap(width, height);
            using var g = Graphics.FromImage(resizeBmp);
            g.InterpolationMode = mode;
            g.DrawImage(image, 0, 0, width, height);

            return resizeBmp;
        }
    }
}
