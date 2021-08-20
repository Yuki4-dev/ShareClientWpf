using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
            set
            {
                SetProperty(ref sendDelayText,
                                value,
                                ModelBase.IntValidate<string>((_) => Message = "Portには数字を入れてください。"),
                                () => Message = "");
            }
        }

        private string sendWidthText;
        public string SendWidthText
        {
            get => sendWidthText;
            set
            {
                SetProperty(ref sendWidthText,
                                value,
                                ModelBase.IntValidate<string>((_) => Message = "Portには数字を入れてください。"),
                                () => Message = "");
            }
        }

        private string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public override void LoadedProcces(object paramater, Action<object> executeCallback)
        {
            callback = executeCallback;
            if (paramater is SettingContext context)
            {
                NameText = context.Name;
                SendDelayText = context.SendDelay.ToString();
                SendWidthText = context.SendWidth.ToString();
            }
        }

        protected override void CloseExecute()
        {
            var context = new SettingContext()
            {
                Name = NameText,
                SendDelay = int.Parse(SendDelayText),
                SendWidth = int.Parse(SendWidthText),
            };

            callback?.Invoke(context);
            OnCloseWindow();
        }
    }
}
