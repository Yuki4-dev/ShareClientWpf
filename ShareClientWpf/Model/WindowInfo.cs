using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class WindowInfo : ModelBase
    {
        public string Title { get; set; }
        public IntPtr WindowHandle { get; set; }

        private bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }
    }

    public class WindowInmageInfo2ImageSourceConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var handle = (IntPtr)value;
            if(parameter != null && int.TryParse(parameter.ToString(),out var width))
            {
                if (ImageHelper.TryGetWindowBmp(handle, out var bmp))
                {
                    var resizeBmp = ImageHelper.ResizeBmp(bmp, width);
                    return ImageHelper.Byte2ImageSourse(ImageHelper.BitMap2Byte(resizeBmp, ImageFormat.Jpeg));
                }
                else
                {
                    return DependencyProperty.UnsetValue;
                }
            }

            if (ImageHelper.TryGetWindow(handle, out var image))
            {
                return image;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
