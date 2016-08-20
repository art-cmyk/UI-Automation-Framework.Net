using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Ravitej.Automation.Common.Config.DriverSession;

namespace Ravitej.Automation.Common.DriverFactories
{
    public class RemoteWebDriverFactory : IDriverFactory
    {
        public IWebDriver Create(Uri hubUrl, DesiredCapabilities capabilities, TimeSpan commandTimeout)
        {
            var driver = new RemoteWebDriver(hubUrl, capabilities, commandTimeout);
            return driver;
        }

        public IWebDriver Create(Uri hubUrl, DesiredCapabilities capabilities)
        {
            var driver = new RemoteWebDriver(hubUrl, capabilities);
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