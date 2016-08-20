using System;

namespace Ravitej.Automation.Common.Utilities
{
    [AttributeUsage(AttributeTargets.All)]
    public class CustomDescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public string SecondaryDescription { get; set; }

        public CustomDescriptionAttribute(string description)
        {
            Description = description;
        }

        public CustomDescriptionAttribute(string description, string secondaryDescription)
        {
            Description = description;
            SecondaryDescription = secondaryDescription;
        }
    }
}