using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;

namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakeWebElement : IWebElement
    {
        public IWebElement FindElement(By @by)
        {
            return this;
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void SendKeys(string text)
        {
            throw new NotImplementedException();
        }

        public void Submit()
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
            throw new NotImplementedException();
        }

        public string GetAttribute(string attributeName)
        {
            throw new NotImplementedException();
        }

        public string GetCssValue(string propertyName)
        {
            throw new NotImplementedException();
        }

        public string TagName { get; private set; }
        public string Text { get; private set; }
        public bool Enabled { get; private set; }
        public bool Selected { get; private set; }
        public Point Location { get; private set; }
        public Size Size { get; private set; }
        public bool Displayed { get; private set; }
    }
}