using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ShareClientWpf
{
    /// <summary>
    /// ImageShareDisplayDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class MessageDialog : WindowBase
    {
        private static readonly IDictionary<MessageBoxButton, MessageBoxResult[]> results = new Dictionary<MessageBoxButton, MessageBoxResult[]>()
        {
            {MessageBoxButton.OK,  new MessageBoxResult[] { MessageBoxResult.OK }},
            {MessageBoxButton.OKCancel,  new MessageBoxResult[] { MessageBoxResult.Cancel, MessageBoxResult.OK }},
            {MessageBoxButton.YesNoCancel,  new MessageBoxResult[] { MessageBoxResult.Cancel, MessageBoxResult.No, MessageBoxResult.Yes }},
            {MessageBoxButton.YesNo, new MessageBoxResult[] { MessageBoxResult.No, MessageBoxResult.Yes }}
        };

        private readonly MessageBoxButton button;
        private MessageBoxResult result = MessageBoxResult.None;

        private MessageDialog(string title, string message, MessageBoxButton button) : base()
        {
            InitializeComponent();
            Title = title;
            MessageTextBlock.Text = message;
            this.button = button;

            var msgButtons = new Button[] { PrimaryButton, SecondryButton, ThirdButton };
            var res = results[button];
            for (int i = 0; i < res.Length; i++)
            {
                msgButtons[i].Visibility = Visibility.Visible;
                msgButtons[i].Content = res[i].ToString();
            }
        }

        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button clickButton)
            {
                result = results[button][int.Parse(clickButton.Tag.ToString())];
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
