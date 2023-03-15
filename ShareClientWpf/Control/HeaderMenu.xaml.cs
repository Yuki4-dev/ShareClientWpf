using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShareClientWpf
{
    /// <summary>
    /// HeaderMenu.xaml の相互作用ロジック
    /// </summary>
    public partial class HeaderMenu : UserControl
    {
        public ICommand ProfileCommand
        {
            get => (ICommand)GetValue(ProfileCommandProperty);
            set => SetValue(ProfileCommandProperty, value);
        }
        public static readonly DependencyProperty ProfileCommandProperty =
            DependencyProperty.Register(nameof(ProfileCommand), typeof(ICommand), typeof(HeaderMenu), new PropertyMetadata(null));

        public ICommand SendCommand
        {
            get => (ICommand)GetValue(SendCommandProperty);
            set => SetValue(SendCommandProperty, value);
        }
        public static readonly DependencyProperty SendCommandProperty =
            DependencyProperty.Register(nameof(SendCommand), typeof(ICommand), typeof(HeaderMenu), new PropertyMetadata(null));


        public ICommand ReceiveCommand
        {
            get => (ICommand)GetValue(ReceiveCommandProperty);
            set => SetValue(ReceiveCommandProperty, value);
        }
        public static readonly DependencyProperty ReceiveCommandProperty =
            DependencyProperty.Register(nameof(ReceiveCommand), typeof(ICommand), typeof(HeaderMenu), new PropertyMetadata(null));


        public ICommand MoreCommand
        {
            get => (ICommand)GetValue(MoreCommandProperty);
            set => SetValue(MoreCommandProperty, value);
        }
        public static readonly DependencyProperty MoreCommandProperty =
            DependencyProperty.Register(nameof(MoreCommand), typeof(ICommand), typeof(HeaderMenu), new PropertyMetadata(null));

        public HeaderMenu()
        {
            InitializeComponent();
        }
    }
}
