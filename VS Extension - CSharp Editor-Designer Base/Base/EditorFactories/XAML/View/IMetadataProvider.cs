using System.Collections.Generic;
using System;

namespace Base.EditorFactories.XAML.View
{
    public interface IMetadataProvider
    {
        IMetadataReaderSession GetMetadata(IEnumerable<string> paths);
    }

    public interface IMetadataReaderSession : IDisposable
    {
        IEnumerable<IAssemblyInformation> Assemblies { get; }
    }
}