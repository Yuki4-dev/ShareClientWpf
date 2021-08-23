using System.Windows;
using System.Windows.Controls;

namespace ShareClientWpf
{
    /// <summary>
    /// ImageShareDisplayDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class MessageDialog : WindowBase
    {
        private readonly MessageBoxResult[] buttonResult;
        private MessageBoxResult result = MessageBoxResult.None;

        private MessageDialog(string title, string message, MessageBoxButton button) : base()
        {
            InitializeComponent();
            Title = title;
            MessageTextBlock.Text = message;

            if (button == MessageBoxButton.OK)
            {
                buttonResult = new MessageBoxResult[] { MessageBoxResult.OK };
            }
            else if (button == MessageBoxButton.OKCancel)
            {
                buttonResult = new MessageBoxResult[] { MessageBoxResult.Cancel, MessageBoxResult.OK };
            }
            else if (button == MessageBoxButton.YesNoCancel)
            {
                buttonResult = new MessageBoxResult[] { MessageBoxResult.Cancel, MessageBoxResult.No, MessageBoxResult.Yes };
            }
            else if (button == MessageBoxButton.YesNo)
            {
                buttonResult = new MessageBoxResult[] { MessageBoxResult.No, MessageBoxResult.Yes };
            }

            var b = new Button[] { PrimaryButton, SecondryButton, ThirdButton };
            for (int i = 0; i < buttonResult.Length; i++)
            {
                b[i].Visibility = Visibility.Visible;
                b[i].Content = buttonResult[i].ToString();
            }
        }

        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button btn)
            {
                result = buttonResult[int.Parse(btn.Tag.ToString())];
                Close();
            }
        }

        public static MessageBoxResult Show(string title, string message, MessageBoxButton button = MessageBoxButton.OK)
        {
            var dialog = new MessageDialog(title, message, button);
            dialog.ShowDialog();
            return dialog.result;
        }
    }
}
