using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using CommandLine;
using Newtonsoft.Json;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library.CommandLine;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.Console
{
    public static class Program
    {
        private static readonly LauncherOptions Options = new LauncherOptions();
        private static bool _startHub;
        private static bool _startNode;
        private static readonly Action<string> OutputAction = s => System.Console.WriteLine(s);
        private static readonly IProgress<string> Progress = new Progress<string>(OutputAction);
        private static readonly string NetworkPath = ConfigurationManager.AppSettings["SeleniumExecutablesPath"];
        private static NetworkCredential _networkCredential;
        private static bool _filesCopied;
        private static int _hubPort;
        private static Hub _hub;
        private static Node _node;

        public static void Main(string[] args)
        {
            var exePath = string.Format("{0}\\SeleniumHubNodeLauncherConsole.exe", AppDomain.CurrentDomain.BaseDirectory);
            Helpers.EncryptConfig(exePath);
            if (Parser.Default.ParseArguments(args, Options))
            {
                try
                {
                    _AcknowledgeAndCheckArguments();
                }
                catch (JsonException e)
                {
                    System.Console.WriteLine(
                        "There was a problem parsing HubParams. The HubParams are expected to be in a Json string format. Please see the exception and make necessary corrections. {0}",
                        e);
                    Options.GetUsage();
                    Environment.Exit(1);
                }
                //If the options are to stop hub and/or node...
                if (Options.StopHub || Options.StopNode)
                {
                    if (Options.StopHub && _hub != null)
                    {
                        _hub.ShutDown();
                        Progress.Report(string.Concat("Hub shutdown successfully.", Environment.NewLine));
                    }
                    if (Options.StopNode && _node != null)
                    {
                        _node.ShutDown();
                        Progress.Report(string.Concat("Node shutdown successfully.", Environment.NewLine));
                    }
                    Environment.Exit(0);
                }

                FileDirOperations.CreateRequiredDirectories(Progress, Options.LogsLocation);
                var filesExistLocally = Helpers.SeleniumJarsExistLocally();

                try
                {
                    Helpers.DecryptConfig(exePath);
                    var username = ConfigurationManager.AppSettings["Username"];
                    var password = ConfigurationManager.AppSettings["Password"];
                    var domain = ConfigurationManager.AppSettings["Domain"];
                    _networkCredential = new NetworkCredential(username, password, domain);
                    Helpers.CopySeleniumJars(Progress, true, NetworkPath, _networkCredential);
                    _filesCopied = true;
                }
                catch (Win32Exception exception)
                {
                    Progress.Report(
                    string.Format(
                        "An error has occurred while trying to copy files from {1}. {0}.{2}",
                        exception.Message, NetworkPath, Environment.NewLine));
                    if (filesExistLocally)
                    {
                        Progress.Report(
                            string.Concat(
                                "Attempting to start the hub and node with previous versions of the files which exist locally.",
                                Environment.NewLine));
                    }
                }

                if ((_filesCopied || filesExistLocally) && _startHub)
                {
                    _hub = Helpers.StartHub(Options, Progress, out _hubPort);
                }
                if ((_filesCopied || filesExistLocally) && _startNode)
                {
                    if (_hubPort != default(int))
                    {
                        _node = Helpers.StartNode(Options, Progress, _hubPort);
                    }
                    else
                    {
                        Progress.Report("Could not start node as the hub wasn't started.");
                    }
                }
            }
        }

        private static void _AcknowledgeAndCheckArguments()
        {
            System.Console.WriteLine("\r\n==== Specified options begin ====");
            // get all public static properties of DeploymentOptions type
            var propertyInfos = typeof(LauncherOptions).GetProperties();

            //print them all to the console
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.Name.Equals("BrowsersList"))
                {
                    System.Console.WriteLine("{0}: {1}", propertyInfo.Name,
                        propertyInfo.GetValue(Options) == null
                            ? "none specified"
                            : string.Join(" ", (IEnumerable<string>)propertyInfo.GetValue(Options)));
                }
                else if (propertyInfo.Name != "HubParams")
                {
                    System.Console.WriteLine("{0}: {1}", propertyInfo.Name, propertyInfo.GetValue(Options));
                }
            }
            if (Options.GetAdditionalHubParams().Count > 0)
            {
                System.Console.WriteLine("***** Additional Hub Parameters *****");
                foreach (var capability in Options.GetAdditionalHubParams())
                {
                    System.Console.WriteLine("{0}: {1}", capability.Key, capability.Value);
                }
                System.Console.WriteLine("***** Additional Hub Parameters *****");
            }
            System.Console.WriteLine("==== Specified options end ======\r\n");
            _startHub = Options.StartHub.ToUpperInvariant() == "YES";
            _startNode = Options.StartNode.ToUpperInvariant() == "YES";
            if (!_startHub)
            {
                if (Options.StartHub.ToUpperInvariant() != "NO")
                {
                    System.Console.WriteLine("\r\n{0}", Options.GetUsage());
                    Environment.Exit(1);
                }
            }

            if (!_startNode)
            {
                if (Options.StartNode.ToUpperInvariant() != "NO")
                {
                    System.Console.WriteLine("\r\n{0}", Options.GetUsage());
                    Environment.Exit(1);
                }
            }
        }
    }
}
