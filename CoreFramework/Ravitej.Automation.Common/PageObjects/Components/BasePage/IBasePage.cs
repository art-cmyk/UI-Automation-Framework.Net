using Ravitej.Automation.Common.PageObjects.Components.AlertDialog;
using Ravitej.Automation.Common.PageObjects.Interactables;

namespace Ravitej.Automation.Common.PageObjects.Components.BasePage
{
    /// <summary>
    /// Defines an interface for all pages in Application Under Test (AUT). 
    /// This is the ultimate base interface of all pages in the AUT; it is the root of the page hierarchy.
    /// </summary>
    public interface IBasePage : IInteractable
    {
        /// <summary>
        /// Gets an instance of IAlertDialog to interact with alerts on the page.
        /// </summary>
        /// <returns>Instance of IAlertDialog.</returns>
        IAlertDialog BrowserAlertDialog
        {
            get;
        }

        /// <summary>
        /// Gets the url of the current page in the browser
        /// </summary>
        /// <returns>String containing the current page's url</returns>
        string CurrentUrl();
    }
}
