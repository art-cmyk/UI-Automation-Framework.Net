using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Ravitej.Automation.Common.PageObjects.Interactables;
using Ravitej.Automation.Common.PageObjects.Services;
using Ravitej.Automation.Common.PageObjects.Session;
using Ravitej.Automation.Sample.PageObjects.Components.Menu;
using Ravitej.Automation.Sample.PageObjects.Components.Search;
using Ravitej.Automation.Sample.PageObjects.Components.TopBar;
using Ravitej.Automation.Sample.PageObjects.Pages.Base;

namespace Ravitej.Automation.Sample.PageObjects.Pages.CreateAccount
{
    public sealed partial class CreateAcount : MoonpigBasePage, ICreateAcount
    {
        public CreateAcount(ISession session, IMenu menu, ISearch search, ITopBar topBar) : base(session, menu, search, topBar)
        {
            Name = "Create account";
            WaitForPageToBeFullyLoaded(true);
        }

        public override bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            using (var checker = new OnPageChecker(Driver, Name))
            {
                return checker.ElementExistsOnPage(FormContainerBy)
                    .TextExistsOnPage("Create account")
                    .TextExistsOnPage("Create a password")
                    .Confirm(throwWhenNotDisplayed);
            }
        }

        public ICreateAcount SelectTitle(string title)
        {
            SelectByText(WebElementType.HtmlCombo, By.Id("title"), "Title combobox", title);
            return this;
        }

        public ICreateAcount EnterOtherTite(string otherTitle)
        {
            EnterText(OtherTitleTextboxBy, OtherTitleTextboxDescription, otherTitle);
            return this;
        }

        public bool IsOtherTitleDisplayed()
        {
            return IsElementDisplayed(OtherTitleTextboxBy, "Other title textbox");
        }

        public ICreateAcount EnterFirstName(string firstname)
        {
            EnterText(FirstNameTextboxBy, FirstNameTextboxDescription, firstname);
            return this;
        }

        public ICreateAcount EnterSurname(string surname)
        {
            EnterText(SurnameTextboxBy, SurnameTextboxDescription, surname);
            return this;
        }

        public ICreateAcount EnterEmailAddress(string email)
        {
            EnterText(EmailTextboxBy, EmailTextboxDescription, email);
            return this;
        }

        public ICreateAcount EnterPassword(string password)
        {
            EnterText(PasswordTextboxBy, PasswordTextboxDescription, password);
            return this;
        }

        public ICreateAcount ClickShowHidePasswordToggle()
        {
            Click(PasswordToggleBy, PasswordToggleDescription);
            return this;
        }

        public string GetShowHidePasswordToggleText()
        {
            return GetText(PasswordToggleBy, PasswordToggleDescription);
        }

        public ICreateAcount TickReceiveNewsOffers()
        {
            Tick(WebElementType.HtmlCheckbox, NewslettersCheckboxBy, NewslettersCheckboxDescription);
            return this;
        }

        public ICreateAcount UntickReceiveNewsOffers()
        {
            Untick(WebElementType.HtmlCheckbox, NewslettersCheckboxBy, NewslettersCheckboxDescription);
            return this;
        }

        public ICreateAcount TickJoinPartner()
        {
            Tick(WebElementType.HtmlCheckbox, PhotoboxCheckboxBy, PhotoboxCheckboxDescription);
            return this;
        }

        public ICreateAcount UntickJoinPartner()
        {
            Untick(WebElementType.HtmlCheckbox, PhotoboxCheckboxBy, PhotoboxCheckboxDescription);
            return this;
        }

        public T ClickCreateAccount<T>() where T : IInteractable
        {
            Click(CreateAccountButtonBy, CreateAccountButtonDescription);
            return MoonpigSession.OnPage.ResolvePageObjectAndCheck<T>();
        }

        public ISignIn ClickSignIn()
        {
            Click(SignInLinkBy, SignInLinkDescription);
            return MoonpigSession.OnPage.SignIn;
        }

        public ITermsAndConditions ClickTermsAndConditions(out string oldWindowHandle)
        {
            string newWindowHandle;
            Driver.NewWindow(() => Click(TermsLinkBy, TermsLinkDescription), out oldWindowHandle, out newWindowHandle);
            Driver.SwitchTo().Window(newWindowHandle);
            return MoonpigSession.OnPage.TermsAndConditions;
        }

        public int GetValidationErrorsCount()
        {
            return GetElements(ValidationErrorIconsBy, ValidationErrorsIconsDescription).Count();
        }

        public IEnumerable<string> GetValidationErrors()
        {
            return
                GetElementsOrThrow(ValidationMessageSpansBy, ValidationMessageSpansDescription)
                    .Select(span => GetText(span, ValidationMessageSpansDescription))
                    .Where(text => !string.IsNullOrEmpty(text));
        }
    }
}