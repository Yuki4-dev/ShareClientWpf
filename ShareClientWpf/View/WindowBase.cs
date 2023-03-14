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
        private static readonly Dictionary<Type, WindowBase> cashedWindows = new();

        private double ContentTop => WindowState == WindowState.Maximized ? 0 : Top;

        private double ContentLeft => WindowState == WindowState.Maximized ? 0 : Left;

        public bool IsCashedWindow
        {
            get => (bool)GetValue(IsCashedWindowProperty);
            set => SetValue(IsCashedWindowProperty, value);
        }
        public static readonly DependencyProperty IsCashedWindowProperty =
            DependencyProperty.Register(nameof(IsCashedWindow), typeof(bool), typeof(WindowBase), new PropertyMetadata(false));

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

        protected void LoadViewModel(object parameter, Action<object> executeCallback)
        {
            if (DataContext is ViewModelBase vm)
            {
                vm.ShowMessageBox += ShowMessageBox;
                vm.ShowWindow += ShowWindow;
                vm.ShowCommonDialog += ShowCommonDialog;
                vm.CloseWindow += CloseWindow;
                Closing += WindowBase_Closing;

                vm.LoadedProcess(parameter, executeCallback);
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
                Closing -= WindowBase_Closing;
            }
        }

        private void WindowBase_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is ViewModelBase vm)
            {
                e.Cancel = vm.PostProcess();
            }
        }

        protected virtual async Task<MessageBoxResult> ShowMessageBox(string arg1, MessageBoxButton arg2)
        {
            return await Dispatcher.InvokeAsync(() => MessageDialog.Show(Title, arg1, arg2));
        }

        protected virtual async Task ShowWindow(Type windowType, bool isModal, object parameter, Action<object> callback)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                Window window;
                if (cashedWindows.ContainsKey(windowType))
                {
                    window = cashedWindows[windowType];
                }
                else
                {
                    window = (Window)Activator.CreateInstance(windowType);
                }

                if (window is WindowBase windowBase)
                {
                    if (windowBase.IsCashedWindow)
                    {
                        if (!cashedWindows.ContainsKey(windowType))
                        {
                            cashedWindows[windowType] = windowBase;
                        }
                    }

                    windowBase.LoadViewModel(parameter, callback);
                }

                window.WindowStartupLocation = WindowStartupLocation.Manual;
                window.Top = ContentTop + (Height / 2) - window.Height / 2;
                window.Left = ContentLeft + (Width / 2) - window.Width / 2;

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

        private async Task<bool> ShowCommonDialog(Type dialogType, Action<CommonDialog> preCallback, Action<CommonDialog> successCallback)
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
                    successCallback?.Invoke(dialog);
                }

                return result;
            });
        }

        protected virtual bool CloseWindow()
        {
            if (IsCashedWindow)
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
            cashedWindows.Remove(GetType());
        }

        protected static void ClearCashedWindow()
        {
            cashedWindows.Values.ToList().ForEach((x) =>
            {
                try
                {
                    x.Close();
                }
                catch { }
            });
            cashedWindows.Clear();
        }
    }
}
