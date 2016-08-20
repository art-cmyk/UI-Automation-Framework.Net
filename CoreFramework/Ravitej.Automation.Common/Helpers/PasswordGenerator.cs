using System;
using System.Linq;

namespace Ravitej.Automation.Common.Helpers
{
    /// <summary>
    /// Generate a fake password
    /// </summary>
    public static class PasswordGenerator
    {
        private static readonly Random NumberRandom = new Random();

        // define a list of accepted, known characters - both alpha and numeric
        private const string KnownCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        // define a list of known characters but also include special ones
        private const string KnownCharactersWithSpecial = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#!$@";

        // define a list of accepted and known digits - is this necessary, not sure.
        private const string KnownDigits = "0123456789";

        private const string KnownSpecialCharacters = "-#!$@";

        /// <summary>
        /// Generates a random number between the given bounds.
        /// </summary>
        /// <param name="min">lowest bound</param>
        /// <param name="max">highest bound</param>
        /// <returns>Random Number</returns>
        private static int RandomNumber(int min, int max)
        {
            return NumberRandom.Next(min, max);
        }

        /// <summary>
        /// Generate a temporary password
        /// </summary>
        /// <param name="length">How long should the password be</param>
        /// <param name="includeSpecialCharacters">Should it contain at least one special character</param>
        /// <returns>A new password</returns>
        public static string GenerateTemporaryPassword(int length = 8, bool includeSpecialCharacters = false)
        {
            if (length == 0)
            {
                throw new ArgumentException("must be at least 1 character", nameof(length));
            }
            // instantiate an array to hold the new password
            var newPassword = new char[length];

            // maintain a counter of how many items have been successfully added
            int addedCount = 0;

            // a flag to indicate if the password contains at least one digit
            bool hasNumber = false;

            bool hasSpecial = false;

            string passwordRunCharacters = includeSpecialCharacters ? KnownCharactersWithSpecial : KnownCharacters;

            // while the added counter is less than the target length
            while (addedCount < length)
            {
                // get the next character to be in the string from the known list
                char nextCharacter = passwordRunCharacters[RandomNumber(0, KnownCharacters.Length)];

                // if it isn't the first character and, the new character is not the same as its immediate predecessor
                if (addedCount > 0 && newPassword[addedCount - 1] == nextCharacter)
                {
                    continue;
                }

                // if the character is a number
                if (char.IsNumber(nextCharacter))
                {
                    // set the flag to indicate that it does contain at least one numeric value
                    hasNumber = true;
                }

                // if the character is a special one
                if (KnownSpecialCharacters.Contains(nextCharacter))
                {
                    // flag it
                    hasSpecial = true;
                }

                // assign the next element to the new password character
                newPassword[addedCount] = nextCharacter;

                // increment the counter
                addedCount += 1;
            }

            // if the password does not contain a number
            if (!hasNumber)
            {
                // replace the last characters with a randomly picked number from the known list
                newPassword[newPassword.Length - 1] = KnownDigits[RandomNumber(0, KnownDigits.Length)];
            }

            // if I am to include specials but, one has not been included...
            if (includeSpecialCharacters && !hasSpecial)
            {
                // get a new password which is one character shorter then append a special character
                return string.Concat(GenerateTemporaryPassword(length - 1), KnownSpecialCharacters[RandomNumber(0, KnownSpecialCharacters.Length)]);
            }

            // convert eh array into a string and return it to the calling method
            return string.Join(string.Empty, newPassword);
        }
    }
}
