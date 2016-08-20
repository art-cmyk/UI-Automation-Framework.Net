namespace Ravitej.Automation.Common.PageObjects.Interactables
{
    /// <summary>
    /// Represents any interactable element within the browser's context.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Name of the item. For example, name of the page, modal dialog, sliding panel etc.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether expected page is displayed or not.
        /// </summary>
        /// <param name="throwWhenNotDisplayed">Indicate <see cref="NotOnPageException"/> should be thrown if not on expected item.</param>
        /// <returns>True if control is on expected item. False if <paramref name="throwWhenNotDisplayed"/> is set to false and control is not on expected item.</returns>
        /// <exception cref="NotOnPageException">Thrown when control is not on expected item and <paramref name="throwWhenNotDisplayed"/> is set to true.</exception>
        bool IsDisplayed(bool throwWhenNotDisplayed = false);
    }
}
