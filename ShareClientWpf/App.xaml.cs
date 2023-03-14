using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ShareClientWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }

    public class WindowHandleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            var handle = (IntPtr)value;
            int width = parameter != null ? int.Parse((string)parameter) : 0;
            var image = GetImageSource(handle, width);

            return image ?? DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        private ImageSource GetImageSource(IntPtr handle, int width)
        {
            ImageSource imageSrc = null;
            if (width > 0)
            {
                if (ImageHelper.TryGetWindowImage(handle, out var image))
                {
                    var resizeImage = ImageHelper.ResizeImage(image, width);
                    imageSrc = ImageHelper.Byte2ImageSource(ImageHelper.Image2Byte(resizeImage, ImageFormat.Jpeg));
                }
            }
            else if (ImageHelper.TryGetWindowImageSource(handle, out var image))
            {
                imageSrc = image;
            }

            return imageSrc;
        }
    }
}
