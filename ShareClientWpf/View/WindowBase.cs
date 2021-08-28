using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class WindowBase : Window
    {
        private static readonly Dictionary<Type, WindowBase> casheWindows = new();

        public bool IsCasheWindow
        {
            get => (bool)GetValue(IsCasheWindowProperty);
            set => SetValue(IsCasheWindowProperty, value);
        }
        public static readonly DependencyProperty IsCasheWindowProperty =
            DependencyProperty.Register(nameof(IsCasheWindow), typeof(bool), typeof(WindowBase), new PropertyMetadata(false));

        public bool IsShowDialog
        {
            get => (bool)GetValue(IsShowDialogProperty);
            set => SetValue(IsShowDialogProperty, value);
        }
        public static readonly DependencyProperty IsShowDialogProperty =
            DependencyProperty.Register(nameof(IsShowDialog), typeof(bool), typeof(WindowBase), new PropertyMetadata(false));

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

            Closed += WindowBase_Closed;
        }

        protected void LoadViewModel(object paramater, Action<object> executeCallback)
        {
            if (DataContext is ViewModelBase vm)
            {
                vm.ShowMessageBox += ShowMessageBox;
                vm.ShowWindow += ShowWindow;
                vm.ShowCommonDialog += ShowCommonDialog;
                vm.CloseWindow += CloseWindow;
                Closing += (s, e) => e.Cancel = vm.PostProcces();

                vm.LoadedProcess(paramater, executeCallback);
            }
        }

        protected void UnLoadViewModel()
        {
            if (DataContext is ViewModelBase vm)
            {
                vm.ShowMessageBox -= ShowMessageBox;
                vm.ShowWindow -= ShowWindow;
                vm.ShowCommonDialog -= ShowCommonDialog;
                vm.CloseWindow -= CloseWindow;
                Closing -= (s, e) => e.Cancel = vm.PostProcces();
            }
        }

        protected virtual async Task<MessageBoxResult> ShowMessageBox(string arg1, MessageBoxButton arg2)
        {
            return await Dispatcher.InvokeAsync(() => MessageDialog.Show(Title, arg1, arg2));
        }

        protected virtual async Task ShowWindow(Type windowType, bool isModal, object paramater, Action<object> callback)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                Window window;
                if (casheWindows.ContainsKey(windowType))
                {
                    window = casheWindows[windowType];
                }
                else
                {
                    window = (Window)Activator.CreateInstance(windowType);
                }

                if (window is WindowBase windowBase)
                {
                    if (windowBase.IsCasheWindow)
                    {
                        if (!casheWindows.ContainsKey(windowType))
                        {
                            casheWindows[windowType] = windowBase;
                        }
                    }

                    windowBase.LoadViewModel(paramater, callback);
                }

                window.WindowStartupLocation = WindowStartupLocation.Manual;
                window.Top = Top + (Height / 2) - window.Height / 2;
                window.Left = Left + (Width / 2) - window.Width / 2;

                if (isModal)
                {
                    IsShowDialog = true;
                    window.ShowDialog();
                    IsShowDialog = false;
                }
                else
                {
                    window.Show();
                }
            });
        }

        private async Task<bool> ShowCommonDialog(Type dialogType, Action<CommonDialog> preCallback, Action<CommonDialog> sucessCallback)
        {
            return await Dispatcher.InvokeAsync(() =>
            {
                var dialog = (CommonDialog)Activator.CreateInstance(dialogType);
                preCallback?.Invoke(dialog);

                IsShowDialog = true;
                var result = dialog.ShowDialog().GetValueOrDefault(false);
                IsShowDialog = false;

                if (result)
                {
                    sucessCallback?.Invoke(dialog);
                }

                return result;
            });
        }

        protected virtual bool CloseWindow()
        {
            if (IsCasheWindow)
            {
                Hide();
            }
            else
            {
                Close();
            }

            UnLoadViewModel();

            return true;
        }


        private void WindowBase_Closed(object sender, EventArgs e)
        {
            casheWindows.Remove(GetType());
        }

        protected static void ClearCasheWindow()
        {
            casheWindows.Values.ToList().ForEach((x) =>
            {
                try
                {
                    x.Close();
                }
                catch { }
            });
            casheWindows.Clear();
        }
    }
}
