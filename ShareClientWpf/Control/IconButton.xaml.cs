using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ShareClientWpf
{
    /// <summary>
    /// IconButton.xaml の相互作用ロジック
    /// </summary>
    public partial class IconButton : Button
    {
        public IconButton()
        {
            InitializeComponent();
        }
    }

    public class IconSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
