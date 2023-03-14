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

    public class HeaderMenuCommands : ModelBase
    {
        private Command profileCommand;
        public Command ProfileCommand
        {
            get => profileCommand;
            set => SetProperty(ref profileCommand, value);
        }

        private Command sendCommand;
        public Command SendCommand
        {
            get => sendCommand;
            set => SetProperty(ref sendCommand, value);
        }

        private Command receiveCommand;
        public Command ReceiveCommand
        {
            get => receiveCommand;
            set => SetProperty(ref receiveCommand, value);
        }

        private Command moreCommand;
        public Command MoreCommand
        {
            get => moreCommand;
            set => SetProperty(ref moreCommand, value);
        }
    }
}
