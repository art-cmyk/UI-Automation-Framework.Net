using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CommandLine;
using Ravitej.Automation.Configure.CommandLine;

namespace Ravitej.Automation.Configure
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var options = new DeploymentOptions();
            if (Parser.Default.ParseArguments(args, options))
            {
                var executeDeployment = true;

                _AcknowledgeArguments(options);

                var hostsEntries = options.HostsFileEntryValues();
                if (hostsEntries.Any())
                {
                    var hostsUpdater = new FileUpdaters.HostsFile();

                    if (options.RemoveHostsEntries)
                    {
                        hostsUpdater.RemoveHostsEntries(hostsEntries);
                        executeDeployment = false;
                    }
                    else
                    {
                        hostsUpdater.AddHostsEntries(hostsEntries);
                    }
                }

                // check to see if the deployment should be executed, it may be aborted by above steps.
                if (executeDeployment)
                {
                    var additionalCapabilities = new Dictionary<string, string>
                    {
                        {"TestSettingsFile", options.TestSettingsFile},
                        {"TestSettingsStore", options.TestSettingsStore}
                    };

                    foreach (var capability in options.GetAdditionalCapabilities())
                    {
                        additionalCapabilities.Add(capability.Key, capability.Value);
                    }
                    var previousAppSettings = _UpdateDeploymentConfig(additionalCapabilities);
                    UpdaterFacade.UpdateSettingsConfig(options.TestAssembly);
                    //reset the deployment app config to its original state 
                    //this is to avoid future runs of the app making unintended
                    //changes to the settings
                    _ResetDeploymentConfig(previousAppSettings);
                }
            }
        }

        private static void _ResetDeploymentConfig(Dictionary<string, string> appSettings)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (var key in appSettings.Keys)
            {
                if (appSettings[key] != null)
                {
                    configuration.AppSettings.Settings[key].Value = appSettings[key];
                }
                else
                {
                    configuration.AppSettings.Settings.Remove(key);
                }
            }
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }

        private static Dictionary<string, string> _UpdateDeploymentConfig(Dictionary<string, string> appSettings)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var previousAppSettings = new Dictionary<string, string>();
            foreach (var key in appSettings.Keys)
            {
                var configurationElement = configuration.AppSettings.Settings[key];
                if (configurationElement != null)
                {
                    var oldValue = configurationElement.Value;
                    configurationElement.Value = appSettings[key];
                    previousAppSettings.Add(key, oldValue);
                }
                else
                {
                    configuration.AppSettings.Settings.Add(key, appSettings[key]);
                    previousAppSettings.Add(key, null);
                }
            }
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
            return previousAppSettings;
        }

        private static void _AcknowledgeArguments(DeploymentOptions options)
        {
            Console.WriteLine("\r\n==== Specified options begin ====");
            // get all public static properties of DeploymentOptions type
            var propertyInfos = typeof(DeploymentOptions).GetProperties();

            //print them all to the console
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.Name != "AdditionalCapabilitiesRaw")
                {
                    Console.WriteLine("{0}: {1}", propertyInfo.Name, propertyInfo.GetValue(options));
                }
            }
            foreach (var capability in options.GetAdditionalCapabilities())
            {
                Console.WriteLine("{0}: {1}", capability.Key, capability.Value);
            }
            Console.WriteLine("==== Specified options end ======\r\n");
        }
    }
}
