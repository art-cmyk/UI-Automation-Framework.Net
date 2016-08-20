using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Ravitej.Automation.Common.Config.AppUnderTest;
using Ravitej.Automation.Common.Config.DriverSession;
using Ravitej.Automation.Common.Config.SuiteSettings;
using Ravitej.Automation.Common.Drivers.CapabilityProviders;
using Ravitej.Automation.Common.Tests;

namespace Ravitej.Automation.Common.Config.SettingsUpdater
{
    public abstract class BaseSettingsUpdater<TSuiteSettingsType> : TestBase<TSuiteSettingsType>, ISettingsUpdater
        where TSuiteSettingsType : ISuiteSettings, new()
    {
        private readonly string _baseAssemblyName;

        protected BaseSettingsUpdater(string baseAssemblyName)
        {
            _baseAssemblyName = baseAssemblyName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? baseAssemblyName : string.Concat(baseAssemblyName, ".dll");
        }

        public void UpdateAppConfigFile()
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(_baseAssemblyName);

            if (configuration.AppSettings.Settings["TestSettingsFile"] != null)
            {
                Console.WriteLine("Current TestSettingsFile = {0}", configuration.AppSettings.Settings["TestSettingsFile"].Value);
                configuration.AppSettings.Settings["TestSettingsFile"].Value = ConfigurationManager.AppSettings["TestSettingsFile"];
                Console.WriteLine("Updated TestSettingsFile = {0}", configuration.AppSettings.Settings["TestSettingsFile"].Value);
            }

            if (configuration.AppSettings.Settings["TestSettingsStore"] != null)
            {
                Console.WriteLine("Current TestSettingsStore = {0}", configuration.AppSettings.Settings["TestSettingsStore"].Value);
                configuration.AppSettings.Settings["TestSettingsStore"].Value = ConfigurationManager.AppSettings["TestSettingsStore"];
                Console.WriteLine("Updated TestSettingsStore = {0}", configuration.AppSettings.Settings["TestSettingsStore"].Value);
            }

            if (configuration.AppSettings.Settings["TestOutputPath"] != null)
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("TestOutputPath") && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["TestOutputPath"]))
                {
                    Console.WriteLine("Current TestOutputPath = {0}", configuration.AppSettings.Settings["TestOutputPath"].Value);
                    configuration.AppSettings.Settings["TestOutputPath"].Value = ConfigurationManager.AppSettings["TestOutputPath"];
                    Console.WriteLine("Updated TestOutputPath = {0}", configuration.AppSettings.Settings["TestOutputPath"].Value);
                }
            }

            configuration.Save();
        }

        public virtual void UpdateTestSuiteConfigFile()
        {
            _UpdateWebDriverSettings();
            _UpdateApplicationUnderTestSettings();
        }

        #region Private Helpers

        private void _UpdateStandardTypeSettings(IEnumerable<string> settingsToUpdate,
            List<PropertyInfo> availableSettings, object settingsObject)
        {
            foreach (var setting in settingsToUpdate)
            {
                var setting1 = setting;
                foreach (
                    var stringAvailableSetting in
                        availableSettings.Where(
                            aSetting => aSetting.Name.Equals(setting1) && aSetting.PropertyType == typeof(string)))
                {
                    Console.WriteLine("Current {0}: {1}", stringAvailableSetting.Name,
                        stringAvailableSetting.GetValue(settingsObject));
                    stringAvailableSetting.SetValue(settingsObject,
                        ConfigurationManager.AppSettings[setting1]);
                    Console.WriteLine("Updated {0}: {1}", stringAvailableSetting.Name,
                        stringAvailableSetting.GetValue(settingsObject));
                }

                foreach (
                    var boolAvailableSetting in
                        availableSettings.Where(
                            aSetting => aSetting.Name.Equals(setting1) && aSetting.PropertyType == typeof(bool)))
                {
                    Console.WriteLine("Current {0}: {1}", boolAvailableSetting.Name,
                        boolAvailableSetting.GetValue(settingsObject));
                    boolAvailableSetting.SetValue(settingsObject,
                        Convert.ToBoolean(ConfigurationManager.AppSettings[setting1]));
                    Console.WriteLine("Updated {0}: {1}", boolAvailableSetting.Name,
                        boolAvailableSetting.GetValue(settingsObject));
                }

                foreach (
                    var intAvailableSetting in
                        availableSettings.Where(
                            aSetting => aSetting.Name.Equals(setting1) && aSetting.PropertyType == typeof(int)))
                {
                    Console.WriteLine("Current {0}: {1}", intAvailableSetting.Name,
                        intAvailableSetting.GetValue(settingsObject));
                    intAvailableSetting.SetValue(settingsObject,
                        Convert.ToInt32(ConfigurationManager.AppSettings[setting1]));
                    Console.WriteLine("Updated {0}: {1}", intAvailableSetting.Name,
                        intAvailableSetting.GetValue(settingsObject));
                }
            }
        }

        private void _UpdateAdditionalCapabilities()
        {
            var additionalCaps = ConfigurationManager.AppSettings.AllKeys.Where(key => key.StartsWith("ac_"));
            additionalCaps = additionalCaps.Select(cap => cap.Substring(3));

            foreach (var cap in additionalCaps)
            {
                if (TestSuiteSettings.WebDriverSettings.AdditionalCapabilities.Exists(ac => ac.Id.Equals(cap)))
                {
                    var additionalCapability =
                        TestSuiteSettings.WebDriverSettings.AdditionalCapabilities.Find(ac => ac.Id.Equals(cap));
                    Console.WriteLine("Current additional capability '{0}' = '{1}'", cap, additionalCapability.Value);
                    additionalCapability.Value = ConfigurationManager.AppSettings[string.Concat("ac_", cap)];
                    Console.WriteLine("Updated additional capability '{0}' = '{1}'", cap, additionalCapability.Value);
                }
                else
                {
                    var additionalCapability = new AdditionalCapability
                    {
                        Id = cap,
                        Value = ConfigurationManager.AppSettings[string.Concat("ac_", cap)]
                    };
                    TestSuiteSettings.WebDriverSettings.AdditionalCapabilities.Add(additionalCapability);
                    Console.WriteLine("Added new additional capability '{0}' = '{1}'", cap, additionalCapability.Value);
                }
            }
        }

        private void _UpdateBrowser()
        {
            Console.WriteLine("Current Browser = {0}", TestSuiteSettings.WebDriverSettings.Browser.Value);
            var newBrowser = ConfigurationManager.AppSettings["Browser"];
            if (newBrowser == null)
            {
                Console.WriteLine("Browser setting not updated.");
                return;
            }
            TestSuiteSettings.WebDriverSettings.Browser = new PermittedSettingsValidatingItem<Browser>(newBrowser);
            Console.WriteLine("Updated Browser = {0}", TestSuiteSettings.WebDriverSettings.Browser.Value);
        }

        private void _UpdatePlatform()
        {
            Console.WriteLine("Current Platform = {0}", TestSuiteSettings.WebDriverSettings.Platform.Value);
            var newPlatform = ConfigurationManager.AppSettings["Platform"];
            if (newPlatform == null)
            {
                Console.WriteLine("Platform setting not updated.");
                return;
            }
            TestSuiteSettings.WebDriverSettings.Platform = new PermittedSettingsValidatingItem<Platform>(newPlatform);
            Console.WriteLine("Updated Platform = {0}", TestSuiteSettings.WebDriverSettings.Platform.Value);
        }

        private void _UpdateHubType()
        {
            Console.WriteLine("Current HubType = {0}", TestSuiteSettings.WebDriverSettings.HubType.Value);
            var newHubType = ConfigurationManager.AppSettings["HubType"];
            if (newHubType == null)
            {
                Console.WriteLine("HubType setting not updated.");
                return;
            }
            TestSuiteSettings.WebDriverSettings.HubType = new PermittedSettingsValidatingItem<HubType>(newHubType);
            Console.WriteLine("Updated HubType = {0}", TestSuiteSettings.WebDriverSettings.HubType.Value);
        }

        private void _UpdateWebDriverSettings()
        {
            var settingsToUpdate =
                ConfigurationManager.AppSettings.AllKeys.Where(key => !key.StartsWith("ac_")).ToList();
            var availableSettings = typeof(DriverSettings).GetProperties().ToList();
            //update standard data types (string, int, bool) properties of WebDriverSettings
            _UpdateStandardTypeSettings(settingsToUpdate, availableSettings, TestSuiteSettings.WebDriverSettings);
            //update custom data type properties of WebDriverSettings
            _UpdateHubType();
            _UpdateBrowser();
            _UpdatePlatform();
            _UpdateAdditionalCapabilities();
        }

        private void _UpdateDatabaseSettings()
        {
            var settingsToUpdate =
                ConfigurationManager.AppSettings.AllKeys.Where(key => !key.StartsWith("ac_")).ToList();
            var availableSettings = typeof(DatabaseSettings).GetProperties().ToList();
            _UpdateStandardTypeSettings(settingsToUpdate, availableSettings, TestSuiteSettings.ApplicationUnderTestSettings.DatabaseSettings);
        }

        private void _UpdateApplicationUnderTestSettings()
        {
            //update the standard data type properties of AUTSettings
            var settingsToUpdate =
                ConfigurationManager.AppSettings.AllKeys.Where(key => !key.StartsWith("ac_")).ToList();
            var availableSettings = typeof(AutSettings).GetProperties().ToList();
            _UpdateStandardTypeSettings(settingsToUpdate, availableSettings, TestSuiteSettings.ApplicationUnderTestSettings);
            //update custom data type settings of AUTSettings
            _UpdateDatabaseSettings();
            //cannot update users and pageElementIds yet!
        }

        #endregion
    }
}