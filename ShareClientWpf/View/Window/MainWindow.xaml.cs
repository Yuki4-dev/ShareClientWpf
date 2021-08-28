using System.Windows;
using System.Windows.Input;

namespace ShareClientWpf
{
    public partial class MainWindow : WindowBase
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HeaderMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void HeaderMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadViewModel(null, null);
        }

        private void WindowBase_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClearCasheWindow();
        }
    }
}
