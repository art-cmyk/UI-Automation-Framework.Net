using OpenQA.Selenium;
using Ravitej.Automation.Common.PageObjects.Services;
using Ravitej.Automation.Common.PageObjects.Session;
using Ravitej.Automation.Sample.PageObjects.Components.Menu;
using Ravitej.Automation.Sample.PageObjects.Components.Search;
using Ravitej.Automation.Sample.PageObjects.Components.TopBar;
using Ravitej.Automation.Sample.PageObjects.Pages.Base;
using Ravitej.Automation.Sample.PageObjects.Pages.CreateAccount;

namespace Ravitej.Automation.Sample.PageObjects.Pages
{
    public class MoonpigHome : MoonpigBasePage, IMoonpigHome
    {
        public MoonpigHome(ISession session, IMenu menu, ISearch search, ITopBar topBar)
            : base(session, menu, search, topBar)
        {
            Name = "Moonpig Home";
        }

        public ICreateAcount ClickCreateAccount()
        {
            Click(By.LinkText("Create account"), "'Create account' link");
            return MoonpigSession.OnPage.CreateAccount;
        }

        public ISignIn ClickSignIn()
        {
            Click(By.LinkText("Sign in"), "'Sign in' link");
            return MoonpigSession.OnPage.SignIn;
        }

        public IHelpDrop HoverOverHelp()
        {
            var helpLink = GetElementOrThrow(By.LinkText("Help"), "'Help' link");
            HoverOver(helpLink, "'Help' link");
            return MoonpigSession.OnPage.ResolvePageObjectAndCheck<IHelpDrop>();
        }

        public override bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            using (var checker = new OnPageChecker(Driver, Name))
            {
                return checker.ElementExistsOnPage(By.CssSelector("div.hero-banner-fullwidth"))
                    .TextExistsOnPage("Why not browse our")
                    .Confirm(throwWhenNotDisplayed);
            }
        }
    }
}