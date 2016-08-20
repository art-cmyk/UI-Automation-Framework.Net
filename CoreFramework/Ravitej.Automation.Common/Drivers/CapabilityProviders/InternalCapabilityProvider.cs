using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Ravitej.Automation.Common.Config.DriverSession;
using Platform = Ravitej.Automation.Common.Config.DriverSession.Platform;

namespace Ravitej.Automation.Common.Drivers.CapabilityProviders
{
    internal class InternalCapabilityProvider : BaseCapabilityProvider
    {
        private readonly string _targetBrowserVersion;

        private readonly Platform _platform;

        private readonly string _targetPlatformVersion;

        public InternalCapabilityProvider(DriverSettings driverSettings) : base(driverSettings)
        {
            BrowserDefaults(driverSettings.Browser.EnumValue);

            // if the version is set to latest or *, do nothing
            if (driverSettings.BrowserVersion != "Latest" && driverSettings.BrowserVersion != "*")
            {
                _targetBrowserVersion = driverSettings.BrowserVersion;
            }

            _platform = driverSettings.Platform.EnumValue;

            switch (_platform)
            {
                case Platform.Mac:
                    {
                        Capabilities.Platform = new OpenQA.Selenium.Platform(PlatformType.Mac);
                        break;
                    }
                case Platform.Windows:
                    {
                        Capabilities.Platform = new OpenQA.Selenium.Platform(PlatformType.Windows);
                        break;
                    }
                case Platform.Linux:
                    {
                        Capabilities.Platform = new OpenQA.Selenium.Platform(PlatformType.Linux);
                        break;
                    }
                case Platform.iOS:
                    {
                        Capabilities.SetCapability("platformVersion", driverSettings.PlatformVersion);
                        Capabilities.SetCapability("platformName", driverSettings.Platform.EnumValue);
                        break;
                    }
                case Platform.Android:
                    {
                        Capabilities.Platform = new OpenQA.Selenium.Platform(PlatformType.Android);
                        Capabilities.SetCapability("platformVersion", driverSettings.PlatformVersion);
                        Capabilities.SetCapability("platformName", driverSettings.Platform.EnumValue);
                        break;
                    }
            }

            // if the version is set to latest or *, do nothing
            if (driverSettings.PlatformVersion != "Latest" && driverSettings.PlatformVersion != "*")
            {
                _targetPlatformVersion = driverSettings.PlatformVersion;
            }
        }

        public override DesiredCapabilities FinalizeCapabilities()
        {
            //only set the "version" capability for web browser requests on PCs and MACs
            if (_platform == Platform.Windows || _platform == Platform.Mac || _platform == Platform.Linux)
            {
                // set the version to the expected format:
                if ((!string.IsNullOrWhiteSpace(_targetBrowserVersion)) &&
                    (!string.IsNullOrWhiteSpace(_targetPlatformVersion)))
                {
                    // 11Windows7
                    Capabilities.SetCapability("version",
                        _targetBrowserVersion + string.Concat(_platform.ToString(), _targetPlatformVersion));
                }
                else if (!string.IsNullOrWhiteSpace(_targetPlatformVersion))
                {
                    // Windows7
                    Capabilities.SetCapability("version", string.Concat(_platform.ToString(), _targetPlatformVersion));
                }
            }

            return Capabilities;
        }
    }
}
