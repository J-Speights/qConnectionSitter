using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace qConnectionSitter
{

    public partial class MainWindow : Form
    {
        private readonly Dictionary<string, string> _monitored = new Dictionary<string, string>();
        private string _executable = null;
        private bool _enabled = false;
        private bool _awaiting = false;
        private bool _up = false;
        private readonly Timer _timer = new Timer();
        private readonly OpenFileDialog _openDialog = new OpenFileDialog();

        private const int TimerIntervalMilliseconds = 500;

        private readonly static Icon _enabledIcon;
        private readonly static Icon _disabledIcon;
        private const string ConfigFilePath = "config.xml";

        private AppSettings _settings = new AppSettings();

        public MainWindow()
        {
            InitializeComponent();
            _openDialog.ShowReadOnly = false;
            _openDialog.CheckFileExists = true;
            _openDialog.Title = "Select Executable File";
            _openDialog.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";
            _openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            var paths = new string[] { @"C:\Program Files (x86)\qBittorrent\qbittorrent.exe", @"C:\Program Files\qBittorrent\qbittorrent.exe" };
            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    _executable = path;
                    txtExecutable.Text = _executable;
                    break;
                }
            }
            nicTray.Icon = _disabledIcon;
            _timer.Tick += Timer_Tick;
            _timer.Interval = TimerIntervalMilliseconds;
            _timer.Enabled = true;
            LoadConfig();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Enabled = false;
            RefreshStatus();
            if (_enabled)
            {
                if (_awaiting && _up)
                {
                    try
                    {
                        var processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(_executable));
                        foreach (var process in processes)
                        {
                            process.Kill();
                            process.WaitForExit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed to stop process: {ex.Message}");
                    }
                    if (!string.IsNullOrEmpty(_executable))
                    {
                        Process.Start(_executable);
                    }
                    _awaiting = false;
                }
                else if (!(_awaiting || _up))
                {
                    _awaiting = true;
                }
            }
            _timer.Enabled = true;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var parameters = new AddConnectionsDialog.Parameters();
            if (AddConnectionsDialog.ShowNewDialog(this, parameters) == DialogResult.OK)
            {
                foreach (var connection in parameters.Connections) _monitored[connection.Id] = connection.Description;
                RefreshConnectionList();
                RefreshStatus();
                RefreshInterface();
                SaveConfig();
            }
        }

        private void RefreshStatus()
        {
            _up = false;
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var intfce in interfaces)
            {
                if (lsvConnections.Items.ContainsKey(intfce.Id))
                {
                    var listItem = lsvConnections.Items[intfce.Id];
                    if (listItem.SubItems[1].Text != intfce.OperationalStatus.ToString()) listItem.SubItems[1].Text = intfce.OperationalStatus.ToString();
                    if (intfce.OperationalStatus == OperationalStatus.Up) _up = true;
                }
            }
        }

        private void RefreshInterface()
        {
            btnStartMonitor.Enabled = ((_executable != null) && (_monitored.Count != 0));
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            var remove = new List<string>();
            foreach (ListViewItem selectedItem in lsvConnections.SelectedItems) remove.Add((string)selectedItem.Tag);
            foreach (var item in remove)
            {
                _monitored.Remove(item);
                lsvConnections.Items.RemoveByKey(item);
            }
            btnRemove.Enabled = false;
            RefreshInterface();
            SaveConfig();
        }

        private void LsvConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = (lsvConnections.SelectedItems.Count != 0);
        }

        private void BtnExecutableBrowse_Click(object sender, EventArgs e)
        {
            if (_executable != null)
            {
                _openDialog.InitialDirectory = Path.GetDirectoryName(_executable);
                _openDialog.FileName = Path.GetFileName(_executable);
            }
            if (_openDialog.ShowDialog(this) == DialogResult.OK)
            {
                _executable = _openDialog.FileName;
                txtExecutable.Text = _executable;
                RefreshInterface();
                SaveConfig();
            }
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            Visible = (WindowState != FormWindowState.Minimized);
        }

        private void NicTray_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Visible = true;
                WindowState = FormWindowState.Normal;
                Activate();
            }
            else
            {
                WindowState = FormWindowState.Minimized;
            }
        }

        private void BtnEnable_Click(object sender, EventArgs e)
        {

        }

        private void SaveConfig()
        {
            var config = new AppSettings
            {
                ExecutablePath = _executable,
                MonitoredInterfaceIds = _monitored.Keys.ToList(),
            };

            try
            {
                using (var stream = new FileStream(ConfigFilePath, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(AppSettings));
                    serializer.Serialize(stream, config);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving config: {ex.Message}");
            }
        }

        private void RefreshConnectionList()
        {
            lsvConnections.Items.Clear();
            var items = _monitored.OrderBy(kvp => kvp.Value);
            foreach (var item in items)
            {
                var listItem = lsvConnections.Items.Add(item.Key, item.Value, -1);
                listItem.Tag = item.Key;
                listItem.SubItems.Add("");
            }
        }

        private void LoadConfig()
        {
            if (!File.Exists(ConfigFilePath)) return;

            try
            {
                using (var stream = new FileStream(ConfigFilePath, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(AppSettings));
                    var config = (AppSettings)serializer.Deserialize(stream);
                    _settings = new AppSettings
                    {
                        StartMonitorOnLaunch = config.StartMonitorOnLaunch,
                        StartMinimized = config.StartMinimized,
                        RunOnStartup = config.RunOnStartup,
                    };

                    _executable = config.ExecutablePath;
                    txtExecutable.Text = _executable;

                    _monitored.Clear();

                    var interfaces = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (var id in config.MonitoredInterfaceIds)
                    {
                        var match = interfaces.FirstOrDefault(nic => nic.Id == id);
                        string name = match != null ? match.Name : "(Unknown Name)";
                        _monitored[id] = name;
                    }

                    RefreshConnectionList();
                    RefreshStatus();
                    RefreshInterface();

                    if (config.StartMonitorOnLaunch)
                    {
                        BtnEnable_Click(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Loading config: {ex.Message}");
            }
        }

        static MainWindow()
        {
            _enabledIcon = CreateTrayIcon(Color.Green, "q");
            _disabledIcon = CreateTrayIcon(Color.DarkRed, "q");
        }

        private static Icon CreateTrayIcon(Color color, string text)
        {
            using (var bitmap = new Bitmap(16, 16, PixelFormat.Format32bppArgb))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.FillRectangle(new SolidBrush(color), 0, 0, 16, 16);
                    graphics.DrawRectangle(Pens.White, 0, 0, 16, 16);
                    graphics.DrawString(text, SystemFonts.MenuFont, Brushes.White, 3, -3);
                }

                return Icon.FromHandle(bitmap.GetHicon());
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveConfig();
            base.OnFormClosing(e);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lblExecutableText_Click(object sender, EventArgs e)
        {

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void BtnAdvanced_Click(object sender, EventArgs e)
        {
            var currentSettings = new AppSettings
            {
                //StartMonitorOnLaunch = chkEnableOnStartup.Checked
            };

        }

        private void BtnStartMonitor_Click(object sender, EventArgs e)
        {

        }
    }


}
