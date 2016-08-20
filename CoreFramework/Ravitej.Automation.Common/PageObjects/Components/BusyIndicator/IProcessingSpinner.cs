using System;
using Ravitej.Automation.Common.PageObjects.Interactables;

namespace Ravitej.Automation.Common.PageObjects.Components.BusyIndicator
{
    /// <summary>
    /// Interface for a processing spinner control
    /// </summary>
    public interface IProcessingSpinner : IInteractable
    {
        /// <summary>
        /// Wait for a duration
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        bool Wait(TimeSpan timeout);
    }
}