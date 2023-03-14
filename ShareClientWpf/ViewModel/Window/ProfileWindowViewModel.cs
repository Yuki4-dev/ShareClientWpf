using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Input;

namespace ShareClientWpf
{
    public class ProfileWindowViewModel : ViewModelBase
    {
        private Profile profile;
        public Profile Profile
        {
            get => profile;
            set => SetProperty(ref profile, value);
        }

        private string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public ICommand ClearCommand { get; }

        public ICommand SelectCommand { get; }

        public ProfileWindowViewModel()
        {
            SelectCommand = new Command(SelectExecute);
            ClearCommand = new Command(() =>
            {
                if (Profile != null)
                {
                    Profile.IconImage = null;
                }
            });
        }

        public override void LoadedProcess(object parameter, Action<object> executeCallback)
        {
            Profile = (Profile)parameter;
        }

        private async void SelectExecute()
        {
            await OnShowCommonDialog(typeof(OpenFileDialog), (dialog) =>
            {
                ((OpenFileDialog)dialog).Title = "プロフィール画像";
                ((OpenFileDialog)dialog).Filter = "Jpeg|*.jpg;*.jpeg|Png|*.png|Gif|*.gif;*.GIF";
            },
            (dialog) =>
            {
                SetProfileImage(((OpenFileDialog)dialog).FileName);
            });
        }

        private void SetProfileImage(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            try
            {
                var image = ImageHelper.ResizeImage(Image.FromFile(filename), 150);
                Profile.IconImage = ImageHelper.Image2Byte(image, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                OnShowMessageBox(ex.Message);
            }
        }

    }
}
