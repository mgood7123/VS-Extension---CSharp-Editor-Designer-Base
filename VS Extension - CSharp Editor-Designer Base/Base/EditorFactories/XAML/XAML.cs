using EnvDTE;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using VS_Extension___CSharp_Editor_Designer_Base;

namespace Base.EditorFactories.XAML
{
    class XAML : EditorFactory
    {
        public XAML(VS_Extension___CSharp_Editor_Designer_BasePackage package) : base(package)
        {
        }

        internal class XamlBufferMetadata
        {
            public Metadata CompletionMetadata { get; set; }

            public bool NeedInvalidation { get; set; } = true;
        }

        protected override Func<object> GetBufferMetadataCreator()
        {
            return () => new XamlBufferMetadata();
        }

        private static readonly Guid XmlLanguageServiceGuid = new Guid("f6819a78-a205-47b5-be1c-675b3c7f0b8e");

        protected override Guid SetLanguageServiceID()
        {
            return XmlLanguageServiceGuid;
        }

        protected override EditorHostPane CreateDesignerPane(Project project, string pszMkDocument, IVsCodeWindow editorWindow, IWpfTextViewHost editorControl)
        {
            return new DesignerPane(project, pszMkDocument, editorWindow, editorControl);
        }
    }
}