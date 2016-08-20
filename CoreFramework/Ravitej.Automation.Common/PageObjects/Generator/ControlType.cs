using System.Collections.Generic;

namespace Ravitej.Automation.Common.PageObjects.Generator
{
    /// <summary>
    /// Class holding the details of a given control
    /// </summary>
    public class ControlType
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ControlType()
        {
            Attributes = new Dictionary<string, string>();
        }

        /// <summary>
        /// The Html Id of the control
        /// </summary>
        public string ControlId
        {
            get;
            set;
        }

        /// <summary>
        /// The Human Readable Name of the control
        /// </summary>
        public string ControlName
        {
            get;
            set;
        }

        /// <summary>
        /// The type of control (button, anchor, input)
        /// </summary>
        public string ControlElementType
        {
            get;
            set;
        }

        /// <summary>
        /// All Html attributes the control has
        /// </summary>
        public Dictionary<string, string> Attributes
        {
            get;
            private set;
        }
    }
}
