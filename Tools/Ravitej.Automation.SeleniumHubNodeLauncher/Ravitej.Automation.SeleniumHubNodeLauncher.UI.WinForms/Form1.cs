using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library;
using Ravitej.Automation.SeleniumHubNodeLauncher.Library.CommandLine;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.UI.WinForms
{
    public partial class FrmLauncherMain : Form
    {
        private readonly LauncherOptions _options = new LauncherOptions();
        private readonly List<string> _selectedBrowsers = new List<string>();
        private Hub _hub;
        private Node _node;
        private readonly IProgress<string> _progress;
        private string _networkPath = ConfigurationManager.AppSettings["SeleniumExecutablesPath"];
        private NetworkCredential _networkCredential;
        private bool _filesCopied;
        private const string DefaultHub = "localhost";
        private const string DefaultShowConsole = "yes";
        private const string DefaultMaxSessions = "5";
        private readonly string[] _defaultBrowsersList = { "chrome", "firefox", "ie" };
        private int _hubPort;

        public FrmLauncherMain()
        {
            InitializeComponent();
            _options.LogsLocation = @"C:\Selenium\_Logs";
            _options.ShowConsole = DefaultShowConsole;
            cboMaxSessions.SelectedItem = "5";
            chkStartHub.Checked = true;
            chkStartNode.Checked = true;
            chkDontShowConsole.Checked = false;
            SetDefaultNodeOptions();
            txtOutput.Clear();
            txtAltPath.Clear();
            txtAltUsername.Clear();
            txtAltPassword.Clear();
            txtAltDomain.Clear();
            ToggleCustomNodeControlsGroup();
            btnShutdown.Enabled = false;
            _SetToolTips();
            Action<string> outputAction = s => txtOutput.InvokeEx(t => t.Text += string.Concat(s, Environment.NewLine));
            _progress = new Progress<string>(outputAction);
            _progress.Report(Application.ExecutablePath);
            _progress.Report(string.Format("Detected OS: {0}", OperatingSystemInfo.ProductName));
            Helpers.EncryptConfig(Application.ExecutablePath);
        }

        private void butShutdown_Click(object sender, EventArgs e)
        {
            if (_hub != null || _node != null)
            {
                if (_hub != null)
                {
                    _hub.ShutDown();
                    _hub = null;
                    _hubPort = 0;
                    _progress.Report(string.Concat("Hub shutdown successfully.", Environment.NewLine));
                }
                if (_node != null)
                {
                    _node.ShutDown();
                    _node = null;
                    _progress.Report(string.Concat("Node shutdown successfully.", Environment.NewLine));
                }
                btnShutdown.Enabled = false;
                btnGo.Enabled = true;
                return;
            }
            ProcessHelper.KillProcessByNameAndCurrentUser("java", 0);

            _progress.Report(string.Concat("Local hub and/or node shutdown successfully.", Environment.NewLine));
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
            //Check the system if any selenium processes are running
            //and sync the data file appropriately.
            Helpers.CheckAndSyncData();
            if (txtHubParams.TextLength != 0)
            {
                _options.HubParams = txtHubParams.Text;
            }
            if (chkStartNode.Checked && chkCustomNode.Checked)
            {
                if (_selectedBrowsers.Any())
                {
                    _options.BrowsersList = _selectedBrowsers.ToArray();
                }
                else
                {
                    MessageBox.Show("Please select at least one browser.", "Custom node options");
                    return;
                }
                _options.Hub = txtHub.Text.Trim();
                _options.MaxSessions = string.IsNullOrEmpty(cboMaxSessions.SelectedItem.ToString())
                    ? _options.MaxSessions
                    : cboMaxSessions.SelectedItem.ToString();
            }
            else
            {
                SetDefaultNodeOptions();
            }
            FileDirOperations.CreateRequiredDirectories(_progress, _options.LogsLocation);
            var filesExistLocally = Helpers.SeleniumJarsExistLocally();

            try
            {
                Helpers.DecryptConfig(Application.ExecutablePath);
                var configuration = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                string username = "", password = "", domain = "";
                if (txtAltPath.Text != string.Empty)
                {
                    configuration.AppSettings.Settings["SeleniumExecutablesPath"].Value = txtAltPath.Text;
                }
                if (txtAltUsername.Text != string.Empty)
                {
                    username = txtAltUsername.Text;
                    configuration.AppSettings.Settings["Username"].Value = txtAltUsername.Text;
                }
                if (txtAltPassword.Text != string.Empty)
                {
                    password = txtAltPassword.Text;
                    configuration.AppSettings.Settings["Password"].Value = txtAltPassword.Text;
                }
                if (txtAltDomain.Text != string.Empty)
                {
                    domain = txtAltDomain.Text;
                    configuration.AppSettings.Settings["Domain"].Value = txtAltDomain.Text;
                }
                configuration.Save();
                Helpers.EncryptConfig(Application.ExecutablePath);
                _networkPath = ConfigurationManager.AppSettings["SeleniumExecutablesPath"];
                _networkCredential = new NetworkCredential(username, password, domain);
                Helpers.CopySeleniumJars(_progress, true, _networkPath, _networkCredential);
                _filesCopied = true;
            }
            catch (Win32Exception exception)
            {
                _progress.Report(
                    string.Format(
                        "An error has occurred while trying to copy files. {0}. Executables Path: {1}. Please try again by providing your credentials at the bottom of the app.{2}",
                        exception.Message, _networkPath, Environment.NewLine));
                txtAltPath.Clear();
                txtAltPath.Text = _networkPath;
                if (filesExistLocally)
                {
                    _progress.Report(
                        string.Concat(
                            "Attempting to start the hub and node with previous versions of the files which exist locally.",
                            Environment.NewLine));
                }
            }
            //start the hub if all conditions suit
            if ((_filesCopied || filesExistLocally) && chkStartHub.Checked)
            {
                _hub = Helpers.StartHub(_options, _progress, out _hubPort);
            }
            if ((_filesCopied || filesExistLocally) && chkStartNode.Checked)
            {
                //if localhub not started, set appropriate hubport and hub details
                if (chkStartHub.Checked == false)
                {
                    try
                    {
                        var hubDetails = _options.Hub.Split(':');
                        _hubPort = int.Parse(hubDetails[1]);
                        _options.Hub = hubDetails[0];
                    }
                    catch (Exception exception)
                    {
                        _hubPort = 0;
                    }
                }
                //start the node if all conditions suit
                if (_hubPort != default(int))
                {
                    _node = Helpers.StartNode(_options, _progress, _hubPort);
                }
                else
                {
                    btnGo.Enabled = true;
                    btnShutdown.Enabled = false;
                    _progress.Report("Could not start node as the hub wasn't started or incorrect hub details were given.\r\n" +
                                     "\r\n Tip: When connecting to a remote hub, the hub should be specified in \"<machine name>:<port>\" format. " +
                                     "\r\nFor example, \"bmsplraluru:4444\" or \"localhost:4445\" without the quotes.");
                }
            }
            //change the buttons state to enable/disable the appropriate ones.
            if (_hub != null || _node != null)
            {
                btnGo.Enabled = false;
                btnShutdown.Enabled = true;
            }
        }

        private void chkChrome_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChrome.Checked)
            {
                _selectedBrowsers.Add("chrome");
            }
            else
            {
                if (_selectedBrowsers.Contains("chrome"))
                {
                    _selectedBrowsers.Remove("chrome");
                }
            }
        }

        private void chkFirefox_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFirefox.Checked)
            {
                _selectedBrowsers.Add("firefox");
            }
            else
            {
                if (_selectedBrowsers.Contains("firefox"))
                {
                    _selectedBrowsers.Remove("firefox");
                }
            }
        }

        private void chkIE_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIE.Checked)
            {
                _selectedBrowsers.Add("internet explorer");
            }
            else
            {
                if (_selectedBrowsers.Contains("internet explorer"))
                {
                    _selectedBrowsers.Remove("internet explorer");
                }
            }
        }

        private void chkStartHub_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStartHub.Checked)
            {
                _options.StartHub = "yes";
                chkCustomNode.Checked = false;

            }
            else
            {
                _options.StartHub = "no";
                chkCustomNode.Checked = true;
                MessageBox.Show("Please enter the custom node details to connect to a remote hub.", "Custom Node",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void chkStartNode_CheckedChanged(object sender, EventArgs e)
        {
            _options.StartNode = chkStartNode.Checked ? "yes" : "no";
            chkCustomNode.Enabled = chkStartNode.Checked;
            ToggleCustomNodeControlsGroup();
        }

        private void ToggleCustomNodeControlsGroup()
        {
            var customNodeControlsEnabled = chkCustomNode.Enabled && chkCustomNode.Checked;
            txtHub.Enabled = customNodeControlsEnabled;
            chkChrome.Enabled = customNodeControlsEnabled;
            chkFirefox.Enabled = customNodeControlsEnabled;
            chkIE.Enabled = customNodeControlsEnabled;
            cboMaxSessions.Enabled = customNodeControlsEnabled;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void chkCustomNode_CheckedChanged(object sender, EventArgs e)
        {
            ToggleCustomNodeControlsGroup();
            if (chkCustomNode.Checked == false) SetDefaultNodeOptions();
        }
        private void SetDefaultNodeOptions()
        {
            _options.BrowsersList = _defaultBrowsersList;
            _options.Hub = DefaultHub;
            _options.MaxSessions = DefaultMaxSessions;
        }

        private void _SetToolTips()
        {
            toolTip1.SetToolTip(btnClose, "Exit Launcher");
            toolTip1.SetToolTip(btnGo, "Launch hub and/or node per options selected");
            toolTip1.SetToolTip(chkDontShowConsole,
                "Checking this option prevents the browser from showing the grid console after \r\n" +
                "launching the hub and/or node. You can see the console by going to \r\nhttp://<hub machine name>:<port>/grid/console");
            toolTip1.SetToolTip(txtHubParams, "Additional parameters to start the hub with. Please enter them in JSON format.\r\n" +
                                              "For example: {'timeout' : '60', 'browserTimeout' : '120' }");
            toolTip1.SetToolTip(txtHub, "Specify the hub details here when connecting to a remote hub. It should be specified in \"<machine name>:<port>\" format. " +
                                     "\r\nFor example - \"bmsplraluru:4444\" or \"localhost:4445\" without the quotes.");
        }

        private void chkShowConsole_CheckedChanged(object sender, EventArgs e)
        {
            _options.ShowConsole = chkDontShowConsole.Checked ? "no" : "yes";
        }

        private void btnGo_EnabledChanged(object sender, EventArgs e)
        {
            btnClose.Enabled = btnGo.Enabled;
        }
    }


}
