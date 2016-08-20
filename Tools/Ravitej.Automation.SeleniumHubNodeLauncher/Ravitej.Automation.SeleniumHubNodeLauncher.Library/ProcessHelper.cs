using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Security;
using System.Security.Principal;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library.Properties;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.Library
{
    public static class ProcessHelper
    {
        /// <exception cref="Win32Exception">There was an error in opening the associated file. </exception>
        /// <exception cref="FileNotFoundException">The PATH environment variable has a string containing quotes.</exception>
        [CanBeNull]
        public static Process StartProcess(string fileName, IProgress<string> progress, string command = null, bool hideWindow = true)
        {
            Process process;
            if (hideWindow)
            {
                var processInfo = new ProcessStartInfo(fileName, command)
                {

                    CreateNoWindow = true,
                    UseShellExecute = false,
                    // *** Redirect the output ***
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true
                };

                process = new Process { StartInfo = processInfo, EnableRaisingEvents = true };
                process.ErrorDataReceived += (sender, args) => progress.Report(args.Data);
                process.OutputDataReceived += (sender, args) => progress.Report(args.Data);
                var started = process.Start();
                progress.Report(string.Format("process started: {0}", started));
                progress.Report(string.Format("process id: {0}", process.Id));
                progress.Report(string.Format("process start info: {0} {1}", process.StartInfo.FileName, process.StartInfo.UserName));
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.Exited += (sender, args) => progress.Report(string.Format("ExitCode: {0}{1}", ((Process)sender).ExitCode, Environment.NewLine));
            }
            else
            {
                var processInfo = new ProcessStartInfo(fileName, command)
                {
                    Arguments = command,
                    FileName = fileName
                };
                process = Process.Start(processInfo);
            }
            return process;
        }

        public static string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            var searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (var o in processList)
            {
                var obj = (ManagementObject)o;
                var argList = new object[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    return argList[1] + "\\" + argList[0];   // return DOMAIN\user
                }
            }
            return "NO OWNER";
        }

        public static bool ProcessExists(string processName, string userName, int hasStartedForSeconds)
        {
            Process[] currentProcesses = Process.GetProcessesByName(processName);
            return currentProcesses.Any(pr =>
            {
                bool timeExpired = (pr.StartTime.AddSeconds(hasStartedForSeconds) < DateTime.Now) || hasStartedForSeconds == 0;
                return GetProcessOwner(pr.Id).Equals(userName) && timeExpired;
            });
        }

        public static bool ProcessExists(int processId, string args)
        {
            Process procById = null;
            bool processExists = false;
            try
            {
                procById = Process.GetProcessById(processId);
            }
            catch (ArgumentException e)
            {
                if (e.Message.Equals(string.Format("Process with an Id of {0} is not running.", processId)))
                {
                    return processExists;
                }
            }
            string wmiQuery = string.Format("Select * From Win32_Process Where ProcessId = {0}", processId);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery);
            ManagementObjectCollection retObjectCollection = searcher.Get();
            foreach (ManagementObject retObject in retObjectCollection)
            {
                if (retObject["CommandLine"].ToString().Contains(args))
                {
                    processExists = true;
                }
            }
            return processExists;
        }

        /// <summary>
        /// Kill processes if they meet the parameter values of process name, owner name, expired started times.
        /// </summary>
        /// <param name="processName">Process Name, case sensitive, for emample "EXCEL" could not be "excel"</param>
        /// <param name="processUserName">Owner name or user name of the process, casse sensitive</param>
        /// <param name="hasStartedForHours">if process has started for more than n (parameter input) hours. 0 means regardless how many hours ago</param>
        public static void KillProcessByNameAndUser(string processName, string processUserName, int hasStartedForHours)
        {
            Process[] foundProcesses = Process.GetProcessesByName(processName);
            foreach (Process p in foundProcesses)
            {
                string userName = GetProcessOwner(p.Id);
                bool prcoessUserNameIsMatched = userName.Equals(processUserName);

                if (processUserName.ToLower() == "all" || prcoessUserNameIsMatched)
                {
                    bool timeExpired = (p.StartTime.AddHours(hasStartedForHours) < DateTime.Now) || hasStartedForHours == 0;
                    if (timeExpired)
                    {
                        p.Kill();
                    }
                }
            }
        }

        /// <exception cref="SecurityException">The caller does not have the correct permissions. </exception>
        public static void KillProcessByNameAndCurrentUser(string processName, int hasStartedForHours)
        {
            var current = WindowsIdentity.GetCurrent();
            if (current != null)
            {
                var currentUserName = current.Name;
                KillProcessByNameAndUser(processName, currentUserName, hasStartedForHours);
            }
        }

        public static List<ManagementObject> GetRunningProcesses(string processName, string args)
        {
            List<ManagementObject> processes = null;
            string wmiQuery = string.Format("Select * From Win32_Process Where CommandLine Like '%{0}%'", args);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery);
            ManagementObjectCollection retObjectCollection = searcher.Get();
            return retObjectCollection.Cast<ManagementObject>().ToList();
        }
    }
}
