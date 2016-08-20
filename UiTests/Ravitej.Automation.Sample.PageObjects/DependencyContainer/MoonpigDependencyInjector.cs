using Ravitej.Automation.Common.Dependency;
using Ravitej.Automation.Sample.PageObjects.Components.Menu;
using Ravitej.Automation.Sample.PageObjects.Components.Menu.Selenium;
using Ravitej.Automation.Sample.PageObjects.Components.Search;
using Ravitej.Automation.Sample.PageObjects.Components.Search.Selenium;
using Ravitej.Automation.Sample.PageObjects.Components.TopBar;
using Ravitej.Automation.Sample.PageObjects.Components.TopBar.Selenium;
using Ravitej.Automation.Sample.PageObjects.Pages;
using Ravitej.Automation.Sample.PageObjects.Pages.Base;
using Ravitej.Automation.Sample.PageObjects.Pages.CreateAccount;

namespace Ravitej.Automation.Sample.PageObjects.DependencyContainer
{
    public class MoonpigDependencyInjector : DependencyInjector
    {
        /// <summary>
        /// Entry point to perform all page object registrations
        /// </summary>
        protected override void RegisterPageObjects()
        {
            RegisterBasePages();
            AppWide.Instance.RegisterType<IMoonpigHome, MoonpigHome>();
            AppWide.Instance.RegisterType<ICreateAcount, CreateAcount>();
            AppWide.Instance.RegisterType<ISignIn, SignIn>();
            //AppWide.Instance.RegisterType<ITermsAndConditions, TermsAndConditions>();

        }

        private void RegisterBasePages()
        {
            AppWide.Instance.RegisterType<IMoonpigBasePage, MoonpigBasePage>();

        }

        /// <summary>
        /// Entry point to register any explicit site components, such as the BusyIndicator etc.
        /// </summary>
        protected override void RegisterSiteComponents()
        {
            AppWide.Instance.RegisterType<IMenu, Menu>();
            AppWide.Instance.RegisterType<ISearch, Search>();
            AppWide.Instance.RegisterType<ITopBar, TopBar>();
            //AppWide.Instance.RegisterType<IHelpDrop, HelpDrop>();

        }
    }
}
