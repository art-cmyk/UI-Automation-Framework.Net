namespace Ravitej.Automation.Common.PageObjects.SelfValidation
{
    /// <summary>
    /// An item to be within the schema
    /// </summary>
    public class SelfValidatingItem
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SelfValidatingItem()
        {
            
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="id"></param>
        public SelfValidatingItem(string id) : this()
        {
            Id = id;
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isNonInteractableElement"></param>
        public SelfValidatingItem(string id, bool isNonInteractableElement) : this(id)
        {
            IsNonInteractableElement = isNonInteractableElement;
        }

        /// <summary>
        /// Html Id of the item
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Is the item classed as non-interactable, as in, DIV, SPAN etc.
        /// </summary>
        public bool IsNonInteractableElement
        {
            get;
            set;
        }
    }
}