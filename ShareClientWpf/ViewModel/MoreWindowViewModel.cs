using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Windows.Controls;

namespace ShareClientWpf
{
    public class MoreWindowViewModel : ViewModelBase
    {
        private Action<object> callback;

        private ImageFormat[] formats = new ImageFormat[] { ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif };

        private int selectIndex = 0;
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
            if (paramater is SettingContext context)
            {
                SendDelayText = context.SendDelay.ToString();
                SendWidthText = context.SendWidth.ToString();
                SelectIndex = Array.IndexOf(formats, context.Format);
            }
        }

        protected override void CloseExecute(object paramater)
        {
            if (paramater?.ToString().Equals("1") ?? false)
            {
                var context = new SettingContext()
                {
                    SendDelay = int.Parse(SendDelayText),
                    SendWidth = int.Parse(SendWidthText),
                    Format = formats[SelectIndex]
                };
                callback?.Invoke(context);
            }

            OnCloseWindow();
        }
    }
}
