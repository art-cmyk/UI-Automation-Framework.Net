using System;
using OpenQA.Selenium;

namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakeTargetLocator : ITargetLocator
    {
        private readonly IWebDriver _fakeWebDriver;

        public FakeTargetLocator(IWebDriver fakeWebDriver)
        {
            _fakeWebDriver = fakeWebDriver;
        }

        public IWebDriver Frame(int frameIndex)
        {
            return _fakeWebDriver;
        }

        public IWebDriver Frame(string frameName)
        {
            return _fakeWebDriver;
        }

        public IWebDriver Frame(IWebElement frameElement)
        {
            return _fakeWebDriver;
        }

        public IWebDriver ParentFrame()
        {
            throw new NotImplementedException();
        }

        public IWebDriver Window(string windowName)
        {
            return _fakeWebDriver;
        }

        public IWebDriver DefaultContent()
        {
            return _fakeWebDriver;
        }

        public IWebElement ActiveElement()
        {
            throw new NotImplementedException();
        }

        public IAlert Alert()
        {
            throw new NotImplementedException();
        }
    }
}
