using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Ravitej.Automation.Common.Config.DriverSession;
using Ravitej.Automation.Common.Dependency;
using Ravitej.Automation.Common.Drivers;
using Ravitej.Automation.Common.PageObjects.Components.BasePage;
using Ravitej.Automation.Common.PageObjects.Interactables;
using Ravitej.Automation.Common.Utilities;

namespace Ravitej.Automation.Common.PageObjects.Session.Navigators
{
    public abstract class Navigator : INavigator
    {
        protected Navigator(IDriverSession driverSession)
        {
            DriverSession = driverSession;
        }
        public IDriverSession DriverSession { get; set; }

        private List<Browser> _slowBrowsers;

        protected List<Browser> SlowBrowsers
        {
            get
            {
                return _slowBrowsers ?? (_slowBrowsers = new List<Browser>
                {
                    Browser.InternetExplorer
                });
            }
            set { _slowBrowsers = value; }
        }

        public TReturnPageObject ResolvePageObjectAndCheck<TReturnPageObject>(Pause.ScriptWaitDuration duration,
            bool throwWhenNotOnPage = true, Dictionary<string, object> arguments = null) where TReturnPageObject : IInteractable
        {
            var performPause = true;

            // if the onlyForBrowser object is passed in and contains elements
            if (SlowBrowsers != null && SlowBrowsers.Any())
            {
                // only perform the pause for instances where the current browser is in the only list
                performPause = SlowBrowsers.Any(s => s == DriverSession.SuiteSettings.WebDriverSettings.Browser.EnumValue);
            }

            if (performPause)
            {
                Logging.Logging.WriteVerboseLogEntry(GetType().ToString(), "Performing slow browser pause for WaitDuration: '{0}' to ensure {1} page is fully loaded.", duration, typeof(TReturnPageObject));
                Pause.PauseExecution(duration);
            }

            return ResolvePageObjectAndCheck<TReturnPageObject>(throwWhenNotOnPage, arguments);
        }

        public TReturnPageObject ResolvePageObjectAndCheck<TReturnPageObject>(bool throwWhenNotOnPage = true, Dictionary<string, object> arguments = null) where TReturnPageObject : IInteractable
        {
            // retrieve the page object
            var returnItem = ResolvePageObject<TReturnPageObject>(arguments);
            _CheckAndWait(throwWhenNotOnPage, returnItem);
            return returnItem;
        }

        public TReturnPageObject ResolvePageObject<TReturnPageObject>(Dictionary<string, object> arguments = null) where TReturnPageObject : IInteractable
        {
            // retrieve the page object and return it
            var returnItem = arguments != null ? AppWide.Instance.Resolve<TReturnPageObject>(arguments) : AppWide.Instance.Resolve<TReturnPageObject>();
            return returnItem;
        }

        private void _CheckAndWait<TReturnPageObject>(bool throwWhenNotOnPage, TReturnPageObject returnItem)
            where TReturnPageObject : IInteractable
        {
            var returnItemAsBasePage = returnItem as IBasePage;

            if (returnItemAsBasePage != null && returnItemAsBasePage.IsDisplayed() == false)
            {
                /* ensure that the browser actually has the correct page displayed. 
                 * If this is not the case, wait for pageload timeout specified in 
                 * config to allow for situations like after an IIS reset etc.
                 */
                _WaitForPageLoad(returnItemAsBasePage);
                returnItemAsBasePage.IsDisplayed(throwWhenNotOnPage);
            }
        }

        private void _WaitForPageLoad<T>(T returnItemAsBasePage) where T : IBasePage
        {
            var sw = new Stopwatch();
            sw.Start();
            do
            {
                if (returnItemAsBasePage.IsDisplayed())
                {
                    break;
                }
                Pause.PauseExecution(Pause.ScriptWaitDuration.VeryShort);
            } while (sw.ElapsedMilliseconds < DriverSession.SuiteSettings.WebDriverSettings.PageLoadTimeoutSeconds * 1000);
            sw.Stop();
            Logging.Logging.WriteVerboseLogEntry(GetType().ToString(), "Waited for {0} milliseconds to ensure {1} page was fully loaded.", sw.ElapsedMilliseconds, ((IInteractable)returnItemAsBasePage).Name);
        }
    }
}