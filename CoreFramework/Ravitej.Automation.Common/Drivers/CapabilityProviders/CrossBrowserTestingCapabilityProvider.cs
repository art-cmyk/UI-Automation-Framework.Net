using System;
using OpenQA.Selenium.Remote;
using Ravitej.Automation.Common.Config;
using Ravitej.Automation.Common.Config.DriverSession;

namespace Ravitej.Automation.Common.Drivers.CapabilityProviders
{
    internal class CrossBrowserTestingCapabilityProvider : BaseCapabilityProvider
    {
        public CrossBrowserTestingCapabilityProvider(DriverSettings driverSettings) : base(driverSettings)
        {
            BrowserDefaults(driverSettings.Browser.EnumValue);
            string browserName;
            switch (driverSettings.Browser.EnumValue)
            {
                case Browser.Chrome:
                    {
                        browserName = "Chrome";
                        break;
                    }
                case Browser.Firefox:
                    {
                        browserName = "FF";
                        break;
                    }
                case Browser.Opera:
                    {
                        browserName = "Opera";
                        break;
                    }
                case Browser.InternetExplorer:
                    {
                        browserName = "IE";
                        break;
                    }
                case Browser.Edge:
                    {
                        throw new Exception("Microsoft Edge is not yet supported.");
                    }
                case Browser.Safari:
                    {
                        browserName = "Safari";
                        break;
                    }
                case Browser.Unspecified:
                    {
                        throw new ArgumentException("Unexpected browser specified");
                    }
                default:
                    {
                        throw new ArgumentException("Unexpected browser specified");
                    }
            }

            // If the version is NOT SET, throw an error as the settings seem to require it
            if (driverSettings.BrowserVersion != "Latest" && driverSettings.BrowserVersion != "*")
            {
                // set the version of the browser to use, such as Chrome 36
                Capabilities.SetCapability(CapabilityType.Version, driverSettings.BrowserVersion);
            }

            Capabilities.SetCapability("browser_api_name", $"{browserName}{driverSettings.BrowserVersion}");

            string osVersion = string.Empty;

            switch (driverSettings.Platform.EnumValue)
            {
                case Platform.Mac:
                    {
                        osVersion = "Mac";
                        break;
                    }
                case Platform.Windows:
                    {
                        osVersion = "Win";
                        break;
                    }
                case Platform.Linux:
                    {
                        throw new ArgumentException("Unexpected platform specified - Linux is not supported");
                    }
            }

            switch (driverSettings.Platform.EnumValue)
            {
                case Platform.Windows:
                    {
                        var platVersion = new PermittedSettingsValidatingItem<WindowsOs>(string.Concat("Windows", driverSettings.PlatformVersion));
                        switch (platVersion.EnumValue)
                        {
                            // 8.1 needs to be specified differently.
                            case WindowsOs.Windows81:
                                {
                                    osVersion += "8.1";
                                    break;
                                }
                            default:
                                {
                                    osVersion += driverSettings.PlatformVersion;
                                    break;
                                }
                        }
                        break;
                    }
                case Platform.Mac:
                    {
                        var platVersion = new PermittedSettingsValidatingItem<MacOs>(driverSettings.PlatformVersion);
                        osVersion += driverSettings.PlatformVersion;
                        break;
                    }
            }

            Capabilities.SetCapability("os_api_name", osVersion);
        }

        public override DesiredCapabilities FinalizeCapabilities()
        {
            // nothing to do here
            return Capabilities;
        }
    }
}
