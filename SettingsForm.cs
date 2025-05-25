using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace qConnectionSitter
{
    public partial class SettingsForm : Form
    {
        public AppSettings Settings { get; private set; } = new AppSettings();
        public SettingsForm(AppSettings existingSettings)
        {
            InitializeComponent();

            Settings = existingSettings;

            chkEnableOnStartup.Checked = Settings.StartMonitorOnLaunch;
            chkStartMinimized.Checked = Settings.StartMinimized;
            chkRunOnStartup.Checked = Settings.RunOnStartup;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            Settings.StartMonitorOnLaunch = chkEnableOnStartup.Checked;
            Settings.StartMinimized = chkStartMinimized.Checked;
            Settings.RunOnStartup = chkRunOnStartup.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
