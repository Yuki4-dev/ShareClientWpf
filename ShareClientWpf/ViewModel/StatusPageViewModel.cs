
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace ShareClientWpf
{
    public class StatusPageViewModel : ModelBase
    {
        private SendStatusPageViewModel sendStatusPageViewModel = new SendStatusPageViewModel();

        private ReceiveStatusPageViewModel receiveStatusPageViewModel = new ReceiveStatusPageViewModel();

        private ModelBase contentPage;
        public ModelBase ContentPage
        {
            get => contentPage;
            set => SetProperty(ref contentPage, value);
        }

        private ICommand selectedCommand;
        public ICommand SelectedCommand
        {
            get => selectedCommand;
            set => SetProperty(ref selectedCommand, value);
        }

        public StatusPageViewModel()
        {
            SelectedCommand = new Command(SelectExecute);
            ContentPage = sendStatusPageViewModel;
        }

        private void SelectExecute(object parameter)
        {
            ContentPage = parameter.ToString().Equals("1") ? sendStatusPageViewModel : receiveStatusPageViewModel;
        }
    }

    public class StatusPageCheckConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pageType = (string)parameter == "Send" ? typeof(SendStatusPageViewModel) 
                : (string)parameter == "Receive" ? typeof(ReceiveStatusPageViewModel) : null;
            return pageType != null && value != null && pageType.Equals(value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class SendStatusPageViewModel : ModelBase
    {

    }

    public class ReceiveStatusPageViewModel : ModelBase
    {

    }
}
