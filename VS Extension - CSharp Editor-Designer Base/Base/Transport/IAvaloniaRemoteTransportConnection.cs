using System;
using System.Threading.Tasks;

namespace Base.Transport
{
    public interface IAvaloniaRemoteTransportConnection : IDisposable
    {
        Task Send(object data);
        event Action<IAvaloniaRemoteTransportConnection, object> OnMessage;
        event Action<IAvaloniaRemoteTransportConnection, Exception> OnException;
    }
}