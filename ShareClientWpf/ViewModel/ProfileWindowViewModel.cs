using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private ICommand selectCommand;
        public ICommand SelectCommand
        {
            get => selectCommand;
            set => SetProperty(ref selectCommand, value);
        }

        public override void LoadedProcces(object paramater, Action<object> executeCallback)
        {
            Profile = (Profile)paramater;
            SelectCommand = new Command(SelectExecute);
        }

        private async void SelectExecute()
        {
            await OnShowCommonDialog(typeof(OpenFileDialog), (dialog) =>
            {
                ((OpenFileDialog)dialog).Title = "プロフィール画像";
                ((OpenFileDialog)dialog).Filter = "Jpeg|*.jpg;*.jpeg;|Png|*.png|Gif|*.gif;*.GIF";
            },
            (dialog) =>
            {
                SetProfileImage(((OpenFileDialog)dialog).FileName);
            });
        }

        private void SetProfileImage(string filename)
        {
            if(filename == null)
            {
                return;
            }

            try
            {
                var image = ImageHelper.ResizeImage(Image.FromFile(filename), 150);
                Profile.IconImage = ImageHelper.Image2Byte(image, ImageFormat.Jpeg);
            }
            catch(Exception ex)
            {
                OnShowMessageBox(ex.Message);
            }
        }

    }
}
