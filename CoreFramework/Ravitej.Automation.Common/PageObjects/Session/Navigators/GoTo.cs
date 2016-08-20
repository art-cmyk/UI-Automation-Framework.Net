using System;
using System.Collections.Generic;
using Ravitej.Automation.Common.Drivers;
using Ravitej.Automation.Common.PageObjects.Interactables;
using Ravitej.Automation.Common.Utilities;

namespace Ravitej.Automation.Common.PageObjects.Session.Navigators
{
    public class GoTo : Navigator
    {
        public GoTo(IDriverSession driverSession)
            : base(driverSession)
        {
        }

        public TReturnPageObject GoToPageAndCheck<TReturnPageObject>(Uri url, Pause.ScriptWaitDuration duration,
            bool throwWhenNotOnPage = true, Dictionary<string, object> arguments = null)
            where TReturnPageObject : IInteractable
        {
            _NavigateTo(url);
            return ResolvePageObjectAndCheck<TReturnPageObject>(duration, throwWhenNotOnPage, arguments);
        }

        public TReturnPageObject GoToPageAndCheck<TReturnPageObject>(Uri url, bool throwWhenNotOnPage = true, Dictionary<string, object> arguments = null) where TReturnPageObject : IInteractable
        {
            _NavigateTo(url);
            return ResolvePageObjectAndCheck<TReturnPageObject>(throwWhenNotOnPage, arguments);
        }

        public TReturnPageObject GoToPage<TReturnPageObject>(Uri url, Dictionary<string, object> arguments = null) where TReturnPageObject : IInteractable
        {
            _NavigateTo(url);
            return ResolvePageObject<TReturnPageObject>(arguments);
        }

        private void _NavigateTo(Uri url)
        {
            DriverSession.Driver.Navigate().GoToUrl(url);
        }
    }
}