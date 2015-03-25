
namespace LigerShark.Templates {
    using System;
    using System.IO;
    using Microsoft.Build.Framework;
    using System.Text;

    public class InmemoryMsbuildLogger : ILogger {
        #region Non-public properties
        protected StringBuilder writer;
        #endregion

        #region ILogger Members

        public void Initialize(IEventSource eventSource) {
            //initialize the writer
            writer = new StringBuilder();

            //this write must be closed in the Shutdown() method

            //register to the events you are interested in here
            eventSource.AnyEventRaised +=
                new AnyEventHandler(AnyEventRaised);
            eventSource.BuildStarted +=
                new BuildStartedEventHandler(BuildStarted);
            eventSource.BuildFinished +=
                new BuildFinishedEventHandler(BuildFinished);
            eventSource.CustomEventRaised +=
                new CustomBuildEventHandler(CustomEvent);
            eventSource.ErrorRaised +=
                new BuildErrorEventHandler(ErrorRaised);
            eventSource.MessageRaised +=
                new BuildMessageEventHandler(MessageRaised);
            eventSource.ProjectStarted +=
                new ProjectStartedEventHandler(ProjectStarted);
            eventSource.ProjectStarted +=
                new ProjectStartedEventHandler(ProjectFinished);
            eventSource.StatusEventRaised +=
                new BuildStatusEventHandler(StatusEvent);
            eventSource.TargetStarted +=
                new TargetStartedEventHandler(TargetStarted);
            eventSource.TargetFinished +=
                new TargetFinishedEventHandler(TargetFinished);
            eventSource.TaskStarted +=
                new TaskStartedEventHandler(TaskStarted);
            eventSource.TaskFinished +=
                new TaskFinishedEventHandler(TaskFinished);
            eventSource.WarningRaised +=
                new BuildWarningEventHandler(WarningRaised);
        }

        #region Build event handlers
        void WarningRaised(object sender, BuildWarningEventArgs e) { writer.AppendLine(GetLogMessage("WarningRaised", e)); }
        void TaskFinished(object sender, TaskFinishedEventArgs e) { writer.AppendLine(GetLogMessage("TaskFinished", e)); }
        void TaskStarted(object sender, TaskStartedEventArgs e) { writer.AppendLine(GetLogMessage("TaskStarted", e)); }
        void TargetFinished(object sender, TargetFinishedEventArgs e) { writer.AppendLine(GetLogMessage("TargetFinished", e)); }
        void TargetStarted(object sender, TargetStartedEventArgs e) { writer.AppendLine(GetLogMessage("TargetStarted", e)); }
        void ProjectFinished(object sender, ProjectStartedEventArgs e) { writer.AppendLine(GetLogMessage("ProjectFinished", e)); }
        void ProjectStarted(object sender, ProjectStartedEventArgs e) { writer.AppendLine(GetLogMessage("ProjectStarted", e)); }
        void MessageRaised(object sender, BuildMessageEventArgs e) { writer.AppendLine(GetLogMessage("MessageRaised", e)); }
        void ErrorRaised(object sender, BuildErrorEventArgs e) { writer.AppendLine(GetLogMessage("ErrorRaised", e)); }
        void CustomEvent(object sender, CustomBuildEventArgs e) { writer.AppendLine(GetLogMessage("CustomEvent", e)); }
        void BuildFinished(object sender, BuildFinishedEventArgs e) { writer.AppendLine(GetLogMessage("BuildFinished", e)); }
        void StatusEvent(object sender, BuildStatusEventArgs e) { writer.AppendLine(GetLogMessage("StatusEvent", e)); }
        void AnyEventRaised(object sender, BuildEventArgs e) { writer.AppendLine(GetLogMessage("AnyEventRaised", e)); }


        void BuildStarted(object sender, BuildStartedEventArgs e) { writer.AppendLine(GetLogMessage("BuildStarted", e)); }
        #endregion

        /// <summary>
        /// This is set by the MSBuild engine
        /// </summary>
        public string Parameters { get; set; }
        
        public LoggerVerbosity Verbosity { get; set; }

        protected string GetLogMessage(string eventName, BuildEventArgs e) {
            if (string.IsNullOrEmpty(eventName)) { throw new ArgumentNullException("eventName"); }

            ////e.SenderName is typically set to MSBuild when called through msbuild.exe
            string eMessage = string.Format("{0}\t{1}\t{2}",
                        eventName,
                        FormatString(e.Message),
                        FormatString(e.HelpKeyword)
                        );
            return eMessage;
        }
        protected string FormatString(string str) {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(str)) {
                result = str.Replace("\t", "    ")
                    .Replace("\r\n", "\r\n\t\t\t\t");
            }
            return result;
        }
        public void Shutdown() {
            throw new NotImplementedException();
        }
        #endregion

        public string GetLog() {
            string result = null;

            if (writer != null) {
                result = writer.ToString();
            }

            return result;
            
        }
    }
}
