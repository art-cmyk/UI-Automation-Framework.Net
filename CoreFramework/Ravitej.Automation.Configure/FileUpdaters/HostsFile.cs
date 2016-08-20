using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ravitej.Automation.Configure.FileUpdaters
{
    public class HostsFile
    {
        public class HostsFileEntry
        {
            public string HostName
            {
                get;
                set;
            }

            public string TargetIpAddress
            {
                get;
                set;
            }
        }

        private string _hostsFilePath;

        public HostsFile()
        {
            _hostsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");
        }

        private string generateHostsEntry(HostsFileEntry entry)
        {
            return string.Concat(entry.TargetIpAddress, "\t", entry.HostName);
        }

        internal void AddHostsEntries(IEnumerable<HostsFileEntry> entriesToWorkWith)
        {
            // read all the lines from the hosts file into memory
            var contents = File.ReadAllLines(_hostsFilePath).ToList();

            var newLines = new List<string>();

            // loop through all the ones that are being asked to be added
            foreach(var entry in entriesToWorkWith)
            {
                string thisEntry = generateHostsEntry(entry);
                // if the entry doesn't exist
                if (contents.Find(s => s.Equals(thisEntry, StringComparison.OrdinalIgnoreCase)) == null)
                {
                    // add it to the list of items to add
                    newLines.Add(thisEntry);
                }
            }

            // if there are items to add
            if (newLines.Any())
            {
                // write the entries to the hosts file
                File.AppendAllLines(_hostsFilePath, newLines);
            }

            // FIN.
        }

        internal void RemoveHostsEntries(IEnumerable<HostsFileEntry> entriesToWorkWith)
        {
            // read all the lines from the hosts file into memory
            var contents = File.ReadAllLines(_hostsFilePath).ToList();

            var newLines = new List<string>();

            var hostsLines = new List<string>();

            foreach (var entry in entriesToWorkWith)
            {
                string thisEntry = generateHostsEntry(entry);
                hostsLines.Add(thisEntry);
            }
            
            foreach (string line in contents)
            {
                if (!hostsLines.Contains(line))
                {
                    newLines.Add(line);
                }
            }

            // if there are items to add
            if (newLines.Any())
            {
                // back up the old file
                File.Move(_hostsFilePath, string.Concat(_hostsFilePath, ".", DateTime.Now.ToFileTime(), ".txt"));

                // write the entries to the hosts file
                File.WriteAllLines(_hostsFilePath, newLines);
            }

            // FIN.
        }
    }
}
