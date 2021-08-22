using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ShareClientWpf
{
    public class SettingContext
    {
        public int SendDelay { get; set; }
        public int SendWidth { get; set; }
        public ImageFormat Format { get; set; }
    }

    public class ImageFormat2TextConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return "";
            }

            var f = (ImageFormat)value;
            return f.ToString().Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
