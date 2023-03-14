using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Drawing.Imaging;

namespace ShareClientWpf
{
    [ObservableObject]
    public partial class MoreWindowViewModel : ViewModelBase
    {
        private Action<object> callback;
        private readonly ImageFormat[] formats = new ImageFormat[] { ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif };

        [ObservableProperty]
        private int selectIndex;

        [ObservableProperty]
        private string message;

        private string sendDelayText;
        public string SendDelayText
        {
            get => sendDelayText;
            set
            {
                if (int.TryParse(value, out var _))
                {
                    Message = "";
                    SetProperty(ref sendDelayText, value);
                }
                else
                {
                    Message = "送信間隔には数字を入れてください。";
                }
            }
        }

        private string sendWidthText;
        public string SendWidthText
        {
            get => sendWidthText;
            set
            {
                if (int.TryParse(value, out var _))
                {
                    Message = "";
                    SetProperty(ref sendWidthText, value);
                }
                else
                {
                    Message = "画面幅には数字を入れてください。";
                }
            }
        }

        public override void LoadedProcess(object parameter, Action<object> executeCallback)
        {
            callback = executeCallback;
            var context = (SettingContext)parameter;

            SendDelayText = context.SendDelay.ToString();
            SendWidthText = context.SendWidth.ToString();
            SelectIndex = context.Format != null ? Array.IndexOf(formats, context.Format) : 0;
        }

        protected override void CloseExecute(object parameter)
        {
            if (parameter.ToString() == "1")
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
