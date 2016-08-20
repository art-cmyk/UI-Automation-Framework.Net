using Ravitej.Automation.Sample.Ui.Tests.Settings;

namespace Ravitej.Automation.Sample.Ui.Tests.Tests
{
    public class CreateAccountSettings : MoonpigPersistableSettings
    {
        public override void HydrateWithDefaults()
        {
            base.HydrateWithDefaults();
            FirstName = "Ravitej";
            Surname = "Aluru";
            EmailAddress = "ravitej.aluru@gmail.com";
            Title = "Mr";
            Password = "Secret!12";
            InvalidPassword = "Short";
            FirstNameValidationMessage = "Please enter your first name.";
        }

        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string Title { get; set; }
        public string InvalidPassword { get; set; }
        public string FirstNameValidationMessage { get; set; }

    }
}