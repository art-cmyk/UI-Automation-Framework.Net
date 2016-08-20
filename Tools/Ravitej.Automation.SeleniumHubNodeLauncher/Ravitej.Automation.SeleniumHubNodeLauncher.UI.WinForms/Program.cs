using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using CommandLine;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library.CommandLine;
using System.Windows.Forms;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.UI.WinForms
{
    static class Program
    {
        private enum RunStyle
        {
            Console,
            Gui,
            DontRun
        }

        private static readonly LauncherOptions Options = new LauncherOptions();
        private static bool _startHub;
        private static bool _startNode;
        private static readonly Action<string> OutputAction = s => Console.WriteLine(s);
        private static readonly IProgress<string> Progress = new Progress<string>(OutputAction);
        private static readonly string NetworkPath = ConfigurationManager.AppSettings["SeleniumExecutablesPath"];
        private static NetworkCredential _networkCredential;
        private static string _logsFolder;
        private static int _hubPort;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var howToRun = ShouldRunAsCommandLine(args);
            if (howToRun == RunStyle.Console)
            {
                LaunchViaCommandLineArguments(args);
            }
            else if (howToRun == RunStyle.Gui)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmLauncherMain());
            }
        }

        static RunStyle ShouldRunAsCommandLine(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                return RunStyle.Gui;
            }

            Parser.Default.ParseArguments(args, Options);

            var runStyle = RunStyle.DontRun;
            var showHelp = false;

            if (args.FirstOrDefault(s => s.Equals("/?", StringComparison.OrdinalIgnoreCase)) != null)
            {
                runStyle = RunStyle.DontRun;
                showHelp = true;
            }

            if (args.FirstOrDefault(s => s.Equals("/s", StringComparison.OrdinalIgnoreCase)) != null)
            {
                runStyle = RunStyle.Console;
            }

            if (runStyle != RunStyle.Gui)
            {
                NativeMethods.AllocConsole();

                Console.WriteLine("Starting the Selenium Hub Launcher in silent mode (console app)...");

                if (showHelp)
                {
                    Console.WriteLine("\r\nPermitted console args are\r\n: {0}", Options.GetUsage());
                    Console.WriteLine("\r\nPress any key to terminate.");
                    Console.ReadLine();
                }
            }

            return runStyle;
        }

        static void LaunchViaCommandLineArguments(string[] args)
        {
            var exePath = string.Format("{0}\\SeleniumHubNodeLauncherUI.exe", AppDomain.CurrentDomain.BaseDirectory);
            Helpers.EncryptConfig(exePath);
            _AcknowledgeAndCheckArguments();

            FileDirOperations.CreateRequiredDirectories(Progress, Options.LogsLocation);

            try
            {
                Helpers.DecryptConfig(exePath);
                var username = ConfigurationManager.AppSettings["Username"];
                var password = ConfigurationManager.AppSettings["Password"];
                var domain = ConfigurationManager.AppSettings["Domain"];
                _networkCredential = new NetworkCredential(username, password, domain);
                Helpers.CopySeleniumJars(Progress, true, NetworkPath, _networkCredential);
            }
            catch (Win32Exception exception)
            {
                Progress.Report(string.Format("An error has occurred while trying to copy files. {0}{1}", exception.Message, Environment.NewLine));
                Progress.Report(string.Concat("Attempting to start the hub and node with local versions of the files, if they exist.", Environment.NewLine));
            }

            if (_startHub)
            {
                Helpers.StartHub(Options, Progress, out _hubPort);
            }
            if (_startNode)
            {
                if (_hubPort != default(int))
                {
                    Helpers.StartNode(Options, Progress, _hubPort);
                }
                else
                {
                    Progress.Report("Could not start node as the hub wasn't started.");
                }
            }
        }

        private static void _AcknowledgeAndCheckArguments()
        {
            Console.WriteLine("\r\n==== Specified options begin ====");
            Console.WriteLine("Start hub: {0}", Options.StartHub);
            Console.WriteLine("Additional hub params: {0}", Options.HubParams == null ? "none specified" : string.Join(" ", Options.HubParams));
            Console.WriteLine("Start node: {0}", Options.StartNode);
            Console.WriteLine("Browsers: {0}", Options.BrowsersList == null ? "none specified" : string.Join(",", Options.BrowsersList));
            Console.WriteLine("Max Sessions: {0}", Options.MaxSessions);
            Console.WriteLine("Hub : {0}", Options.Hub);
            Console.WriteLine("==== Specified options end ======\r\n");
            _startHub = Options.StartHub.ToUpperInvariant() == "YES";
            _startNode = Options.StartNode.ToUpperInvariant() == "YES";

            if (!_startHub)
            {
                if (Options.StartHub.ToUpperInvariant() != "NO")
                {
                    Console.WriteLine("\r\n{0}", Options.GetUsage());
                    Environment.Exit(1);
                }
            }

            if (!_startNode)
            {
                if (Options.StartNode.ToUpperInvariant() != "NO")
                {
                    Console.WriteLine("\r\n{0}", Options.GetUsage());
                    Environment.Exit(1);
                }
            }
        }
    }

    internal static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        internal static extern Boolean AllocConsole();
    }
}
