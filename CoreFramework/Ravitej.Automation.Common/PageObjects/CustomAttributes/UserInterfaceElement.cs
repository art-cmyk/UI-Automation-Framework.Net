using System;

namespace Ravitej.Automation.Common.PageObjects.CustomAttributes
{
    /// <summary>
    /// Class refering to an actual Html Element within a page
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class UserInterfaceElement : Attribute
    {
        /// <summary>
        /// The Html Id of the element
        /// </summary>
        public string ElementId
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates that the element is not the usual interactable type (like a button, anchor etc.) but is instead a known html element (such as a div, span li etc.) so would not be
        /// picked up in the default framework detection code.
        /// </summary>
        public bool IsNonInteractableElement
        {
            get;
            set;
        }
    }
}
