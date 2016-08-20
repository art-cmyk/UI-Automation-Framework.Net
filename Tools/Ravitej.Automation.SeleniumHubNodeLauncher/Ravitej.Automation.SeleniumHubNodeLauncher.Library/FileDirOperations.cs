using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.VisualBasic.FileIO;
using SearchOption = System.IO.SearchOption;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.Library
{
    public class FileDirOperations
    {
        private readonly string _networkPath;
        private readonly string _destinationDir;
        private readonly NetworkCredential _networkCredential;
        private readonly List<string> _subDirs = new List<string>
        {
            "ServerExecutable",
            "chromedriver_win32",
            "IEDriver_Win32",
            "IEDriver_x64",
        };

        public FileDirOperations(string networkPath, NetworkCredential networkCredential, string destinationDir)
        {
            _networkPath = networkPath;
            _networkCredential = networkCredential;
            _destinationDir = destinationDir;
        }

        public FileDirOperations(string destinationDir)
        {
            _destinationDir = destinationDir;
        }

        public bool CheckSeleniumJarsExistLocally()
        {
            var destinationDir = new DirectoryInfo(_destinationDir);
            return destinationDir.GetFileSystemInfos("selenium-server-standalone", SearchOption.AllDirectories).Any();
        }

        public bool IsLatestVersion()
        {
            try
            {
                return _IsLatestVersion();
            }
            catch (Exception)
            {
                using (new NetworkConnection(_networkPath, _networkCredential))
                {
                    return _IsLatestVersion();
                }
            }

        }

        public void DeleteLocalContents()
        {
            foreach (var subDir in _subDirs)
            {
                string deleteDir = Path.Combine(_destinationDir, subDir);
                if (Directory.Exists(deleteDir))
                {
                    FileSystem.DeleteDirectory(deleteDir, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                }
            }
        }

        public void CopyServerContents(bool showUiDialogs = true)
        {
            try
            {
                _CopyServerContents(showUiDialogs);
            }
            catch (Exception e)
            {
                using (new NetworkConnection(_networkPath, _networkCredential))
                {
                    _CopyServerContents(showUiDialogs);
                }
            }
        }

        private void _CopyServerContents(bool showUiDialogs)
        {
            foreach (var subDir in _subDirs)
            {
                var source = Path.Combine(_networkPath, subDir);
                var destination = Path.Combine(_destinationDir, subDir);
                if (showUiDialogs)
                {
                    FileSystem.CopyDirectory(source, destination, UIOption.AllDialogs, UICancelOption.ThrowException);
                }
                else
                {
                    FileSystem.CopyDirectory(source, destination, true);
                }
            }
        }

        public static void CreateRequiredDirectories(IProgress<string> progress, string logsFolderName)
        {
            progress.Report(string.Format(@"Checking required folder structure exists...{0}", Environment.NewLine));
            if (!Directory.Exists(@"C:\Selenium"))
            {
                progress.Report(string.Format(@"C:\Selenium folder doesn't exist on your machine. Creating it now...{0}", Environment.NewLine));
                Directory.CreateDirectory(@"C:\Selenium");
                progress.Report(string.Format(@"C:\Selenium folder created successfully{0}", Environment.NewLine));
            }
            if (!Directory.Exists(logsFolderName))
            {
                progress.Report(string.Format(@"{0} folder doesn't exist on your machine. Creating it now...{1}", logsFolderName, Environment.NewLine));
                Directory.CreateDirectory(logsFolderName);
                progress.Report(string.Format(@"{0} folder created successfully{1}", logsFolderName, Environment.NewLine));
            }
            progress.Report(string.Format(@"Required folder structure checked successfully{0}", Environment.NewLine));
        }

        private bool _IsLatestVersion()
        {
            // Take a snapshot of the file system.
            var sourceFiles = new List<FileInfo>();
            var destinationFiles = new List<FileInfo>();
            foreach (var subDir in _subDirs)
            {
                var sourceSubDir = new DirectoryInfo(Path.Combine(_networkPath, subDir));
                var destinationSubDir = new DirectoryInfo(Path.Combine(_destinationDir, subDir));
                sourceFiles.AddRange(sourceSubDir.GetFiles());
                if (destinationSubDir.Exists)
                {
                    destinationFiles.AddRange(destinationSubDir.GetFiles());
                }
            }
            var myFileCompare = new FileCompare();
            return sourceFiles.SequenceEqual(destinationFiles, myFileCompare);
        }
    }
}
