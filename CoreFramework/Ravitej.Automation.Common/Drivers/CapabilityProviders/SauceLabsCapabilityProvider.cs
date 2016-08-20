using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Ravitej.Automation.Common.Config.DriverSession;
using Platform = Ravitej.Automation.Common.Config.DriverSession.Platform;

namespace Ravitej.Automation.Common.Drivers.CapabilityProviders
{
    internal class SauceLabsCapabilityProvider : BaseCapabilityProvider
    {
        public SauceLabsCapabilityProvider(DriverSettings driverSettings) : base(driverSettings)
        {
            BrowserDefaults(driverSettings.Browser.EnumValue);

            // if the version is set to latest or *, do nothing
            if (driverSettings.BrowserVersion != "Latest" && driverSettings.BrowserVersion != "*")
            {
                // set the version of the browser to use, such as Chrome 36
                Capabilities.SetCapability("version", driverSettings.BrowserVersion);
            }

            switch (driverSettings.Platform.EnumValue)
            {
                case Platform.Mac:
                    {
                        Capabilities.SetCapability("platform", $"OS X {driverSettings.PlatformVersion}");
                        break;
                    }
                case Platform.Windows:
                    {
                        Capabilities.SetCapability("platform", $"Windows {driverSettings.PlatformVersion}");
                        break;
                    }
                case Platform.Linux:
                    {
                        Capabilities.SetCapability("platform", $"Linux {driverSettings.PlatformVersion}");
                        break;
                    }
                case Platform.iOS:
                    {
                        Capabilities.SetCapability("appiumVersion", "1.4.10");
                        Capabilities.SetCapability("platformName", driverSettings.Platform.EnumValue);
                        Capabilities.SetCapability("platformVersion", driverSettings.PlatformVersion);
                        Capabilities.SetCapability("browserName", "");
                        Capabilities.SetCapability("deviceName", "iPhone Simulator");
                        Capabilities.SetCapability("device-orientation", "portrait");
                        break;
                    }
                case Platform.Android:
                    {
                        Capabilities.SetCapability("platform", PlatformType.Android);
                        Capabilities.SetCapability("platformName", driverSettings.Platform.EnumValue);
                        Capabilities.SetCapability("platformVersion", driverSettings.PlatformVersion);
                        Capabilities.SetCapability("browserName", "");
                        break;
                    }
                //case Platform.Unspecified:
                //    {
                //        return;
                //    }
            }
        }

        public override DesiredCapabilities FinalizeCapabilities()
        {
            // nothing to do here
            return Capabilities;
        }
    }
}
