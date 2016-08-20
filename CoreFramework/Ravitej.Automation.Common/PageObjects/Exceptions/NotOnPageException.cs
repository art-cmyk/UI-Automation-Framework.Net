using System;

namespace Ravitej.Automation.Common.PageObjects.Exceptions
{
    /// <summary>
    /// Exception class to be thrown when the current browser page is not the one expected by the page object
    /// </summary>
    public class NotOnPageException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sMessage"></param>
        public NotOnPageException(string sMessage):base(sMessage)
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sMessage"></param>
        /// <param name="innerException"></param>
        public NotOnPageException(string sMessage, Exception innerException) : base(sMessage, innerException)
        {
            
        }
        
    }
}