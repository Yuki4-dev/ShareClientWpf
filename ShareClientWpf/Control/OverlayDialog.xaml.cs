using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShareClientWpf
{
    /// <summary>
    /// OverlayDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class OverlayDialog : UserControl
    {
        public double DialogWidth
        {
            get => (double)GetValue(DialogWidthProperty);
            set => SetValue(DialogWidthProperty, value);
        }

        public double DialogHeight
        {
            get => (double)GetValue(DialogHeightProperty);
            set => SetValue(DialogHeightProperty, value);
        }

        public OverlayDialog()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.OriginalSource.Equals(sender))
            {
                Visibility = Visibility.Hidden;
            }
        }

        public static readonly DependencyProperty DialogWidthProperty = DependencyProperty.Register(nameof(DialogWidth), typeof(double), typeof(OverlayDialog), new PropertyMetadata(300.0));
        public static readonly DependencyProperty DialogHeightProperty = DependencyProperty.Register(nameof(DialogHeight), typeof(double), typeof(OverlayDialog), new PropertyMetadata(300.0));
    }
}
