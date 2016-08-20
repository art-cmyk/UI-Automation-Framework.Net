using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Ravitej.Automation.Common.Config.DriverSession;
using Ravitej.Automation.Common.DriverFactories;

namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakeWebDriverFactory : IDriverFactory
    {
        private readonly FakeWebDriver _fakeDriver;

        public FakeWebDriverFactory(FakeWebDriver fakeDriver)
        {
            _fakeDriver = fakeDriver;
        }

        public IWebDriver Create(Uri hubUrl, DesiredCapabilities capabilities, TimeSpan commandTimeout)
        {
            return _fakeDriver;
        }

        public IWebDriver Create(Uri hubUrl, DesiredCapabilities capabilities)
        {
            return _fakeDriver;
        }

        public IWebDriver SetTimeouts(IWebDriver driver, DriverTimeouts timeouts)
        {
            return _fakeDriver;
        }
    }
}