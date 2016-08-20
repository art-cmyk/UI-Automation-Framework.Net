using System;

namespace Ravitej.Automation.Common.PageObjects.CustomAttributes
{
    /// <summary>
    /// A list of elements to be either included or excluded from the page object validation
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UserInterfaceElementDefinitionSet : Attribute
    {
        /// <summary>
        /// Comma delimited list of items to exclude
        /// </summary>
        public string ExcludeElementIdList
        {
            get;
            set;
        }

        /// <summary>
        /// Comma delimited list of items to include
        /// </summary>
        public string IncludeElementIdList
        {
            get;
            set;
        }
    }
}