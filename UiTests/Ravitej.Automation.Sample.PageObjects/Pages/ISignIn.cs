
using OpenQA.Selenium;
using Ravitej.Automation.Sample.PageObjects.Components.Menu;
using Ravitej.Automation.Sample.PageObjects.Components.Search;
using Ravitej.Automation.Sample.PageObjects.Components.TopBar;
using Ravitej.Automation.Sample.PageObjects.Fluent;
using Ravitej.Automation.Sample.PageObjects.Pages.Base;

namespace Ravitej.Automation.Sample.PageObjects.Pages
{
    public interface ISignIn : IMoonpigBasePage
    {
        ISignIn EnterUsername(string username);
        ISignIn EnterPassword(string password);
        void ClickSignIn();
    }

    public class SignIn : MoonpigBasePage, ISignIn
    {
        public SignIn(IMoonpigSession session, IMenu menu, ISearch search, ITopBar topBar) : base(session, menu, search, topBar)
        {
        }

        public override bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            throw new System.NotImplementedException();
        }

        public ISignIn EnterUsername(string username)
        {
            EnterText(By.Id("email"), "Email address or username textbox", username);
            return this;
        }

        public ISignIn EnterPassword(string password)
        {
            EnterText(By.Id("password"), "Password textbox", password);
            return this;
        }

        public void ClickSignIn()
        {
            Click(By.Id("signin-button"), "Sign in button");
        }
    }

    public interface ILoggedInHome : IMoonpigBasePage
    {
    }
}