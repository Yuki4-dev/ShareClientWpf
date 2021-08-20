using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareClientWpf
{
    public class MoreWindowViewModel : ViewModelBase
    {
        private Action<object> callback;

        private string nameText;
        public string NameText
        {
            get => nameText;
            set => SetProperty(ref nameText, value);
        }

        private string sendDelayText;
        public string SendDelayText
        {
            get => sendDelayText;
            set => SetProperty(ref sendDelayText, value);
        }

        private string sendWidthText;
        public string SendWidthText
        {
            get => sendWidthText;
            set => SetProperty(ref sendWidthText, value);
        }

        public override void LoadedProcces(object paramater, Action<object> executeCallback)
        {
            callback = executeCallback;
            if (paramater is SettingContext context)
            {
                NameText = context.Name;
                SendDelayText = context.SendDelay;
                SendWidthText = context.SendWidth;
            }
        }

        protected override void CloseExecute()
        {
            var context = new SettingContext()
            {
                Name = NameText,
                SendDelay = SendDelayText,
                SendWidth = SendWidthText
            };

            callback?.Invoke(context);
            OnCloseWindow();
        }
    }
}
