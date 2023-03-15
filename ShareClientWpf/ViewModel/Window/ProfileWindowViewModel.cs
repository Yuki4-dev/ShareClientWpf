using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ShareClientWpf
{
    [ObservableObject]
    public partial class ProfileWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Profile profile;

        [ObservableProperty]
        private string message;

        public override void Loaded(object parameter, Action<object> executeCallback)
        {
            Profile = (Profile)parameter;
        }

        [RelayCommand]
        private async void Select()
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

        [RelayCommand]
        private void Clear()
        {
            if (Profile != null)
            {
                Profile.IconImage = null;
            }
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
