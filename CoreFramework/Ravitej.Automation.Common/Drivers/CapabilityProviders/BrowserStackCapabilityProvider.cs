using OpenQA.Selenium.Remote;
using Ravitej.Automation.Common.Config;
using Ravitej.Automation.Common.Config.DriverSession;

namespace Ravitej.Automation.Common.Drivers.CapabilityProviders
{
    internal class BrowserStackCapabilityProvider : BaseCapabilityProvider
    {
        public BrowserStackCapabilityProvider(DriverSettings driverSettings) : base(driverSettings)
        {
            BrowserDefaults(driverSettings.Browser.EnumValue);

            // if the version is set to latest or *, do nothing
            if (driverSettings.BrowserVersion != "Latest" && driverSettings.BrowserVersion != "*")
            {
                // set the version of the browser to use, such as Chrome 36
                Capabilities.SetCapability(CapabilityType.Version, driverSettings.BrowserVersion);
            }

            switch (driverSettings.Platform.EnumValue)
            {
                case Platform.Mac:
                    {
                        Capabilities.SetCapability("os", "MAC");
                        break;
                    }
                case Platform.Windows:
                    {
                        Capabilities.SetCapability("os", "Windows");
                        break;
                    }
                case Platform.Linux:
                    {
                        Capabilities.SetCapability("os", "Linux");
                        break;
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
                                    Capabilities.SetCapability("os_version", "8.1");
                                    break;
                                }
                            default:
                                {
                                    Capabilities.SetCapability("os_version", driverSettings.PlatformVersion);
                                    break;
                                }
                        }
                        break;
                    }
                case Platform.Mac:
                    {
                        var platVersion = new PermittedSettingsValidatingItem<MacOs>(driverSettings.PlatformVersion);
                        Capabilities.SetCapability("os_version", driverSettings.PlatformVersion);
                        break;
                    }
            }
        }

        public override DesiredCapabilities FinalizeCapabilities()
        {
            // nothing to do here
            return Capabilities;
        }
    }
}
