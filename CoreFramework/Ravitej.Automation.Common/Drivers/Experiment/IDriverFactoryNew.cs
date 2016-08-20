using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using Ravitej.Automation.Common.Config.DriverSession;
using Ravitej.Automation.Common.Drivers.CapabilityProviders;
using Platform = Ravitej.Automation.Common.Config.DriverSession.Platform;

namespace Ravitej.Automation.Common.Drivers.Experiment
{
    public interface IDriverFactoryNew
    {
        IEnumerable<Browser> SupportedBrowsers { get; }
        IEnumerable<Platform> SupportedPlatforms { get; }
        IEnumerable<HubType> SupportedHubTypes { get; }
        IWebDriver CreateDriver();
        void SetTimeouts(IWebDriver driver, DriverTimeouts timeouts);

    }

    public class RemoteWebDriverFactoryNew : IDriverFactoryNew
    {
        public RemoteWebDriverFactoryNew()
        {
            SupportedBrowsers = new List<Browser>
            {
                Browser.Chrome, Browser.Firefox,
                Browser.InternetExplorer, Browser.Opera,
                Browser.Edge, Browser.Safari
            };

            SupportedPlatforms = new List<Platform>
            {
                Platform.Windows, Platform.Mac, Platform.Linux
            };

            SupportedHubTypes = new List<HubType>
            {
                HubType.Internal, HubType.BrowserStack,
                HubType.CrossBrowserTesting, HubType.SauceLabs
            };
        }

        public IEnumerable<Browser> SupportedBrowsers { get; }
        public IEnumerable<Platform> SupportedPlatforms { get; }
        public IEnumerable<HubType> SupportedHubTypes { get; }
        public IWebDriver CreateDriver()
        {
            var z = new ChromeOptions();
            var x = new FirefoxOptions();
            var v = new InternetExplorerOptions();
            var dc = new DesiredCapabilities();
            var o =((DesiredCapabilities)v).
            var r = new InternetExplorerDriver(InternetExplorerDriverService.CreateDefaultService("path"), v);

            return new RemoteWebDriver();
        }

        public void SetTimeouts(IWebDriver driver, DriverTimeouts timeouts)
        {
            throw new System.NotImplementedException();
        }
    }
}