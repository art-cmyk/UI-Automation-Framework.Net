using System;

namespace Ravitej.Automation.Common.Config.AppUnderTest
{
    /// <summary>
    /// Represents database settings applicable to an application and/or test suite
    /// </summary>
    [Serializable]
    public class DatabaseSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string DatabaseServerInstance
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ApplicationDatabase
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string TestDataDatabase
        {
            get;
            set;
        }
    }
}
