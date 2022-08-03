using EnvDTE;
using Task = System.Threading.Tasks.Task;
using Base.Utils;

using TargetEditor = Base.EditorFactories.XAML.XAML;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using System.Threading;
using System;
using Serilog;
using Serilog.Core;
using Base.EditorFactories.XAML;

namespace VS_Extension___CSharp_Editor_Designer_Base
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>

    [Guid(
        guid: PackageGuidString
    )]

    [PackageRegistration(
        UseManagedResourcesOnly = true,
        AllowsBackgroundLoading = true
    )]

    [ProvideAutoLoad(
        cmdUiContextGuid: VSConstants.UICONTEXT.SolutionExistsAndFullyLoaded_string,
        flags: PackageAutoLoadFlags.BackgroundLoad
    )]

    [ProvideEditorExtension(
        factoryType: typeof(TargetEditor),
        extension: ".ed",
        priority: 100,
        NameResourceID = 113,
        EditorFactoryNotify = true,
        ProjectGuid = VSConstants.UICONTEXT.CSharpProject_string,
        DefaultName = Name
    )]

    [ProvideEditorFactory(
        factoryType: typeof(TargetEditor),
        nameResourceID: 113,
        TrustLevel = __VSEDITORTRUSTLEVEL.ETL_AlwaysTrusted
    )]

    [ProvideEditorLogicalView(
        factoryType: typeof(TargetEditor),
        logicalViewGuid: LogicalViewID.Designer
    )]
    [ProvideOptionPage(typeof(OptionsDialogPage), Name, "General", 113, 0, supportsAutomation: true)]
    public sealed class VS_Extension___CSharp_Editor_Designer_BasePackage : AsyncPackage
    {
        /// <summary>
        /// VS_Extension___CSharp_Editor_Designer_BasePackage GUID string.
        /// </summary>
        public const string PackageGuidString = "ab447ce9-39c4-4e64-beed-7b845b5f35e3";
        public const string Name = "C# Editor And Designer Base Package";

        public static Base.SolutionService SolutionService { get; private set; }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(
            CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            InitializeLogging();
            RegisterEditorFactory(new TargetEditor(this));

            var dte = (DTE)await GetServiceAsync(typeof(DTE));
            SolutionService = new Base.SolutionService(dte);
        }

        private void InitializeLogging()
        {
            const string format = "{Timestamp:HH:mm:ss.fff} [{Level}] {Pid} {Message}{NewLine}{Exception}";
            IVsOutputWindow output = (IVsOutputWindow)GetService(typeof(SVsOutputWindow));
            var settings = this.GetMefService<IAvaloniaVSSettings>();
            var levelSwitch = new LoggingLevelSwitch() { MinimumLevel = settings.MinimumLogVerbosity };

            var sink = new OutputPaneEventSink(output, outputTemplate: format, "C# Editor And Designer - Diagnostics");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.Sink(sink, levelSwitch: levelSwitch)
                .CreateLogger();
        }

        #endregion
    }
}
