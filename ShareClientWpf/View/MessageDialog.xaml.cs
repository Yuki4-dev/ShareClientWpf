using System.Windows;
using System.Windows.Controls;

namespace ShareClientWpf
{
    /// <summary>
    /// ImageShareDisplayDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class MessageDialog : WindowBase
    {
        private readonly MessageBoxResult[] _ButtonResult;
        private MessageBoxResult _Result = MessageBoxResult.None;

        private MessageDialog(string title, string message, MessageBoxButton button) : base()
        {
            InitializeComponent();

            TitleTextBlock.Text = title;
            MessageTextBlock.Text = message;

            if (button == MessageBoxButton.OK)
            {
                _ButtonResult = new MessageBoxResult[] { MessageBoxResult.OK };
            }
            else if (button == MessageBoxButton.OKCancel)
            {
                _ButtonResult = new MessageBoxResult[] { MessageBoxResult.Cancel, MessageBoxResult.OK };
            }
            else if (button == MessageBoxButton.YesNoCancel)
            {
                _ButtonResult = new MessageBoxResult[] { MessageBoxResult.Cancel, MessageBoxResult.No, MessageBoxResult.Yes };
            }
            else if (button == MessageBoxButton.YesNo)
            {
                _ButtonResult = new MessageBoxResult[] { MessageBoxResult.No, MessageBoxResult.Yes };
            }

            var b = new Button[] { PrimaryButton, SecondryButton, ThirdButton };
            for (int i = 0; i < _ButtonResult.Length; i++)
            {
                b[i].Visibility = Visibility.Visible;
                b[i].Content = _ButtonResult[i].ToString();
            }
        }

        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button btn)
            {
                _Result = _ButtonResult[int.Parse(btn.Tag.ToString())];
                Close();
            }
        }

        public static MessageBoxResult Show(string title, string message, MessageBoxButton button = MessageBoxButton.OK)
        {
            var dialog = new MessageDialog(title, message, button);
            dialog.ShowDialog();
            return dialog._Result;
        }
    }
}
