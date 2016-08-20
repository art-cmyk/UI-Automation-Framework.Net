using System;
using System.Collections.Generic;

namespace Ravitej.Automation.Common.Config.AppUnderTest
{
    /// <summary>
    /// Settings applicable to the application under test (AUT)
    /// </summary>
    [Serializable]
    public class AutSettings : ISelfHydratableSetting, ISelfHydratable<AutSettings>
    {
        /// <summary>
        /// Url of the application under test (AUT)
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Specifies whether the application under test uses basic authentication or not
        /// </summary>
        public bool BasicAuthentication { get; set; }

        /// <summary>
        /// Users' details to use in case of basic authentication
        /// </summary>
        public List<User> Users { get; set; } 
        
        /// <summary>
        /// Elements to exclude from PageObject (expected) to DOM (actual) comparison. 
        /// All elements specified here will always be excluded from the comparison.
        /// </summary>
        public List<string> PageElementsIdsToExcludeFromDomComparison { get; set; }
        
        /// <summary>
        /// Settings used for database actions such as credentials, database servers and database names.
        /// </summary>
        public DatabaseSettings DatabaseSettings { get; set; }

        public AutSettings SelfHydrate()
        {
            var returnVal = new AutSettings()
            {
                BasicAuthentication = false,
                DatabaseSettings = new DatabaseSettings(),
                PageElementsIdsToExcludeFromDomComparison = new List<string>(),
                Url = "http://www.myapplication.under/test",
                Users = new List<User>
                {
                    new User("user1", "password1"),
                    new User("user2", "password2")

                }
            };
            return returnVal;
        }
    }
}
