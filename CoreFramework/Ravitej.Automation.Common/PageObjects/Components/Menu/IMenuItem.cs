using Ravitej.Automation.Common.PageObjects.Interactables;

namespace Ravitej.Automation.Common.PageObjects.Components.Menu
{
    public interface IMenuItem : IInteractable
    {
        void Click();
        void HoverOver();
        void MoveAway();
    }
}