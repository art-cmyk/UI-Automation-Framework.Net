using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Ravitej.Automation.Common.Utilities
{
    /// <summary>
    /// Extension methods to work with strings
    /// </summary>
    public static class StringExtensions
    {
        private static readonly Regex InvalidFilenameChars =
            new Regex(
                $@"^(CON|PRN|AUX|NUL|CLOCK\$|COM[1-9]|LPT[1-9])(?=\..|$)|(^(\.‌​+|\s+)$)|((\.+|\s+)$)|([{Regex.Escape(
                    new string(Path.GetInvalidFileNameChars()))}])",
                RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        private static readonly Regex InvalidPathChars = new Regex(
            $"[{Regex.Escape(new string(Path.GetInvalidPathChars()))}]",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        /// <summary>
        /// For the passed in file name, ensure that it does not contain any illegal characters (such as PRN, CON)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string SanitiseFilename(this string filename)
        {
            return InvalidFilenameChars.Replace(filename, "");
        }

        /// <summary>
        /// For the passed in file path, ensure that it does not contain any illegal characters (such as PRN, CON)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string SanitisePath(this string path)
        {
            return InvalidPathChars.Replace(path, "");
        }
    }
}
