using System;
using System.Collections.Generic;

namespace Ravitej.Automation.Common.PageObjects.Exceptions
{
    /// <summary>
    /// Exception thrown when the HTML Dom contains one or more elements that are classed as invalid - generally this is an interactable element that does not have an Id
    /// </summary>
    public class InvalidPageDomStructureException : Exception
    {
        /// <summary>
        /// The name of the page
        /// </summary>
        public string PageName
        {
            get;
            private set;
        }

        /// <summary>
        /// List of elements that are invalid
        /// </summary>
        public IEnumerable<string> InvalidDomElements
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="domElements"></param>
        public InvalidPageDomStructureException(string pageName, IEnumerable<string> domElements)
            : base(string.Concat("Invalid Dom Element(s) Detected On Page: ", pageName))
        {
            PageName = pageName;
            InvalidDomElements = domElements;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="domElements"></param>
        /// <param name="message"></param>
        public InvalidPageDomStructureException(string pageName, IEnumerable<string> domElements, string message)
            : base(message)
        {
            PageName = pageName;
            InvalidDomElements = domElements;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="domElements"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InvalidPageDomStructureException(string pageName, IEnumerable<string> domElements, string message, Exception innerException)
            : base(message, innerException)
        {
            PageName = pageName;
            InvalidDomElements = domElements;
        }
    }
}
