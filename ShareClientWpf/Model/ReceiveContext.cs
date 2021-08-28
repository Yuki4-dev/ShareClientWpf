using System.Net;

namespace ShareClientWpf
{
    public class ReceiveContext : ModelBase
    {
        private IPEndPoint iPEndPoint;
        public IPEndPoint IPEndPoint
        {
            get => iPEndPoint;
            set => SetProperty(ref iPEndPoint, value);
        }

        private Profile profile;
        public Profile Profile
        {
            get => profile;
            set => SetProperty(ref profile, value);
        }
    }
}
