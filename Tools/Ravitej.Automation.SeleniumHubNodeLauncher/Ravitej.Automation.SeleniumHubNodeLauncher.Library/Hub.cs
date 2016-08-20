using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library.CommandLine;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library.Data;
using Type = Ravitej.Automation.SeleniumHubNodeLauncher.Library.Data.Type;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.Library
{
    public class Hub
    {
        private readonly LauncherOptions _options;
        private readonly IProgress<string> _progress;
        private readonly string _additionalParams = string.Empty;
        private Process _gridConsoleProcess;
        private Process _hubProcess;
        private readonly LauncherDataProvider _launcherDataProvider;
        private string _hubLog;

        public Hub(LauncherOptions options, IProgress<string> progress)
        {
            _additionalParams = ProcessAdditionalParams(options.GetAdditionalHubParams());
            _options = options;
            _progress = progress;
            _launcherDataProvider = new LauncherDataProvider();
            _hubLog = string.Format("\"{0}\\hub_{1}.log\"", _options.LogsLocation, DateTime.Now.ToString("yyyyMMdd-HHmmss"));
        }

        public int Start()
        {
            var port = default(int);
            try
            {
                port = _launcherDataProvider.GetFirstAvailablePort(Type.Hub);
                if (port == default(int))
                {
                    _progress.Report("Could not start hub due to non-availability of free ports. Please try again later when at least one instance is shut down.");
                    return port;
                }
                var hubCommand = _GetDefaultHubCommand(port.ToString(CultureInfo.InvariantCulture));

                _hubProcess = ProcessHelper.StartProcess(String.Format(@"{0}\java.exe", SysOperations.GetJavaPath(_progress)), _progress, hubCommand, false);
                Thread.Sleep(1000);
                if (_hubProcess != null && _hubProcess.HasExited == false)
                {
                    _progress.Report(string.Concat(
                        "Hub started. Please check the opened command window and/or browser window to ensure it started successfully. For logs look in C:\\Selenium\\_Logs folder.",
                        Environment.NewLine));
                    //remove double quotes from _hubLog before storing
                    _launcherDataProvider.AddProcess(Type.Hub, _hubLog.Trim('\"'), _hubProcess.Id, port);
                    return port;
                }
                _progress.Report(
                    string.Concat(
                        "There was a problem starting the hub. Please check that all required files exist in C:\\Selenium folder, expecially 'selenium-server-standalone.jar'",
                        Environment.NewLine));
            }
            catch (Exception e)
            {
                _progress.Report(string.Format("Could not start hub. {0}{1}", e, Environment.NewLine));
            }
            return port;
        }

        public void ShowGridConsole(int hubPort)
        {
            _gridConsoleProcess = ProcessHelper.StartProcess(String.Format("http://{0}:{1}/grid/console", Environment.MachineName, hubPort.ToString(CultureInfo.InvariantCulture)), _progress, null, false);
        }

        public void ShutDown()
        {
            Helpers.CloseGridConsole(_gridConsoleProcess);
            _gridConsoleProcess = null;
            if (_hubProcess != null && !_hubProcess.HasExited)
            {
                var processId = _hubProcess.Id;
                _hubProcess.Kill();
                _hubProcess = null;
                _launcherDataProvider.RemoveProcess(processId);
            }
        }

        private string _GetDefaultHubCommand(string hubPort)
        {
            return string.Format(
                @"-Xmx1024m -jar C:\Selenium\ServerExecutable\selenium-server-standalone.jar -role hub -port {2} -trustAllSSLCertificates true -log {1} {0}",
                _additionalParams, _hubLog, hubPort).Trim();
        }

        private static string ProcessAdditionalParams(Dictionary<string, string> additionalParams)
        {
            var processedParams = new StringBuilder();
            if (additionalParams != null)
            {
                foreach (var additionalParam in additionalParams)
                {
                    processedParams.Append(string.Format("-{0} {1} ", additionalParam.Key, additionalParam.Value));
                }
                //processedParams = String.Join(" ", additionalParams);
                //processedParams = processedParams.Replace("/", "-");
            }
            return processedParams.ToString();
        }
    }
}
