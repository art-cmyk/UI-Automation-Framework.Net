using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library.Data.Entities;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.Library.Data
{
    public class LauncherDataProvider
    {
        private readonly string _file;
        private LauncherData _launcherData;
        private readonly XmlSerializer _serializer;
        private List<LauncherDataProcessesProcess> _processes;
        private List<LauncherDataPortsPort> _ports;

        public LauncherDataProvider()
        {
            _file = "LauncherData.xml";
            _serializer = new XmlSerializer(typeof(LauncherData));
            LoadData();
        }

        private void LoadData()
        {
            using (var streamReader = new StreamReader(_file))
            {
                _launcherData = _serializer.Deserialize(streamReader) as LauncherData;
            }
            if (_launcherData != null)
            {
                _processes =
                    _launcherData.Items.Where(p => p.GetType() == typeof(LauncherDataProcesses))
                        .Cast<LauncherDataProcesses>().First().Processeses;
                _ports =
                    _launcherData.Items.Where(p => p.GetType() == typeof(LauncherDataPorts))
                        .Cast<LauncherDataPorts>().First().Portses;
            }
        }

        private void SaveData()
        {
            _launcherData.Items.Where(p => p.GetType() == typeof(LauncherDataProcesses))
                .Cast<LauncherDataProcesses>().First().Processeses = _processes;
            _launcherData.Items.Where(p => p.GetType() == typeof(LauncherDataPorts))
                    .Cast<LauncherDataPorts>().First().Portses = _ports;
            using (var streamWriter = new StreamWriter(_file))
            {
                _serializer.Serialize(streamWriter, _launcherData);
            }
        }

        #region LauncherData - Generated

        public int GetProcessId(string log)
        {
            LoadData();
            var process = _processes.FirstOrDefault(p => p.LogFile.Equals(log));
            return process != null ? Int32.Parse(process.Id) : -1;
        }

        public IEnumerable<LauncherDataProcessesProcess> GetActiveProcesses()
        {
            LoadData();
            return _processes.Where(p => p.Active.ToLower(CultureInfo.InvariantCulture).Equals("true"));
        }

        public IEnumerable<LauncherDataProcessesProcess> GetActiveHubProcesses()
        {
            LoadData();
            return _processes.Where(p => p.Type.Equals(Type.Hub.ToString()) && p.Active.ToLower(CultureInfo.InvariantCulture).Equals("true"));
        }

        public IEnumerable<LauncherDataProcessesProcess> GetActiveNodeProcesses()
        {
            LoadData();
            return _processes.Where(p => p.Type.Equals(Type.Node.ToString()) && p.Active.ToLower(CultureInfo.InvariantCulture).Equals("true"));
        }

        public void AddProcess(Type type, string log, int processId, int port)
        {
            LoadData();
            _processes
                .Add(new LauncherDataProcessesProcess
                {
                    Active = "true",
                    Id = processId.ToString(CultureInfo.InvariantCulture),
                    LogFile = log,
                    Type = type.ToString(),
                    Port = port.ToString(CultureInfo.InvariantCulture)
                });
            _ports.First(p => p.Number == port.ToString()).Locked = "true";
            SaveData();
        }

        public void RemoveProcess(int processId)
        {
            LoadData();
            var process = _processes.First(p => p.Active.ToLower().Equals("true") && p.Id.Equals(processId.ToString(CultureInfo.InvariantCulture)));
            _processes.Remove(process);
            _ports.First(p => p.Number == process.Port).Locked = "false";
            SaveData();
        }

        public IEnumerable<int> GetBusyPorts(Type type)
        {
            LoadData();
            return _processes.Where(p => p.Type.Equals(type.ToString()) && p.Active.ToLower().Equals("true")).Select(p => p.Port).Cast<int>();
        }

        public IEnumerable<int> GetAllPorts(Type type)
        {
            LoadData();
            return _ports.Where(p => p.Type.Equals(type.ToString())).Select(p => p.Number).Cast<int>();
        }

        public int GetFirstAvailablePort(Type type)
        {
            LoadData();
            var firstOrDefaultAvailablePort =
                _ports.FirstOrDefault(p => p.Type.Equals(type.ToString()) && p.Locked.ToLower(CultureInfo.InvariantCulture) == "false");
            if (firstOrDefaultAvailablePort != null)
            {
                firstOrDefaultAvailablePort.Locked = "true";
                SaveData();
                return Int32.Parse(firstOrDefaultAvailablePort.Number);

            }
            return default(int);
        }

        #endregion

    }
}