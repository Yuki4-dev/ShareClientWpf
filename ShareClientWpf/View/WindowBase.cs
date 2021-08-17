using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class WindowBase : Window
    {
        protected Action<object> ExecuteCallback
        {
            get => (Action<object>)GetValue(ExecuteCallbackProperty);
            set => SetValue(ExecuteCallbackProperty, value);
        }
        public static readonly DependencyProperty ExecuteCallbackProperty =
            DependencyProperty.Register(nameof(ExecuteCallback), typeof(Action<object>), typeof(WindowBase), new PropertyMetadata(null));


        protected object Paramater
        {
            get => GetValue(ParamaterProperty);
            set => SetValue(ParamaterProperty, value);
        }
        public static readonly DependencyProperty ParamaterProperty =
            DependencyProperty.Register(nameof(Paramater), typeof(Action<object>), typeof(WindowBase), new PropertyMetadata(null));

        public Brush ThemeBrush
        {
            get => (Brush)GetValue(ThemeBrushProperty);
            set => SetValue(ThemeBrushProperty, value);
        }
        public static readonly DependencyProperty ThemeBrushProperty =
            DependencyProperty.Register(nameof(ThemeBrush), typeof(Brush), typeof(WindowBase), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

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
            vm.ShowMessageBox += Vm_ShowMessageBox;
            vm.ShowWindow += Vm_ShowWindow;
            vm.CloseWindow += Vm_CloseWindow;
            Closing += (s, e) => e.Cancel = vm.PostProcces();
        }

        protected virtual bool Vm_CloseWindow()
        {
            Close();
            return true;
        }

        protected virtual async Task Vm_ShowWindow(Type windowType, bool isModal, object paramater, Action<object> callback)
        {

            await Dispatcher.InvokeAsync(() =>
            {
                var window = (Window)Activator.CreateInstance(windowType);
                if (window is WindowBase windowBase)
                {
                    windowBase.Paramater = paramater;
                    windowBase.ExecuteCallback = callback;
                }

                if (isModal)
                {
                    window.ShowDialog();
                }
                else
                {
                    window.Show();
                }
            });
           
        }

        protected virtual MessageBoxResult Vm_ShowMessageBox(string arg1, MessageBoxButton arg2)
        {
            return MessageDialog.Show(Title, arg1, arg2);
        }
    }
}
