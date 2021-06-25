using ShareClient.Model;
using System;

namespace ShareClientWpf
{
    public interface ISendDialogService
    {
        public Action<IWindowCaputure, Connection> SendExecute { get; set; }
    }
}
