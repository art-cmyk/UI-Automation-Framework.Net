using System;
using OpenQA.Selenium;

namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakeOptions : IOptions
    {
        public ICookieJar Cookies
        {
            get { throw new NotImplementedException(); }
        }

        public ITimeouts Timeouts()
        {
            return new FakeTimeouts();
        }

        public IWindow Window => new FakeWindow();
        public ILogs Logs
        {
            get { return null; }
        }
    }
}