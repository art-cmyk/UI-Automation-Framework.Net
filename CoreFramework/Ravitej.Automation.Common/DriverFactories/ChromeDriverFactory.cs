using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Ravitej.Automation.Common.Config.DriverSession;

namespace Ravitej.Automation.Common.DriverFactories
{
    public class ChromeDriverFactory : IDriverFactory
    {
        private readonly ChromeOptions _options;
        private readonly ChromeDriverService _chromeDriverService;

        public ChromeDriverFactory()
        {
            _chromeDriverService = ChromeDriverService.CreateDefaultService("chromedriver.exe");

            //TODO: get the download directory from settings, somehow!
            //TODO: unexpected alert behaviour?
            _options = new ChromeOptions();
            _options.AddUserProfilePreference("browser.cache.disk.enable", false);
            _options.AddUserProfilePreference("browser.startup.homepage", "about:blank");
            _options.AddUserProfilePreference("startup.homepage_override_url", "about:blank");
            _options.AddUserProfilePreference("download.default_directory", "C:");
            _options.AddUserProfilePreference("download.prompt_for_download", false);
            _options.Proxy = new Proxy { IsAutoDetect = true };
            _options.AddExcludedArgument("ignore-certificate-errors");
        }
        public IWebDriver Create(Uri hubUrl, DesiredCapabilities capabilities, TimeSpan commandTimeout)
        {
            //TODO: figure out how to convert DesiredCapabilities into ChromeOptions
            // or may be move this into CapabilityProviders??
            var driver = new ChromeDriver(_chromeDriverService, _options, commandTimeout);
            return driver;
        }

        public IWebDriver Create(Uri hubUrl, DesiredCapabilities capabilities)
        {
            //TODO: figure out how to convert DesiredCapabilities into ChromeOptions
            // or may be move this into CapabilityProviders??
            var driver = new ChromeDriver(_chromeDriverService, _options);
            return driver;
        }

        public IWebDriver SetTimeouts(IWebDriver driver, DriverTimeouts timeouts)
        {
            driver.Manage().Timeouts().ImplicitlyWait(timeouts.ImplicitWait);
            driver.Manage().Timeouts().SetPageLoadTimeout(timeouts.PageLoadTimeout);
            driver.Manage().Timeouts().SetScriptTimeout(timeouts.ScriptTimeout);
            return driver;
        }
    }
}