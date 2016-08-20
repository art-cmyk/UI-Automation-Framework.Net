using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Ravitej.Automation.Common.PageObjects.Interactables.Selenium;
using Ravitej.Automation.Sample.PageObjects.Fluent;

namespace Ravitej.Automation.Sample.PageObjects.Components.Search.Selenium
{
    public class Search : Interactable, ISearch
    {
        public Search(IMoonpigSession session)
            : base(session)
        {
            Name = "Search";
        }

        public override bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            return IsElementDisplayed(SearchControlsContainerBy, "Search controls container");
        }

        public ISearch EnterSearchTerm(string searchTerm)
        {
            EnterText(SearchInputBy, "Search textbox", searchTerm);
            return this;
        }

        public ISearch ClickClearInput()
        {
            Click(ClearSearchInputBy, "'Clear search textbox' button");
            return this;
        }

        public ISearch ClickSearch()
        {
            Click(SearchButtonBy, "Search button");
            return this;
        }

        public IEnumerable<string> GetSearchSuggestions()
        {
            return
                GetElements(new ByChained(SearchSuggestionsContainerBy, SearchSuggestionsBy), "Search suggestions list")
                    .Select(item => GetText(item, string.Format("Search suggestion")));
        }

        #region Private Element Identifiers

        private static By SearchSuggestionsBy => By.CssSelector("li");

        private static By SearchControlsContainerBy => By.CssSelector("div.search-open");

        private static By SearchInputBy => By.Id("search-box");

        private static By ClearSearchInputBy => By.CssSelector("span.clear-input.active");

        private static By SearchButtonBy => By.Id("search-btn");

        private static By SearchSuggestionsContainerBy => By.CssSelector("ul.list-reset");

        #endregion
    }
}