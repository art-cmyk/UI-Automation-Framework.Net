using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library.CommandLine;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library.Data;
using Type = Ravitej.Automation.SeleniumHubNodeLauncher.Library.Data.Type;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.Library
{
    public class Node
    {
        private readonly LauncherOptions _nodeOptions;
        private readonly IProgress<string> _progress;
        private readonly int _hubPort;
        private Process _gridConsoleProcess;
        private Process _nodeProcess;
        private readonly LauncherDataProvider _launcherDataProvider;
        private readonly string _nodeLog;


        public Node(LauncherOptions nodeOptions, IProgress<string> progress, int hubPort)
        {
            _nodeOptions = nodeOptions;
            _progress = progress;
            _hubPort = hubPort;
            _launcherDataProvider = new LauncherDataProvider();
            _nodeLog = string.Format("\"{0}\\node_{1}.log\"", _nodeOptions.LogsLocation, DateTime.Now.ToString("yyyyMMdd-Hhmmss"));
        }

        public void Start()
        {
            try
            {
                var nodePort = _launcherDataProvider.GetFirstAvailablePort(Type.Node);
                if (nodePort == default(int))
                {
                    _progress.Report("Could not start node due to non-availability of free ports. Please try again later when at least one instance is shut down.");
                }
                var ieVersion = SysOperations.GetIeVersion();
                var defaultNodeCommand = _GetDefaultNodeCommand(_hubPort.ToString(CultureInfo.InvariantCulture), nodePort.ToString(CultureInfo.InvariantCulture));
                var nodeBrowsers = _BuildBrowsersNodeCommand(_nodeOptions, ieVersion);
                string nodeCommand = String.Format("{0}{1}", defaultNodeCommand, nodeBrowsers);
                _nodeProcess = ProcessHelper.StartProcess(String.Format(@"{0}\java.exe", SysOperations.GetJavaPath(_progress)), _progress, nodeCommand, false);
                Thread.Sleep(1000);
                if (_nodeProcess != null && _nodeProcess.HasExited == false)
                {
                    _progress.Report(
                        string.Concat(
                            "Node started. Please check the opened command window and/or browser window to ensure it started successfully. For logs look in C:\\Selenium\\_Logs folder.",
                            Environment.NewLine));
                    //remove double quotes from _nodeLog before storing
                    _launcherDataProvider.AddProcess(Type.Node, _nodeLog.Trim('\"'), _nodeProcess.Id, nodePort);
                }
                else
                {
                    _progress.Report(
                        string.Concat(
                            "There was a problem starting the node. Please check that all required files exist in C:\\Selenium folder, expecially 'selenium-server-standalone.jar'",
                            Environment.NewLine));
                }
            }
            catch (Exception e)
            {
                _progress.Report(string.Format("Could not start node. {0}{1}", e, Environment.NewLine));
            }
        }

        public void ShowGridConsole()
        {
            _gridConsoleProcess = ProcessHelper.StartProcess(String.Format("http://{0}:{1}/grid/console", _nodeOptions.Hub, _hubPort.ToString(CultureInfo.InvariantCulture)), _progress, null, false);
        }

        public void ShutDown()
        {
            Helpers.CloseGridConsole(_gridConsoleProcess);
            _gridConsoleProcess = null;
            if (_nodeProcess != null && !_nodeProcess.HasExited)
            {
                var processId = _nodeProcess.Id;
                _nodeProcess.Kill();
                _nodeProcess = null;
                _launcherDataProvider.RemoveProcess(processId);
            }
        }

        private string _GetDefaultNodeCommand(string hubPort, string nodePort)
        {
            return String.Format(
                @"-Xmx512m -jar C:\Selenium\ServerExecutable\selenium-server-standalone.jar -role wd -host {4} -hub http://{0}:{1}/grid/register -port {2} -trustAllSSLCertificates true -log {3} -Dwebdriver.chrome.driver=C:\Selenium\chromedriver_win32\chromedriver.exe -Dwebdriver.ie.driver=C:\Selenium\IEDriver_Win32\IEDriverServer.exe",
                _nodeOptions.Hub, hubPort, nodePort, _nodeLog, Environment.MachineName.ToString());
        }

        private static string _BuildBrowsersNodeCommand(LauncherOptions nodeParams, string ieVersion)
        {
            var nodeBrowsers = new StringBuilder();
            var osName = OperatingSystemInfo.ProductName;
            // If the OS name starts with "Microsoft," remove it.  We know that already
            osName = osName.TrimStart("Microsoft".ToCharArray());
            // If the OS name ends with "Enterprise" or "Standard" remove it.  We don't need it
            osName = osName.TrimEnd("Enterprise".ToCharArray()).TrimEnd("Standard".ToCharArray()).Trim();
            //If the OS name contains spaces and dots, remove them
            osName = osName.Replace(" ", "").Replace(".", "");
            foreach (var browser in nodeParams.BrowsersList)
            {
                if (browser.ToUpperInvariant() == "IE" || browser.ToUpperInvariant() == "INTERNET EXPLORER" || browser.ToUpperInvariant() == "INTERNETEXPLORER")
                {
                    nodeBrowsers.Append(String.Format(
                        " -browser \"platform=WINDOWS,version={0}{1},browserName=internet explorer,maxInstances=1\"",
                        ieVersion, osName));
                }
                else
                {
                    nodeBrowsers.Append(String.Format(
                        " -browser \"platform=WINDOWS,version={2},browserName={0},maxInstances={1}\"",
                        browser, nodeParams.MaxSessions, osName));
                }
            }
            return nodeBrowsers.ToString();
        }
    }
}
