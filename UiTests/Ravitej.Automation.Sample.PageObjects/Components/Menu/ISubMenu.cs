using System.Collections.Generic;
using Ravitej.Automation.Common.PageObjects.Interactables;
using Ravitej.Automation.Sample.PageObjects.Pages.Base;

namespace Ravitej.Automation.Sample.PageObjects.Components.Menu
{
    public interface ISubMenu : IInteractable
    {
        bool CheckExists(string sSubMenuItemName);
        T Click<T>(string subMenuItemName) where T : IMoonpigBasePage;
        ISubMenuItem Get(string sSubMenuItemName);
        IEnumerable<ISubMenuItem> SubMenuItems { get; }
        IEnumerable<string> SubMenuItemTexts { get; }
    }
}