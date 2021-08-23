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

    public class WindowHandleConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            var handle = (IntPtr)value;
            if (parameter != null && int.TryParse(parameter.ToString(), out var width))
            {
                if (ImageHelper.TryGetWindowImage(handle, out var img))
                {
                    var resizeBmp = ImageHelper.ResizeImage(img, width);
                    return ImageHelper.Byte2ImageSourse(ImageHelper.Image2Byte(resizeBmp, ImageFormat.Jpeg));
                }
            }
            else if (ImageHelper.TryGetWindowImageSource(handle, out var image))
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
