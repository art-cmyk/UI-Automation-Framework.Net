using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Ravitej.Automation.Sample.PageObjects.Pages.CreateAccount
{
    public sealed partial class CreateAcount
    {
        private By FormContainerBy => By.CssSelector("ol.form-createAccount");
        private string FormContainerDescription => "Create account form container";

        private By TitleComboboxBy => By.Id("title");
        private string TitleComboboxDescription => "Title combobox";

        private static By OtherTitleTextboxBy => By.Id("otherTitle");
        private static string OtherTitleTextboxDescription => "Other title textbox";

        private static By FirstNameTextboxBy => By.Id("FirstName");
        private static string FirstNameTextboxDescription => "First name textbox";

        private static By SurnameTextboxBy => By.Id("Surname");
        private static string SurnameTextboxDescription => "Surname textbox";

        private static By EmailTextboxBy => By.Id("email");
        private static string EmailTextboxDescription => "Email address textbox";

        private static By PasswordTextboxBy => By.Id("Password");
        private static string PasswordTextboxDescription => "Password textbox";

        private static By PasswordToggleBy => By.CssSelector("a.passwordToggle");
        private static string PasswordToggleDescription => "Show / Hide Password link";

        private static By NewslettersCheckboxBy => By.Id("CheckboxNewsletter");
        private static string NewslettersCheckboxDescription => "Newsletter checkbox";

        private static By PhotoboxCheckboxBy => By.Id("CheckboxPhotobox");
        private static string PhotoboxCheckboxDescription => "Photobox checkbox";

        private static By CreateAccountButtonBy => By.Id("button-createAccount");
        private static string CreateAccountButtonDescription => "Create account button";

        private By SignInLinkBy => new ByChained(FormContainerBy, By.LinkText("Sign in"));
        private static string SignInLinkDescription => "Sign in link next to Create account button";

        private By TermsLinkBy => new ByChained(FormContainerBy, By.LinkText("Terms and Conditions​"));
        private static string TermsLinkDescription => "Terms and Conditions link";

        private By ValidationErrorIconsBy => new ByChained(FormContainerBy, By.CssSelector("span.validation-icon.error"));
        private static string ValidationErrorsIconsDescription => "Validation error icons";

        private By ValidationMessageSpansBy => new ByChained(FormContainerBy, By.CssSelector("span[data-valmsg-for]"));
        private static string ValidationMessageSpansDescription => "Validation error message spans";
            
    }
}