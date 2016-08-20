using System;

namespace Ravitej.Automation.Common.Config.AppUnderTest
{
    /// <summary>
    /// Represents a user in the application under test (AUT)
    /// </summary>
    [Serializable]
    public class User
    {
        /// <summary>
        /// Fully qualified doamin username to be passed to AUT in case of basic authentication
        /// </summary>
        public string Username { get; private set; }
        
        /// <summary>
        /// Password of the user to be passed to AUT in case of basic authentication
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Initialises a new user with the given username and password
        /// </summary>
        /// <param name="username">username of the user</param>
        /// <param name="password">password of the user</param>
        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
