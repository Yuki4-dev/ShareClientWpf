using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class WindowBase : Window
    {
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
        }

        protected void LoadViewModel(object paramater, Action<object> executeCallback)
        {
            if (DataContext is ViewModelBase vm)
            {
                vm.ShowMessageBox += Vm_ShowMessageBox;
                vm.ShowWindow += Vm_ShowWindow;
                vm.CloseWindow += Vm_CloseWindow;

                Closing += (s, e) => e.Cancel = vm.PostProcces();

                vm.LoadedProcces(paramater, executeCallback);
            }
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
                    windowBase.LoadViewModel(paramater, callback);
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

        protected virtual async Task<MessageBoxResult> Vm_ShowMessageBox(string arg1, MessageBoxButton arg2)
        {
            return await Dispatcher.InvokeAsync(() => MessageDialog.Show(Title, arg1, arg2));
        }
    }
}
