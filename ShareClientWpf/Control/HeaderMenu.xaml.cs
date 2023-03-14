using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ShareClientWpf
{
    /// <summary>
    /// HeaderMenu.xaml の相互作用ロジック
    /// </summary>
    public partial class HeaderMenu : UserControl
    {
        public HeaderMenuCommands Commands
        {
            get => (HeaderMenuCommands)GetValue(CommandsProperty);
            set => SetValue(CommandsProperty, value);
        }
        public static readonly DependencyProperty CommandsProperty =
            DependencyProperty.Register(nameof(Commands), typeof(HeaderMenuCommands), typeof(HeaderMenu), new PropertyMetadata(null));

        public HeaderMenu()
        {
            InitializeComponent();
        }
    }

    [ObservableObject]
    public partial class HeaderMenuCommands
    {
        [ObservableProperty]
        private Command profileCommand;

        [ObservableProperty]
        private Command sendCommand;

        [ObservableProperty]
        private Command receiveCommand;

        [ObservableProperty]
        private Command moreCommand;
    }
}
