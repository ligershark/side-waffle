using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplatePack.Tooling {
    public class ActivityLogger {
        // ref at: https://msdn.microsoft.com/en-us/library/bb166359.aspx
        private IVsActivityLog _log;

        public ActivityLogger(IVsActivityLog log) {
            _log = log;
        }

        public void Error(string message) {
            if (_log != null) {
                _log.LogEntry(
                    (UInt32)__ACTIVITYLOG_ENTRYTYPE.ALE_ERROR,
                    this.ToString(),
                    string.Format(CultureInfo.CurrentCulture, "{0}", message));
            }
            else {
                // not sure what else to do here
                Console.WriteLine(string.Format("error: {0}", message));
            }
        }

        public void Info(string message) {
            if (_log != null) {
                _log.LogEntry(
                    (UInt32)__ACTIVITYLOG_ENTRYTYPE.ALE_INFORMATION,
                    this.ToString(),
                    string.Format(CultureInfo.CurrentCulture, "{0}", message));
            }
            else {
                // not sure what else to do here
                Console.WriteLine(string.Format("info: {0}", message));
            }
        }
    }
}
