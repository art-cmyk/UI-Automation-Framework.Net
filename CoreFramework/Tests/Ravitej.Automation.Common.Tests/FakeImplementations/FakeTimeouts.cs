using System;
using OpenQA.Selenium;

namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakeTimeouts : ITimeouts
    {
        public ITimeouts ImplicitlyWait(TimeSpan timeToWait)
        {
            return new FakeTimeouts();
        }

        public ITimeouts SetPageLoadTimeout(TimeSpan timeToWait)
        {
            return new FakeTimeouts();
        }

        public ITimeouts SetScriptTimeout(TimeSpan timeToWait)
        {
            return new FakeTimeouts();
        }
    }
}