using System.Collections.Generic;
using Ravitej.Automation.Common.PageObjects.Interactables;
using Ravitej.Automation.Sample.PageObjects.Pages.Base;

namespace Ravitej.Automation.Sample.PageObjects.Pages.CreateAccount
{
    public interface ICreateAcount : IMoonpigBasePage
    {
        ICreateAcount SelectTitle(string title);
        ICreateAcount EnterOtherTite(string otherTitle);
        bool IsOtherTitleDisplayed();
        ICreateAcount EnterFirstName(string firstname);
        ICreateAcount EnterSurname(string surname);
        ICreateAcount EnterEmailAddress(string email);
        ICreateAcount EnterPassword(string password);
        ICreateAcount ClickShowHidePasswordToggle();
        string GetShowHidePasswordToggleText();
        ICreateAcount TickReceiveNewsOffers();
        ICreateAcount UntickReceiveNewsOffers();
        ICreateAcount TickJoinPartner();
        ICreateAcount UntickJoinPartner();
        T ClickCreateAccount<T>() where T : IInteractable;
        ISignIn ClickSignIn();
        ITermsAndConditions ClickTermsAndConditions(out string oldWindowHandle);

        int GetValidationErrorsCount();
        IEnumerable<string> GetValidationErrors();
    }
}