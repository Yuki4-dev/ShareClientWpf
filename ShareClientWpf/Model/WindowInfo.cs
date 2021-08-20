using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class WindowInfo
    {
        public string Title { get; set; }
        public IntPtr WindowHandle { get; set; }
    }

    public class WindowInmageInfo2ImageSourceConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var info = (WindowInfo)value;
            if (info != null && ImageHelper.TryGetWindow(info.WindowHandle, out var image))
            {
                return image;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
