using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenQA.Selenium.Support.UI
{
    /// <summary>
    /// Extension methods for select elements
    /// </summary>
    public static class SelectElementExtensions
    {
        /// <summary>
        /// Select the text of the element regardless of case
        /// </summary>
        /// <param name="oSelectElement"></param>
        /// <param name="sSearchText"></param>
        public static void SelectByTextIgnoringCase(this SelectElement oSelectElement, string sSearchText)
        {
            using (IEnumerator<IWebElement> enumerator = (from oOption in oSelectElement.Options
                                                          where oOption.Text.Equals(sSearchText, StringComparison.OrdinalIgnoreCase)
                                                          select oOption).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    IWebElement oOption2 = enumerator.Current;
                    oOption2.Click();
                    return;
                }
            }
            throw new NoSuchElementException(
                $"Could not find <option> with text: {sSearchText} by a case insenstive equals match.");
        }

        /// <summary>
        /// Select the text of the element where it contains the text, case insensitive
        /// </summary>
        /// <param name="oSelectElement"></param>
        /// <param name="sSearchText"></param>
        public static void SelectByTextContainsIgnoringCase(this SelectElement oSelectElement, string sSearchText)
        {
            using (IEnumerator<IWebElement> enumerator = (from oOption in oSelectElement.Options
                                                          where oOption.Text.IndexOf(sSearchText, StringComparison.OrdinalIgnoreCase) >= 0
                                                          select oOption).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    IWebElement oOption2 = enumerator.Current;
                    oOption2.Click();
                    return;
                }
            }
            throw new NoSuchElementException(
                $"Could not find <option> with text: {sSearchText} by a case insenstive contains match.");
        }
    }
}
