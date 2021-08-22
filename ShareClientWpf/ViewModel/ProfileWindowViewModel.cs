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

        private void SelectExecute()
        {
            var f = new OpenFileDialog();
            f.ShowDialog();
            var file = f.FileName;
            var image = ImageHelper.ResizeImage(Image.FromFile(file), 150);
            Profile.IconImage = ImageHelper.Image2Byte(image, ImageFormat.Jpeg);
        }

    }
}
