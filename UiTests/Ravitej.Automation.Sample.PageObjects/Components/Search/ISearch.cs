using System.Collections.Generic;
using Ravitej.Automation.Common.PageObjects.Interactables;

namespace Ravitej.Automation.Sample.PageObjects.Components.Search
{
    public interface ISearch : IInteractable
    {
        ISearch EnterSearchTerm(string searchTerm);

        ISearch ClickClearInput();

        ISearch ClickSearch();

        IEnumerable<string> GetSearchSuggestions();
    }
}