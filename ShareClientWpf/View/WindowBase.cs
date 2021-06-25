using System;
using System.Windows;
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

        private void PreViewModel(ViewModelBase viewModel)
        {
            viewModel.ShowMessageBox += ShowMessageBox;

            IsVisibleChanged += (s, e) =>
             {
                 if ((bool)e.NewValue)
                 {
                     viewModel.DisplayProcces();
                 }
             };

            Closing += (s, e) =>
            {
                e.Cancel = viewModel.PostProcces();
            };
        }

        protected virtual void ShowMessageBox(object sender, ShowMessageBoxEventArgs e)
        {
            e.Result = MessageDialog.Show(Title, e.Message, e.Button);
        }

        public static readonly DependencyProperty ThemeBrushProperty = DependencyProperty.Register(nameof(ThemeBrush), typeof(Brush), typeof(WindowBase), new PropertyMetadata(new SolidColorBrush(Colors.Red)));
    }
}
