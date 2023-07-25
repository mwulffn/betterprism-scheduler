using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Odk.BluePrism
{
    public static class AutomateC
    {
        private static string _automateCPath = "C:\\Program Files\\Blue Prism Limited\\Blue Prism Automate\\automatec.exe"; // /sso
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static Guid? LaunchBPASession(string processName, string resourceName, string parameters)
        {
            parameters = parameters.Replace('\"', '\'');
            var cmdline = $"/sso /resource \"{resourceName}\" /run \"{processName}\" /startp \"{parameters}\"";
            var output = RunAutomateC(cmdline);
            var match = Regex.Match(output, "Session:\\s*([a-f0-9\\-]+)");

            if (!match.Success)
            {
                Logger.Error(string.Format("Error on running AutomateC:\r\nCmd: {0}\r\nResult:\r\n{1}", cmdline, output));
                return null;
            }

            return Guid.Parse(match.Groups[1].Value);
        }

        public static string GetSessionStatus(Guid sessionId)
        {
            var cmdline = $"/sso /status {sessionId}";
            var output = RunAutomateC(cmdline);
            var match = Regex.Match(output, "Status:(.+)");

            if (!match.Success) {
                Logger.Error(string.Format("Error on status AutomateC:\r\nCmd: {0}\r\nResult:\r\n{1}", cmdline, output));
                return null;
            }
            
            return match.Groups[1].Value.Trim();
        }

        public static string RequestStop(Guid sessionId)
        {
            var cmdline = $"/sso /requeststop {sessionId}";
            var output = RunAutomateC(cmdline);
            var match = Regex.Match(output, "Status:(.+)");

            if (!match.Success)
            {
                Logger.Error(string.Format("Error on requeststop AutomateC:\r\nCmd: {0}\r\nResult:\r\n{1}", cmdline, output));
                return null;
            }

            return match.Groups[1].Value.Trim();
            /* Stop requested for session: 34506*/
        }

        private static string RunAutomateC(string parameters)
        {
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = _automateCPath;
            p.StartInfo.Arguments = parameters;
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return output;
        }
    }
}