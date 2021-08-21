using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShareClientWpf
{
    public class Profile : ModelBase
    {
        private Geometry iconData;
        public Geometry IconData
        {
            get => iconData;
            set => SetProperty(ref iconData, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
    }
}
