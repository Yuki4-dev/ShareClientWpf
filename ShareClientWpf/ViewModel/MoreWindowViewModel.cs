using System;
using System.Drawing.Imaging;

namespace ShareClientWpf
{
    public class MoreWindowViewModel : ViewModelBase
    {
        private Action<object> callback;
        private readonly ImageFormat[] formats = new ImageFormat[] { ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif };

        private int selectIndex;
        public int SelectIndex
        {
            get => selectIndex;
            set => SetProperty(ref selectIndex, value);
        }

        private string sendDelayText;
        public string SendDelayText
        {
            get => sendDelayText;
            set => SetProperty(ref sendDelayText,
                                value,
                                IntValidate<string>((_) => Message = "送信間隔には数字を入れてください。"),
                                () => Message = "");
        }

        private string sendWidthText;
        public string SendWidthText
        {
            get => sendWidthText;
            set => SetProperty(ref sendWidthText,
                                value,
                                IntValidate<string>((_) => Message = "画面幅には数字を入れてください。"),
                                () => Message = "");
        }

        private string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public override void LoadedProcess(object paramater, Action<object> executeCallback)
        {
            callback = executeCallback;
            var context = (SettingContext)paramater;

            SendDelayText = context.SendDelay.ToString();
            SendWidthText = context.SendWidth.ToString();
            SelectIndex = context.Format != null ? Array.IndexOf(formats, context.Format) : 0;
        }

        protected override void CloseExecute(object paramater)
        {
            if (paramater.ToString() == "1")
            {
                var context = new SettingContext()
                {
                    SendDelay = int.Parse(SendDelayText),
                    SendWidth = int.Parse(SendWidthText),
                    Format = formats[SelectIndex]
                };
                callback.Invoke(context);
            }

            OnCloseWindow();
        }
    }
}
