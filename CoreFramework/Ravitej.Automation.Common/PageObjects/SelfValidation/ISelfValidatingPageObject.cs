using System.Collections.Generic;

namespace Ravitej.Automation.Common.PageObjects.SelfValidation
{
    /// <summary>
    /// Interface identifying that a page can be self validated
    /// </summary>
    public interface ISelfValidatingPageObject
    {
        /// <summary>
        /// Validates whether the displayed page's DOM is according to the structure defined by its page object.
        /// </summary>
        /// <returns></returns>
        bool ValidateDisplayedPage(List<string> elementIdsToExcludeFromDomComparison);
    }
}
