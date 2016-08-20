using System;
using System.Collections.Generic;
using Ravitej.Automation.Common.Drivers.CapabilityProviders;

namespace Ravitej.Automation.Common.Config.DriverSession
{
    /// <summary>
    /// Represents webdriver settings applicable to a suite of tests.
    /// </summary>
    [Serializable]
    public class DriverSettings : ISelfHydratableSetting, ISelfHydratable<DriverSettings>
    {
        /// <summary>
        /// Initialises a new instance of <see cref="DriverSettings"/> with the following default values.  
        /// <see cref="CommandTimeoutSeconds"/> = 120,
        /// <see cref="ImplicitWaitSeconds"/> = 0, 
        /// <see cref="ScriptTimeoutSeconds"/> = 10, 
        /// <see cref="PageLoadTimeoutSeconds"/> = 60, 
        /// <see cref="MaximiseBrowser"/> = 10, 
        /// and initialises a new empty list of <see cref="AdditionalCapabilities"/>.
        /// </summary>
        public DriverSettings()
        {
            CommandTimeoutSeconds = 120;
            ImplicitWaitSeconds = 0;
            ScriptTimeoutSeconds = 10;
            PageLoadTimeoutSeconds = 60;
            MaximiseBrowser = true;
            AdditionalCapabilities = new List<AdditionalCapability>();
        }

        /// <summary>
        /// The type of hub being used for the tests.
        /// <see cref="Drivers.CapabilityProviders.HubType"/> = "Internal" indicates that the browser is started on a remote machine inside the firewall.
        /// <see cref="Drivers.CapabilityProviders.HubType"/> = "None" indicates that the browser is started on the local machine.
        /// </summary>
        public PermittedSettingsValidatingItem<HubType> HubType
        {
            get;
            set;
        }

        /// <summary>
        /// The url of the Selenium hub.
        /// </summary>
        public string HubUrl
        {
            get;
            set;
        }

        /// <summary>
        /// The browser to execute a suite of tests in.
        /// </summary>
        public PermittedSettingsValidatingItem<Browser> Browser
        {
            get;
            set;
        }

        /// <summary>
        /// The version of the browser to execute a suite of tests in.
        /// The string 'Latest' or '*' indicates that the latest version should be used.
        /// </summary>
        public string BrowserVersion
        {
            get;
            set;
        }

        /// <summary>
        /// The platform to execute a suite of tests on.
        /// </summary>
        public PermittedSettingsValidatingItem<Platform> Platform
        {
            get;
            set;
        }

        /// <summary>
        /// The version of the platform (Operating System) to execute a suite of tests on.
        /// The string 'Latest' or '*' indicates that the latest version should be used.
        /// </summary>
        public string PlatformVersion
        {
            get;
            set;
        }

        /// <summary>
        /// The command timeout value in seconds to assign to the webdriver instance. 
        /// CommandTimeout is the maximum amount of time to wait for each command sent to the browser.
        /// </summary>
        public int CommandTimeoutSeconds { get; set; }

        /// <summary>
        /// The implicit wait timeout value in seconds to assign to the webdriver instance. 
        /// See <see cref="OpenQA.Selenium.ITimeouts.ImplicitlyWait"/> for more information.
        /// </summary>
        public int ImplicitWaitSeconds
        {
            get;
            set;
        }

        /// <summary>
        /// The script timeout value in seconds to assign to the webdriver instance. 
        /// See <see cref="OpenQA.Selenium.ITimeouts.SetScriptTimeout"/> for more information.
        /// </summary>
        public int ScriptTimeoutSeconds
        {
            get;
            set;
        }

        /// <summary>
        /// The page load timeout value in seconds to assign to the webdriver instance. 
        /// See <see cref="OpenQA.Selenium.ITimeouts.SetPageLoadTimeout"/> for more information.
        /// </summary>
        public int PageLoadTimeoutSeconds
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether the browser should be maximised or not.
        /// </summary>
        public bool MaximiseBrowser
        {
            get;
            set;
        }

        /// <summary>
        /// Any additional capabilities that are passed on to the webdriver instance.
        /// </summary>
        public List<AdditionalCapability> AdditionalCapabilities
        {
            get;
            set;
        }

        /// <summary>
        /// Download directory path to set in browser profile for automatic (dialog-less) file download
        /// </summary>
        public string DownloadDirectory { get; set; }

        /// <summary>
        /// Assigns following default values to a new instance of <see cref="DriverSettings"/>.
        /// <see cref="Browser"/> = Chrome, 
        /// <see cref="BrowserVersion"/> = * (latest), 
        /// <see cref="Platform"/> = Windows, 
        /// <see cref="PlatformVersion"/> = 7 (Windows 7), 
        /// <see cref="HubType"/> = "Ravitej" (tests are directed to a hub inside the firewall - a hub running on the ADP network).
        /// </summary>
        /// <returns></returns>
        public DriverSettings SelfHydrate()
        {
            var retVal = new DriverSettings
                         {
                             Browser = new PermittedSettingsValidatingItem<Browser>("Chrome"),
                             BrowserVersion = "*",
                             HubType = new PermittedSettingsValidatingItem<HubType>("Internal"),
                             HubUrl = "http://localhost:4444/wd/hub",
                             Platform = new PermittedSettingsValidatingItem<Platform>("Windows"),
                             PlatformVersion = "7",
                             DownloadDirectory = @"C:\SeleniumDownloads"
                         };

            retVal.AdditionalCapabilities.Add(new AdditionalCapability
            {
                Id = "Sample",
                Value = "Sample Value"
            });

            return retVal;
        }
    }
}
