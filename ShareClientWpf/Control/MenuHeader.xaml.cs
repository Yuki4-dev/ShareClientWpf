using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShareClientWpf
{
    /// <summary>
    /// ImageShareDisplayHeader.xaml の相互作用ロジック
    /// </summary>
    public partial class MenuHeader : UserControl
    {
        public event EventHandler<MenuHeaderButtonClickEventArgs> MenuButtonClick;

        public bool SendEnable
        {
            get => (bool)GetValue(SendEnableProperty);
            set => SetValue(SendEnableProperty, value);
        }

        public bool ReceiveEnable
        {
            get => (bool)GetValue(ReceiveEnableProperty);
            set => SetValue(ReceiveEnableProperty, value);
        }

        public bool SettingEnable
        {
            get => (bool)GetValue(SettingEnableProperty);
            set => SetValue(SettingEnableProperty, value);
        }

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public double IconWidth
        {
            get => (double)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        public double IconHeight
        {
            get => (double)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        public Brush MouseOverFill
        {
            get => (Brush)GetValue(MouseOverFillProperty);
            set => SetValue(MouseOverFillProperty, value);
        }

        public MenuHeader()
        {
            InitializeComponent();
            ProfileButton.Tag = MenuHeaderButton.Profile;
            SendButton.Tag = MenuHeaderButton.Send;
            ReceiveButton.Tag = MenuHeaderButton.Recieve;
            SettingButton.Tag = MenuHeaderButton.Setting;
        }

        private void MenuHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            var ib = e.Source as IconButton;
            if (ib != null)
            {
                MenuButtonClick?.Invoke(this, new MenuHeaderButtonClickEventArgs((MenuHeaderButton)ib.Tag));
            }
        }

        public static readonly DependencyProperty SendEnableProperty = DependencyProperty.Register(nameof(SendEnable), typeof(bool), typeof(MenuHeader), new PropertyMetadata(true));
        public static readonly DependencyProperty ReceiveEnableProperty = DependencyProperty.Register(nameof(ReceiveEnable), typeof(bool), typeof(MenuHeader), new PropertyMetadata(true));
        public static readonly DependencyProperty SettingEnableProperty = DependencyProperty.Register(nameof(SettingEnable), typeof(bool), typeof(MenuHeader), new PropertyMetadata(true));
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(MenuHeader), new PropertyMetadata(Orientation.Horizontal));
        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(MenuHeader), new PropertyMetadata(48.0));
        public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(MenuHeader), new PropertyMetadata(48.0));
        public static readonly DependencyProperty MouseOverFillProperty = DependencyProperty.Register(nameof(MouseOverFill), typeof(Brush), typeof(MenuHeader), new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));
    }

    public class MenuHeaderButtonClickEventArgs : EventArgs
    {
        public MenuHeaderButton Button { get; }
        public MenuHeaderButtonClickEventArgs(MenuHeaderButton button)
        {
            Button = button;
        }
    }

    public enum MenuHeaderButton
    {
        Profile, Send, Recieve, Setting
    }
}
