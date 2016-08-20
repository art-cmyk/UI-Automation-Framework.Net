using System;
using System.IO;
using System.Linq;
using System.Security;
using Microsoft.Win32;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.Library
{
    public static class SysOperations
    {
        /// <exception cref="ApplicationException">Java is either not installed on this machine or its path is not set in System environment variable 'Path'. Cannot proceed without Java.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission to perform this operation.</exception>
        public static string GetJavaPath(IProgress<string> progress)
        {
            var pathSystemEnvironmentVariable = Environment.GetEnvironmentVariable("path",
                EnvironmentVariableTarget.Machine);
            if (pathSystemEnvironmentVariable != null)
            {
                var javaPath = pathSystemEnvironmentVariable.Split(';').FirstOrDefault(s => s.Contains("javapath"));
                if (javaPath == default(string))
                {
                    var pathUserEnvironmentVariable = Environment.GetEnvironmentVariable("path",
                        EnvironmentVariableTarget.User);
                    if (pathUserEnvironmentVariable != null)
                    {
                        javaPath =
                            pathUserEnvironmentVariable.Split(';')
                                .FirstOrDefault(s => s.Contains("java") || s.Contains("Java") || s.Contains("javapath"));
                        if (javaPath == default(string))
                        {
                            _ReportAndThrowException(progress);
                        }
                    }
                    else
                    {
                        _ReportAndThrowException(progress);
                    }

                }
                progress.Report(string.Format("Discovered Java path is: \"{0}\"{1}", javaPath, Environment.NewLine));
                return javaPath;
            }
            return null;
        }

        private static void _ReportAndThrowException(IProgress<string> progress)
        {
            progress.Report(
                string.Format(
                    @"{0}Java is either not installed on this machine or its path is not set in System environment variable 'Path'. Cannot proceed without Java.{0}",
                    Environment.NewLine));
            throw new ApplicationException(
                @"Java is either not installed on this machine or its path is not set in System environment variable 'Path'. Cannot proceed without Java.");
        }

        /// <exception cref="SecurityException">The user does not have the permissions required to read the registry key. </exception>
        /// <exception cref="IOException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value has been marked for deletion. </exception>
        /// <exception cref="UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        //public static string GetIeVersion()
        //{
        //    var ieSubKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer");
        //    return ieSubKey != null ? ieSubKey.GetValue("svcVersion").ToString().Split('.')[0] : null;
        //}

        public static string GetIeVersion()
        {
            string ieVersion =
                Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer").GetValue("Version").ToString();
            string basicVersion = ieVersion.Substring(0, ieVersion.IndexOf('.'));
            string alternateVersion = null;
            string svcieVersion =
                Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer").GetValue("svcVersion") != null
                    ? Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer")
                        .GetValue("svcVersion")
                        .ToString()
                    : null;
            if (string.IsNullOrEmpty(svcieVersion) == false)
            {
                alternateVersion = svcieVersion.Substring(0, svcieVersion.IndexOf('.'));
            }
            return alternateVersion ?? basicVersion;
        }

    }
}
