using System.Collections.Generic;

namespace Ravitej.Automation.Common.PageObjects.SelfValidation
{
    /// <summary>
    /// Schema for the Dom
    /// </summary>
    public class SelfValidatingPageObjectSchema
    {
        /// <summary>
        /// Constructor for the schema
        /// </summary>
        /// <param name="pageName"></param>
        public SelfValidatingPageObjectSchema(string pageName)
        {
            ExpectedElements = new List<SelfValidatingItem>();
            ActualElements = new List<SelfValidatingItem>();
            KnownExclusionElements = new List<SelfValidatingItem>();
            InvalidElementDetails = new List<string>();
            ChildPageObjectNames = new List<string>();
            PageName = pageName;
            SchemaValid = true;
        }

        /// <summary>
        /// Is the schema syntactically valie
        /// </summary>
        public bool SchemaValid
        {
            get;
            set;
        }

        /// <summary>
        /// Does the schema have child schema obhects
        /// </summary>
        public bool HasChildObjects
        {
            get;
            private set;
        }

        /// <summary>
        /// The names of all child objects
        /// </summary>
        public List<string> ChildPageObjectNames
        {
            get;
            private set;
        }

        /// <summary>
        /// The name of the page
        /// </summary>
        public string PageName
        {
            get;
            private set;
        }

        /// <summary>
        /// List of all emements that are classed as invalid
        /// </summary>
        public List<string> InvalidElementDetails
        {
            get;
            private set;
        }

        /// <summary>
        /// A list of all known items to exclude from the schema
        /// </summary>
        public List<SelfValidatingItem> KnownExclusionElements
        {
            get;
            private set;
        }

        /// <summary>
        /// A list of items expected in the schema
        /// </summary>
        public List<SelfValidatingItem> ExpectedElements
        {
            get;
            private set;
        }

        /// <summary>
        /// A list of the actual elements in the Dom
        /// </summary>
        public List<SelfValidatingItem> ActualElements
        {
            get;
            private set;
        }

        /// <summary>
        /// Append a child object to this one
        /// </summary>
        /// <param name="childSchema"></param>
        public void AppendChild(SelfValidatingPageObjectSchema childSchema)
        {
            HasChildObjects = true;
            ChildPageObjectNames.Add(childSchema.PageName);
            KnownExclusionElements.AddRange(childSchema.KnownExclusionElements);
            ExpectedElements.AddRange(childSchema.ExpectedElements);
            ActualElements.AddRange(childSchema.ActualElements);
        }
    }
}
