using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShareClientWpf
{
    /// <summary>
    /// TitleBarContent.xaml の相互作用ロジック
    /// </summary>
    public partial class TitleBarContent : UserControl
    {
        private Window owner;

        public bool DoubleClickEnable
        {
            get => (bool)GetValue(DoubleClickEnableProperty);
            set => SetValue(DoubleClickEnableProperty, value);
        }
        public static readonly DependencyProperty DoubleClickEnableProperty =
            DependencyProperty.Register(nameof(DoubleClickEnable), typeof(bool), typeof(TitleBarContent), new PropertyMetadata(true));

        public TitleBarContent()
        {
            InitializeComponent();
        }

        private void TitleBarContent_Loaded(object sender, RoutedEventArgs e)
        {
            owner = Window.GetWindow(this);
            if (Content is Control control)
            {
                control.MouseDoubleClick += Content_MouseDoubleClick;
            }
        }

        private void TitleBarContent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                owner?.DragMove();
            }
        }

        private void Content_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DoubleClickEnable && GetIsHitDoubleClick((UIElement)e.OriginalSource))
            {
                owner.WindowState = owner.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
        }

        public static void SetIsHitDoubleClick(UIElement element, bool value) => element.SetValue(IsHitDoubleClickProperty, value);
        public static bool GetIsHitDoubleClick(UIElement element) => (bool)element.GetValue(IsHitDoubleClickProperty);
        public static readonly DependencyProperty IsHitDoubleClickProperty = DependencyProperty.RegisterAttached("IsHitDoubleClick", typeof(bool), typeof(TitleBarContent), new PropertyMetadata(false));
    }
}

