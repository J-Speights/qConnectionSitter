using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qConnectionSitter
{
    public class AppSettings
    {
        public string ExecutablePath { get; set; }
        public List<string> MonitoredInterfaceIds { get; set; } = new List<string>();
        public bool StartMonitorOnLaunch { get; set; }
        public bool StartMinimized { get; set; }
        public bool RunOnStartup { get; set; }
    }
}
