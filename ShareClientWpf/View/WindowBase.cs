using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class WindowBase : Window
    {
        protected Action<object> Callback { get; set; }
        protected object Paramater { get; set; }

        public Brush ThemeBrush
        {
            get => (Brush)GetValue(ThemeBrushProperty);
            set => SetValue(ThemeBrushProperty, value);
        }
        public static readonly DependencyProperty ThemeBrushProperty =
            DependencyProperty.Register(nameof(ThemeBrush), typeof(Brush), typeof(WindowBase), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        public WindowBase()
        {
            NativeMethod.DwmGetColorizationColor(out var rgb, out var b);
            var color = Color.FromArgb((byte)(rgb >> 24), (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
            ThemeBrush = new SolidColorBrush(color);

            Loaded += WindowBase_Loaded;
        }

        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModelBase vm)
            {
                PreViewModel(vm);
            }
        }

        private void PreViewModel(ViewModelBase vm)
        {
            vm.ShowMessageBox += ViewModel_ShowMessageBox;
            vm.ShowWindow += Vm_ShowWindow;
            Closing += (s, e) => e.Cancel = vm.PostProcces();
        }

        protected virtual void Vm_ShowWindow(Type windowType, bool isModal, object paramater, Action<object> callback)
        {
            var window = (Window)Activator.CreateInstance(windowType);
            if (window is WindowBase windowBase)
            {
                windowBase.Callback = callback;
                windowBase.Paramater = paramater;
            }

            if (isModal)
            {
                window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }

        protected virtual MessageBoxResult ViewModel_ShowMessageBox(string arg1, MessageBoxButton arg2)
        {
            return MessageDialog.Show(Title, arg1, arg2);
        }
    }
}
