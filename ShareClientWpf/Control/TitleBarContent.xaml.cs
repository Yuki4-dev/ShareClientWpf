using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShareClientWpf
{
    /// <summary>
    /// TitleBarContent.xaml の相互作用ロジック
    /// </summary>
    public partial class TitleBarContent : UserControl
    {
        private Window _Owner;

        public bool DoubleClickEnable
        {
            get => (bool)GetValue(DoubleClickEnableProperty);
            set => SetValue(DoubleClickEnableProperty, value);
        }

        public TitleBarContent()
        {
            InitializeComponent();
            Loaded += TitleBarContent_Loaded;
        }

        private void TitleBarContent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _Owner?.DragMove();
        }

        private void TitleBarContent_Loaded(object sender, RoutedEventArgs e)
        {
            _Owner = Window.GetWindow(this);
            if (Content is Control control)
            {
                control.MouseDoubleClick += Content_MouseDoubleClick;
            }
        }

        private void Content_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DoubleClickEnable && GetIsHitDoubleClick((UIElement)e.OriginalSource))
            {
                _Owner.WindowState = _Owner.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
        }

        public static void SetIsHitDoubleClick(UIElement element, bool value) => element.SetValue(IsHitDoubleClickProperty, value);
        public static bool GetIsHitDoubleClick(UIElement element) => (bool)element.GetValue(IsHitDoubleClickProperty);

        public static readonly DependencyProperty IsHitDoubleClickProperty = DependencyProperty.RegisterAttached("IsHitDoubleClick", typeof(bool), typeof(TitleBarContent), new PropertyMetadata(false));
        public static readonly DependencyProperty DoubleClickEnableProperty = DependencyProperty.Register(nameof(DoubleClickEnable), typeof(bool), typeof(TitleBarContent), new PropertyMetadata(true));
    }
}
