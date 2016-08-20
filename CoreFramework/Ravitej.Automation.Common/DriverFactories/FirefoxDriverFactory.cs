using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using Ravitej.Automation.Common.Config.DriverSession;

namespace Ravitej.Automation.Common.DriverFactories
{
    public class FirefoxDriverFactory : IDriverFactory
    {
        private readonly FirefoxOptions _options;
        private readonly FirefoxDriverService _firefoxDriverService;

        public FirefoxDriverFactory()
        {
            _firefoxDriverService = FirefoxDriverService.CreateDefaultService("Firefoxdriver.exe");

            //TODO: get the download directory from settings, somehow!
            //TODO: unexpected alert behaviour?
            _options = new FirefoxOptions();
        }
        public IWebDriver Create(Uri hubUrl, DesiredCapabilities capabilities, TimeSpan commandTimeout)
        {
            //TODO: figure out how to convert DesiredCapabilities into FirefoxOptions
            var driver = new FirefoxDriver(_firefoxDriverService, _options, commandTimeout);
            return driver;
        }

        public IWebDriver Create(Uri hubUrl, DesiredCapabilities capabilities)
        {
            //TODO: figure out how to convert DesiredCapabilities into FirefoxOptions
            var driver = new FirefoxDriver(_firefoxDriverService);
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