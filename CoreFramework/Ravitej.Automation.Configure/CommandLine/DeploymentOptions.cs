using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using Newtonsoft.Json;
using Ravitej.Automation.Configure.FileUpdaters;

namespace Ravitej.Automation.Configure.CommandLine
{
    public class DeploymentOptions
    {
        [Option('f', "testsettingsfile", Required = true, HelpText = "This argument is REQUIRED. Path and name of TestSettingsFile. Usage: -f \"RunSettings_QA.Config\" or --testsettingsfile \"RunSettings_QA.Config\"")]
        public string TestSettingsFile { get; set; }

        [Option('s', "testsettingsstore", Required = true, HelpText = "This argument is REQUIRED. Path and name of TestSettingsStore. Usage: -s \"\\\\some-shared-path\\RunSettings_QA.Config\" or --testsettingsfile \"\\\\some-shared-path\\RunSettings_QA.Config\"")]
        public string TestSettingsStore { get; set; }

        [Option('a', "additionalcaps", HelpText = "Additional capabilities to add to the config. Usage: -a \"{'key1' : 'value1', 'key2' : 'value2' }\" ")]
        public string AdditionalCapabilitiesRaw { get; set; }

        [Option('t', "testassembly", Required = true, HelpText = "This argument is REQUIRED.  The name of the tests assembly (DLL) whose relevant settings to update. Usage: -t \"Adp.Automation.Ess.UI.Tests.dll\" or --testassembly \"Adp.Automation.Run.UI.Tests.dll\"")]
        public string TestAssembly { get; set; }

        [Option('h', "hostsentries", HelpText = "Additional hosts entries to add to the server. Usage: -h \"[['10.97.116.243', 'runessmain-dit-995770.nj.adp.com'], ['10.97.118.238', 'runess-dit-995770.nj.adp.com']]\"")]
        public string HostsFileEntries { get; set; }

        [Option('r', "removehostsentries", HelpText = "Remove any additional hosts entries that are specified in the -h setting server.  Setting this option will NOT execute the deployment, it will simply remove the entries and terminate. Usage: -r")]
        public bool RemoveHostsEntries
        {
            get;
            set;
        }

        public Dictionary<string, string> GetAdditionalCapabilities()
        {
            var keyValuePairs = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(AdditionalCapabilitiesRaw)) return keyValuePairs;
            dynamic jsonObject = JsonConvert.DeserializeObject(AdditionalCapabilitiesRaw);
            foreach (var pair in jsonObject)
            {
                keyValuePairs[pair.Name] = pair.Value.ToString();
            }
            return keyValuePairs;
        }

        public List<HostsFile.HostsFileEntry> HostsFileEntryValues()
        {
            var retList = new List<HostsFile.HostsFileEntry>();

            if (!string.IsNullOrWhiteSpace(HostsFileEntries))
            {
                string[,] deserialized = JsonConvert.DeserializeObject<string[,]>(HostsFileEntries);
                for(var i = 0; i < deserialized.Length / 2; i++)
                {
                    var newItem = new HostsFile.HostsFileEntry();
                    newItem.TargetIpAddress = deserialized[i, 0];
                    newItem.HostName = deserialized[i, 1];
                    retList.Add(newItem);
                }
            }

            return retList;
        }

        [HelpOption(HelpText = "Display this help screen.")]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
