using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShareClientWpf
{
    /// <summary>
    /// CustomScrollViewer.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomScrollViewer : ScrollViewer
    {
        public double BarSize
        {
            get => (double)GetValue(BarSizeProperty);
            set => SetValue(BarSizeProperty, value);
        }
        public static readonly DependencyProperty BarSizeProperty =
            DependencyProperty.Register(nameof(BarSize), typeof(double), typeof(CustomScrollViewer), new PropertyMetadata(11.0));

        public Thickness VerticalBarMargin
        {
            get => (Thickness)GetValue(VerticalBarMarginProperty);
            set => SetValue(VerticalBarMarginProperty, value);
        }
        public static readonly DependencyProperty VerticalBarMarginProperty =
            DependencyProperty.Register(nameof(VerticalBarMargin), typeof(Thickness), typeof(CustomScrollViewer), new PropertyMetadata(new Thickness(0)));

        public Thickness HorizontalBarMargin
        {
            get => (Thickness)GetValue(HorizontalBarMarginProperty);
            set => SetValue(HorizontalBarMarginProperty, value);
        }
        public static readonly DependencyProperty HorizontalBarMarginProperty =
            DependencyProperty.Register(nameof(HorizontalBarMargin), typeof(Thickness), typeof(CustomScrollViewer), new PropertyMetadata(new Thickness(0)));

        public Brush TrackBrush
        {
            get => (Brush)GetValue(TrackBrushProperty);
            set => SetValue(TrackBrushProperty, value);
        }
        public static readonly DependencyProperty TrackBrushProperty =
            DependencyProperty.Register(nameof(TrackBrush), typeof(Brush), typeof(CustomScrollViewer), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public Brush TabBrush
        {
            get => (Brush)GetValue(TabBrushProperty);
            set => SetValue(TabBrushProperty, value);
        }
        public static readonly DependencyProperty TabBrushProperty =
            DependencyProperty.Register(nameof(TabBrush), typeof(Brush), typeof(CustomScrollViewer), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(128, 255, 255, 255))));

        public double TabRedius
        {
            get => (double)GetValue(TabRediusProperty);
            set => SetValue(TabRediusProperty, value);
        }
        public static readonly DependencyProperty TabRediusProperty =
            DependencyProperty.Register(nameof(TabRedius), typeof(double), typeof(CustomScrollViewer), new PropertyMetadata(6.0));

        public CustomScrollViewer()
        {
            InitializeComponent();
        }
    }
}
