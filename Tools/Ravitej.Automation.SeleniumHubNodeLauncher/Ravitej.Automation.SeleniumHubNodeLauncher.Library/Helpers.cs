using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Principal;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library.CommandLine;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library.Data;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.Library
{
    public static class Helpers
    {
        private static void StopHubAndNode()
        {
            ProcessHelper.KillProcessByNameAndCurrentUser("java", 0);
        }

        private static bool AreHubAndNodeStarted(int secondsAgo)
        {
            return ProcessHelper.ProcessExists("java", WindowsIdentity.GetCurrent().Name, secondsAgo);
        }

        public static void EncryptConfig(string exePath)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
            ConfigurationSection section = config.GetSection("appSettings"); // could be any section

            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                config.Save();
            }
        }

        public static void DecryptConfig(string exePath)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
            ConfigurationSection section = config.GetSection("appSettings"); // could be any section

            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
                //config.Save(); --do not save the decrypted config.
            }
        }

        public static Hub StartHub(LauncherOptions options, IProgress<string> progress, out int hubPort)
        {
            var hub = new Hub(options, progress);
            hubPort = hub.Start();
            if (options.ShowConsole.ToLower().Equals("yes"))
            {
                hub.ShowGridConsole(hubPort);
            }
            return hub;
        }

        public static Node StartNode(LauncherOptions options, IProgress<string> progress, int hubPort)
        {
            var node = new Node(options, progress, hubPort);
            node.Start();
            if (options.ShowConsole.ToLower().Equals("yes"))
            {
                node.ShowGridConsole();
            }
            return node;
        }

        public static void CopySeleniumJars(IProgress<string> progress, bool showUiDialogs, string networkPath, NetworkCredential networkCredential)
        {
            var fileDirOperations = new FileDirOperations(networkPath, networkCredential, @"C:\Selenium");
            progress.Report("Checking for newer version(s)..." + Environment.NewLine);
            if (!fileDirOperations.IsLatestVersion())
            {
                progress.Report("Newer version(s) available on the server. Copying now..." + Environment.NewLine);
                fileDirOperations.DeleteLocalContents();
                try
                {
                    fileDirOperations.CopyServerContents(showUiDialogs);
                    progress.Report("Latest version(s) copied successfully." + Environment.NewLine);
                }
                catch (Exception e)
                {
                    progress.Report(
                        "Copy operation interrupted. Please ensure latest files are copied. Click the button again. " +
                        e.Message + Environment.NewLine);
                }
            }
            else
            {
                progress.Report("All local files are latest version(s)." + Environment.NewLine);
            }
        }

        public static bool SeleniumJarsExistLocally()
        {
            var fileDirOperations = new FileDirOperations(@"C:\Selenium");
            return fileDirOperations.CheckSeleniumJarsExistLocally();
        }

        public static void CloseGridConsole(Process gridConsoleProcess)
        {
            if (gridConsoleProcess != null)
            {
                gridConsoleProcess.Kill();
            }
        }

        public static void CheckAndSyncData()
        {
            var data = new LauncherDataProvider();
            var activeProcesses = data.GetActiveProcesses();
            foreach (var process in activeProcesses)
            {
                var processId = Int32.Parse(process.Id);
                if (!ProcessHelper.ProcessExists(processId, process.LogFile))
                {
                    data.RemoveProcess(processId);
                }
            }

            var runningSeleniumProcesses = ProcessHelper.GetRunningProcesses("java", "selenium-server-standalone");
            foreach (var runningSeleniumProcess in runningSeleniumProcesses)
            {
                var args = runningSeleniumProcess["CommandLine"].ToString();
                var split = args.Split('-');
                var type = args.Contains("-role hub") ? SeleniumHubNodeLauncher.Library.Data.Type.Hub : SeleniumHubNodeLauncher.Library.Data.Type.Node;
                var id = runningSeleniumProcess["ProcessId"].ToString();
                var log = string.Empty;
                if (args.Contains("-log"))
                {
                    log = split.FirstOrDefault(s => s.Contains("log")).Replace("log ", "").Trim();
                }
                var port = string.Empty;
                if (args.Contains("-port"))
                {
                    port = split.FirstOrDefault(s => s.Contains("port")).Replace("port ", "").Trim();
                }
                else
                {
                    port = type == SeleniumHubNodeLauncher.Library.Data.Type.Hub ? "4444" : "5555";
                }
                data.AddProcess(type, log, int.Parse(id), int.Parse(port));
            }
        }
    }
}
