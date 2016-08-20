using OpenQA.Selenium;
using Ravitej.Automation.Common.PageObjects.Interactables.Selenium;
using Ravitej.Automation.Common.PageObjects.Session;
using Ravitej.Automation.Sample.PageObjects.Fluent;
using Ravitej.Automation.Sample.PageObjects.Pages;
using Ravitej.Automation.Sample.PageObjects.Pages.CreateAccount;

namespace Ravitej.Automation.Sample.PageObjects.Components.TopBar.Selenium
{
    public class TopBar : Interactable, ITopBar
    {
        public TopBar(ISession session) : base(session)
        {
            Name = "Top Bar";
        }

        public override bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            throw new System.NotImplementedException();
        }

        public ICreateAcount ClickCreateAccount()
        {
            Click(By.LinkText("Create account"), "'Create account' link");
            return ((MoonpigSession)Session).OnPage.CreateAccount;
        }

        public ISignIn ClickSignIn()
        {
            Click(By.LinkText("Sign in"), "'Sign in' link");
            return ((MoonpigSession)Session).OnPage.SignIn;
        }

        public IHelpDrop HoverOverHelp()
        {
            var helpLink = GetElementOrThrow(By.LinkText("Help"), "'Help' link");
            HoverOver(helpLink, "'Help' link");
            return Session.OnPage.ResolvePageObjectAndCheck<IHelpDrop>();
        }
    }
}