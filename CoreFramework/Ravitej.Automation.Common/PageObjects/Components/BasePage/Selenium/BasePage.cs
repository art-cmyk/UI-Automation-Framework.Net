using Ravitej.Automation.Common.Dependency;
using Ravitej.Automation.Common.PageObjects.Components.AlertDialog;
using Ravitej.Automation.Common.PageObjects.Interactables.Selenium;
using Ravitej.Automation.Common.PageObjects.Session;

namespace Ravitej.Automation.Common.PageObjects.Components.BasePage.Selenium
{
    /// <summary>
    /// Defines all the pages in Application Under Test (AUT) and provides low-level services to derived classes. 
    /// This is the ultimate base class of all pages in the AUT; it is the root of the page hierarchy.
    /// </summary>
    public abstract class BasePage : Interactable, IBasePage
    {
        private IAlertDialog _alertDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BasePage"/> class.
        /// </summary>
        /// <param name="session">Session which drives the page.</param>
        protected BasePage(ISession session) : base(session)
        {
        }

        public IAlertDialog BrowserAlertDialog => _alertDialog ?? (_alertDialog = AppWide.Instance.Resolve<IAlertDialog>());

        public string CurrentUrl()
        {
            return Driver.Url;
        }
    }
}
