using System;

namespace Base.Transport
{
    public interface IMessageTypeResolver
    {
        Type GetByGuid(Guid id);
        Guid GetGuid(Type type);
    }
}