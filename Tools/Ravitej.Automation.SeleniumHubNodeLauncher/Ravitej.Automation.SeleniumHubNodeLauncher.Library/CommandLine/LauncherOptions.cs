using CommandLine;
using System.Collections.Generic;
using CommandLine.Text;
using Newtonsoft.Json;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.Library.CommandLine
{
    public class LauncherOptions
    {
        [Option('h', "startHub", MutuallyExclusiveSet = "Start", DefaultValue = "yes", HelpText = "Start hub? Usage: -h yes or -h no or --startHub=yes or --startHub=no")]
        public string StartHub { get; set; }

        [Option("stopHub", MutuallyExclusiveSet = "Stop", HelpText = "Stop hub? Usage: --stopHub")]
        public bool StopHub { get; set; }

        [Option("stopNode", MutuallyExclusiveSet = "Stop", HelpText = "Stop node? Usage: --stopNode")]
        public bool StopNode { get; set; }

        [Option('c', "showConsole", DefaultValue = "yes", MutuallyExclusiveSet = "Start", HelpText = "Show grid console after launching hub and/or node? Usage: -c yes or -c no or --showConsole=yes or --showConsole=no")]
        public string ShowConsole { get; set; }

        [Option('p', "additionalHubParams", MutuallyExclusiveSet = "Start", HelpText = "Additional parameters to start the hub with. Usage: -p \"{'timeout' : '60', 'browserTimeout' : '120' }\" or --additionalHubParams=\"{'timeout' : '60', 'browserTimeout' : '120' }\" ")]
        public string HubParams { get; set; }

        [Option('n', "startNode", MutuallyExclusiveSet = "Start", DefaultValue = "yes", HelpText = "Start node? Usage: -n yes or -n no or --startNode=yes or --startNode=no.")]
        public string StartNode { get; set; }

        [OptionList('b', "browsers", Separator = ',', MutuallyExclusiveSet = "Start", DefaultValue = new[] { "chrome", "firefox", "ie" }, HelpText = "Browser(s) that the node is capable of running tests in. Example usage: -b chrome,firefox,ie or --browsers=chrome,firefox")]
        public IEnumerable<string> BrowsersList { get; set; }

        [Option('s', "maxSessions", MutuallyExclusiveSet = "Start", DefaultValue = "5", HelpText = "Maximum no.of browser sessions that this node is capable of running parallely. Example usage: -s 5 or --maxSessions=5")]
        public string MaxSessions { get; set; }

        [Option("hub", MutuallyExclusiveSet = "Start", DefaultValue = "localhost", HelpText = "Hub with which this node should register. Example usage: --hub=bmswkalururav")]
        public string Hub { get; set; }

        [Option('l', "logsLocation", MutuallyExclusiveSet = "Start", Required = false, DefaultValue = "C:\\Selenium\\_Logs", HelpText = "Where to create hub and node logs? Usage: -l \"C:\\Selenium\\_Logs\" or --logsLocation=\"C:\\Selenium\\_Logs\"")]
        public string LogsLocation { get; set; }

        [HelpOption(HelpText = "Display this help screen.")]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        public Dictionary<string, string> GetAdditionalHubParams()
        {
            var keyValuePairs = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(HubParams)) return keyValuePairs;
            dynamic jsonObject = JsonConvert.DeserializeObject(HubParams);
            foreach (var pair in jsonObject)
            {
                keyValuePairs[pair.Name] = pair.Value.ToString();
            }
            return keyValuePairs;
        }
    }
}