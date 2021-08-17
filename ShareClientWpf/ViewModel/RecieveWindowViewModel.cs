using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShareClientWpf
{
    public class RecieveWindowViewModel : ViewModelBase
    {
        private string portText;
        public string PortText
        {
            get => portText;
            set => SetProperty(ref portText, value);
        }

        private ICommand recieveCommand;
        public ICommand RecieveCommand
        {
            get => recieveCommand;
            set => SetProperty(ref recieveCommand, value);
        }

        public Action<object> Execute { get; set; }

        public RecieveWindowViewModel()
        {
            RecieveCommand = new Command(RecieveExecute);
        }

        public void RecieveExecute()
        {
            Execute?.Invoke(PortText);
            OnCloseWindow();
        }
    }
}
