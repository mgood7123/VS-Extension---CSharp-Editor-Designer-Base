using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Formatting;
using System.IO;
using System;

namespace VS_Extension___CSharp_Editor_Designer_Base
{
    /// <summary>
    /// A serilog sink that outputs to the VS output window.
    /// </summary>
    internal class OutputPaneEventSink : ILogEventSink
    {
        private readonly IVsOutputWindowPane _pane;
        private readonly ITextFormatter _formatter;

        IVsOutputWindowPane CreatePane(IVsOutputWindow output, Guid paneGuid, string title,
        bool visible, bool clearWithSolution)
        {
            IVsOutputWindowPane pane;

            // Create a new pane.
            int r = output.CreatePane(
                ref paneGuid,
                title,
                Convert.ToInt32(visible),
                Convert.ToInt32(clearWithSolution));

            // Retrieve the new pane.
            output.GetPane(ref paneGuid, out pane);

            pane.OutputString("This is the Created Pane\n");
            pane.OutputString("CreatePane returned: " + r + "\n");
            return pane;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="OutputPaneEventSink"/> class.
        /// </summary>
        /// <param name="output">The VS output window.</param>
        /// <param name="outputTemplate">The serilog output template.</param>
        public OutputPaneEventSink(
            IVsOutputWindow output,
            string outputTemplate, string name)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _formatter = new MessageTemplateTextFormatter(outputTemplate, null);

            _pane = CreatePane(output, Base.Guids.outputPane, name, true, true);
        }

#pragma warning disable VSTHRD010
        /// <inheritdoc/>
        public void Emit(LogEvent logEvent)
        {
            var sw = new StringWriter();
            _formatter.Format(logEvent, sw);
            var message = sw.ToString();

            if (_pane is IVsOutputWindowPaneNoPump noPump)
            {
                noPump.OutputStringNoPump(message);
            }
            else
            {
                ErrorHandler.ThrowOnFailure(_pane.OutputStringThreadSafe(message));
            }

            if (logEvent.Level == LogEventLevel.Error)
            {
                _pane.Activate();
            }
        }
#pragma warning restore VSTHRD010
    }
}