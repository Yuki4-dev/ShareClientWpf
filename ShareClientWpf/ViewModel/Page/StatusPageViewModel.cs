﻿using System.Windows.Input;

namespace ShareClientWpf
{
    public class StatusPageViewModel : ModelBase
    {
        private readonly SendStatusPageViewModel sendStatusPageViewModel = new SendStatusPageViewModel();

        private readonly ReceiveStatusPageViewModel receiveStatusPageViewModel = new ReceiveStatusPageViewModel();

        private readonly ModelBase[] pages;

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
            pages = new ModelBase[] { sendStatusPageViewModel, receiveStatusPageViewModel };

            ContentPage = sendStatusPageViewModel;
        }

        private void SelectExecute(object parameter)
        {
            var index = int.Parse(parameter.ToString());
            ContentPage = pages[index];
        }
    }
}
