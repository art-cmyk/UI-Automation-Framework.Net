using Ravitej.Automation.Common.PageObjects.Interactables;

namespace Ravitej.Automation.Common.PageObjects.Components.AlertDialog
{
    /// <summary>
    /// Defines the interface through which the user can manipulate JavaScript alerts.
    /// </summary>
    public interface IAlertDialog : IInteractable
    {
        /// <summary>
        /// Accepts the currently displayed alert.
        /// </summary>
        void AcceptAlert();
        /// <summary>
        /// Accepts the currently displayed alert and returns an object of the type specified by the T parameter.
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> of PageObject to return.</typeparam>
        /// <returns>The PageObject of the page the user is expecting to be on after the alert is accepted.</returns>
        T AcceptAlert<T>() where T : IInteractable;
        /// <summary>
        /// Dismisses the currently displayed alert.
        /// </summary>
        void DismissAlert();

        /// <summary>
        /// Dismisses the currently displayed alert and returns an object of the type specified by the T parameter.
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> of PageObject to return.</typeparam>
        /// <returns>The PageObject of the page the user is expecting to be on after the alert is dismissed.</returns>
        T DismissAlert<T>() where T : IInteractable;
        /// <summary>
        /// Gets the text of the currently displayed alert.
        /// </summary>
        /// <returns>String containing the text of the alert, if an alert is displayed, null otherwise.</returns>
        string GetAlertText();
    }
}
