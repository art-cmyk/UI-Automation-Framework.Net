using System;
using System.Configuration;
using System.IO;
using Ravitej.Automation.Common.Utilities;

namespace Ravitej.Automation.Common.Config
{
    /// <summary>
    /// Class to read the settings from the App.Config
    /// </summary>
    public static class ExecutionSettings
    {
        /// <summary>
        /// Read the settings file path from the application config file
        /// </summary>
        /// <returns></returns>
        public static string SettingsFilePath()
        {
            string targetFolder = AppSetting("TestSettingsFile", "Settings.config");

            var settingFile = new FileInfo(targetFolder);

            if (!Directory.Exists(settingFile.Directory.FullName))
            {
                Directory.CreateDirectory(settingFile.Directory.FullName);
            }

            return targetFolder;
        }

        /// <summary>
        /// Read the settings store path from the application config file
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException">Thrown when the TestSettingsStore value is not set.</exception>
        public static string SettingsStorePath()
        {
            string settingsStorePath = AppSetting("TestSettingsStore");
            if (settingsStorePath != null)
            {
                return settingsStorePath;
            }
            throw new ConfigurationErrorsException("No value was specified for 'TestSettingsStore' in the appication config.");
        }

        /// <summary>
        /// Read the output path from the settings file, using the passed in sub-folder as a child directory
        /// </summary>
        /// <param name="subFolder"></param>
        /// <param name="createIfDoesntExist"></param>
        /// <param name="targetFile"></param>
        /// <returns></returns>
        public static string OutputPath(string subFolder, bool createIfDoesntExist, string targetFile = "")
        {
            string targetFolder = AppSetting("TestOutputPath") ?? string.Empty;

            targetFolder = Path.Combine(targetFolder, subFolder).SanitisePath();

            if (createIfDoesntExist)
            {
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }
            }

            if (!string.IsNullOrWhiteSpace(targetFile))
            {
                targetFolder = Path.Combine(targetFolder, targetFile.SanitiseFilename());
            }

            return targetFolder;
        }

        /// <summary>
        /// Get or Set the current test results path
        /// </summary>
        public static string CurrentTestResultsPath { get; set; }

        private static string AppSetting(string key, string defaultIfNull = null)
        {
            return ConfigurationManager.AppSettings[key] ?? defaultIfNull;
        }

        /// <summary>
        /// Convert the value into a specic Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
