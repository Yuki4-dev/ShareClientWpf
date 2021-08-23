using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ShareClientWpf
{
    public class SettingContext
    {
        public int SendDelay { get; set; }
        public int SendWidth { get; set; }
        public ImageFormat Format { get; set; }
    }
}
