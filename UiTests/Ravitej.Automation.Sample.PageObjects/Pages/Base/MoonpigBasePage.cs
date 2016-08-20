using Ravitej.Automation.Common.PageObjects.Components.BasePage.Selenium;
using Ravitej.Automation.Common.PageObjects.Session;
using Ravitej.Automation.Sample.PageObjects.Components.Menu;
using Ravitej.Automation.Sample.PageObjects.Components.Search;
using Ravitej.Automation.Sample.PageObjects.Components.TopBar;
using Ravitej.Automation.Sample.PageObjects.Fluent;

namespace Ravitej.Automation.Sample.PageObjects.Pages.Base
{
    public abstract class MoonpigBasePage : BasePage, IMoonpigBasePage
    {
        protected IMoonpigSession MoonpigSession => Session as IMoonpigSession;

        protected MoonpigBasePage(ISession session, IMenu menu, ISearch search, ITopBar topBar)
            : base(session)
        {
            Menu = menu;
            Search = search;
            TopBar = topBar;
        }

        public IMenu Menu { get; }
        public ISearch Search { get; }
        public ITopBar TopBar { get; }
    }
}