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
        private ImageSource source;
        public ImageSource Source
        {
            get => source;
            set => SetProperty(ref source, value);
        }

        public MainWindowViewModel()
        {
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
