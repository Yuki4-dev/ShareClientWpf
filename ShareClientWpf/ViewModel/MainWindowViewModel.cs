using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Visibility _SendDialogVisibility = Visibility.Collapsed;
        public Visibility SendDialogVisibility
        {
            get => _SendDialogVisibility;
            set => OnPropertyChanged(ref _SendDialogVisibility, value, nameof(SendDialogVisibility));
        }

        public MainWindowViewModel()
        {
        }

        public void MainWindow_MenuButtonClick(MenuHeaderButtonClickEventArgs e)
        {
            if (e.Button == MenuHeaderButton.Send)
            {
                SendDialogVisibility = Visibility.Visible;
            }
        }

        public override bool PostProcces()
        {
            var args = new ShowMessageBoxEventArgs("終了しますか？");
            args.Button = MessageBoxButton.YesNo;
            OnShowMessageBox(args);
            return args.Result != MessageBoxResult.Yes;
        }
    }
}
