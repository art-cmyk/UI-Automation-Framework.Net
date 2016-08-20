using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Ravitej.Automation.Common.Config
{
    /// <summary>
    /// A self validating item, ensures that the value is within a enumerator type
    /// </summary>
    public class PermittedSettingsValidatingItem<T> where T : struct, IComparable, IFormattable, IConvertible
    {
        private string _value;
        private string _constrainingTypeName;
        private readonly Type _constrainingType;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PermittedSettingsValidatingItem()
        {
            PermittedValues = new List<string>();
            _constrainingType = typeof(T);
            ConstrainingTypeName = _constrainingType.FullName;
        }

        /// <summary>
        /// Constructor and assignment
        /// </summary>
        /// <param name="value"></param>
        public PermittedSettingsValidatingItem(string value)
            : this()
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// During the set, it checks the permitted list to ensure it is valid
        /// </summary>
        public string Value
        {
            get { return _value; }
            set
            {
                // if there any values, do the checking
                if (PermittedValues.Any())
                {
                    if (!PermittedValues.Exists(s => s == value))
                    {
                        throw new ArgumentOutOfRangeException(
                            $"The specified value '{value}' is not permitted for this item.  Please ensure it falls within the PermittedValues list of '{string.Join(",", PermittedValues)}'");
                    }
                }

                _value = value;
            }
        }

        /// <summary>
        /// Gets the enumerator version of the string
        /// </summary>
        [IgnoreDataMember]
        public T EnumValue => ExecutionSettings.ToEnum<T>(Value);

        /// <summary>
        /// The type to be validated against
        /// </summary>
        public string ConstrainingTypeName
        {
            get { return _constrainingTypeName; }
            set
            {
                _constrainingTypeName = value;

                PermittedValues.Clear();

                if (_constrainingType != null)
                {
                    foreach (object enumValue in Enum.GetValues(_constrainingType))
                    {
                        PermittedValues.Add(enumValue.ToString());
                    }

                    // as, during deserialization it is possible to assign the value before the type-name, validate it here.
                    if (!string.IsNullOrWhiteSpace(Value))
                    {
                        if (!PermittedValues.Exists(s => s == Value))
                        {
                            throw new ArgumentOutOfRangeException(
                                $"The specified value '{Value}' is not permitted for this item.  Please ensure it falls within the PermittedValues list");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Values that are valid
        /// </summary>
        [IgnoreDataMember]
        public List<string> PermittedValues
        {
            get; 
            private set;
        }
    }
}
