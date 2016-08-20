using System.Collections.Generic;
using Ravitej.Automation.Common.PageObjects.Interactables;

namespace Ravitej.Automation.Sample.PageObjects.Components.Menu
{
    public interface IMenu
    {
        bool CheckExists(string sMenuItemName);
        ISubMenu HoverOver(string sMenuItemName);
        void MoveAway(string sMenuItemName);
        T Click<T>(string menuItemName) where T : IInteractable;
        IMenuItem Get(string sMenuItemName);
        IMenuItem SelectedItem { get; }
        IEnumerable<IMenuItem> MenuItems { get; }
    }
}