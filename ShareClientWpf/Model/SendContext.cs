using System.Net;

namespace ShareClientWpf
{
    public class SendContext : ModelBase
    {
        private IPEndPoint iPEndPoint;
        public IPEndPoint IPEndPoint
        {
            get => iPEndPoint;
            set => SetProperty(ref iPEndPoint, value);
        }

        private WindowInfo windowInfo;
        public WindowInfo WindowInfo
        {
            get => windowInfo;
            set => SetProperty(ref windowInfo, value);
        }
    }
}
