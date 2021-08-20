using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareClientWpf
{
     public class SettingContext
    {
        public string Name {  get; set; }
        public int SendDelay {  get; set; }
        public int SendWidth { get; set; }
        public ImageFormat Format {  get; set; }
    }
}
