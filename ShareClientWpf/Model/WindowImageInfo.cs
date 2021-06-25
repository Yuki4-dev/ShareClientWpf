using System;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class WindowImageInfo
    {
        public string Title { get; set; }
        public ImageSource WindowImage { get; set; }
        public IntPtr WindowHandle { get; set; }
    }
}
