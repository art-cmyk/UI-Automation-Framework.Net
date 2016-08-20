using System.Collections.Generic;
using Ravitej.Automation.Common.PageObjects.Interactables;

namespace Ravitej.Automation.Common.PageObjects.Components.SlidingPanel
{
    public interface ISlidingPanel : IInteractable
    {
        /// <summary>
        /// Gets the title of the sliding panel.
        /// </summary>
        /// <returns></returns>
        string GetTitle();
        /// <summary>
        /// Gets the text displayed on each of the buttons of the sliding panel.
        /// </summary>
        /// <returns>IEnumerable containing text of the buttons.</returns>
        IEnumerable<string> GetButtons();

        /// <summary>
        /// Sends "Esc" keypress event to the browser.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Instance of the specified <see cref="IInteractable"/> item.</returns>
        T HitEscapeKey<T>() where T : IInteractable;

        /// <summary>
        /// Waits for a maximum of <paramref name="timeoutSeconds"/> for the overlay element to disappear.
        /// </summary>
        void WaitForOverlayGone(int timeoutSeconds);

        /// <summary>
        /// Waits for a maximum of <paramref name="timeoutSeconds"/> for the panel element to slide back out completely.
        /// </summary>
        /// <exception cref="InvalidOperationException">When <see cref="Interactable.ContentContainer"/> is not defined.</exception>
        void WaitForPanelToSlideBack(int timeoutSeconds);
    }
}