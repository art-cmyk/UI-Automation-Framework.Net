using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakeWebDriver : IWebDriver
    {
        public IWebElement FindElement(By @by)
        {
            return new FakeWebElement();
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
        }

        public void Dispose()
        {

        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Quit()
        {

        }

        public IOptions Manage()
        {
            return new FakeOptions();
        }

        public INavigation Navigate()
        {
            throw new NotImplementedException();
        }

        public ITargetLocator SwitchTo()
        {
            return new FakeTargetLocator(this);
        }

        public string Url { get; set; }
        public string Title { get; private set; }
        public string PageSource { get; private set; }
        public string CurrentWindowHandle { get; private set; }
        public ReadOnlyCollection<string> WindowHandles { get; private set; }
    }
}