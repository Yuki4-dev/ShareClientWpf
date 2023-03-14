using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Input;

namespace ShareClientWpf
{
    [ObservableObject]
    public partial class ProfileWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Profile profile;

        [ObservableProperty]
        private string message;

        public ICommand ClearCommand { get; }

        public ICommand SelectCommand { get; }

        public ProfileWindowViewModel()
        {
            SelectCommand = new Command(SelectExecuteAsync);
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

        private async void SelectExecuteAsync()
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
