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
        private ICommand profileCommand;
        public ICommand ProfileCommand
        {
            get => profileCommand;
            set => SetProperty(ref profileCommand, value);
        }

        private ICommand sendCommand;
        public ICommand SendCommand
        {
            get => sendCommand;
            set => SetProperty(ref sendCommand, value);
        }

        private ICommand recieveCommand;
        public ICommand RecieveCommand
        {
            get => recieveCommand;
            set => SetProperty(ref recieveCommand, value);
        }

        private ICommand moreCommand;
        public ICommand MoreCommand
        {
            get => moreCommand;
            set => SetProperty(ref moreCommand, value);
        }
    }
}
