using Base.Transport.DesignMessages;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace Base.Transport
{
    public class DefaultMessageTypeResolver : IMessageTypeResolver
    {
        private readonly Dictionary<Guid, Type> _guidsToTypes = new Dictionary<Guid, Type>();
        private readonly Dictionary<Type, Guid> _typesToGuids = new Dictionary<Type, Guid>();
        public DefaultMessageTypeResolver(params Assembly[] assemblies)
        {
            foreach (var asm in
                (assemblies ?? Array.Empty<Assembly>()).Concat(new[]
                    {typeof(AvaloniaRemoteMessageGuidAttribute).GetTypeInfo().Assembly}))
            {
                foreach (var t in asm.ExportedTypes)
                {
                    var attr = t.GetTypeInfo().GetCustomAttribute<AvaloniaRemoteMessageGuidAttribute>();
                    if (attr != null)
                    {
                        _guidsToTypes[attr.Guid] = t;
                        _typesToGuids[t] = attr.Guid;
                    }
                }
            }
        }

        public Type GetByGuid(Guid id) => _guidsToTypes[id];
        public Guid GetGuid(Type type) => _typesToGuids[type];
    }
}