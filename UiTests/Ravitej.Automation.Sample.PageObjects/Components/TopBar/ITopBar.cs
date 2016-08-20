using Ravitej.Automation.Common.PageObjects.Interactables;
using Ravitej.Automation.Sample.PageObjects.Pages;
using Ravitej.Automation.Sample.PageObjects.Pages.CreateAccount;

namespace Ravitej.Automation.Sample.PageObjects.Components.TopBar
{
    public interface ITopBar : IInteractable
    {
        ISignIn ClickSignIn();

        ICreateAcount ClickCreateAccount();

        IHelpDrop HoverOverHelp();
    }
}