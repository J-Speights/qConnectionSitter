using System;
using System.Collections.Generic;

namespace qConnectionSitter
{
    [Serializable]
    public class AppConfig
    {
        public string ExecutablePath { get; set; }
        public List<string> MonitoredInterfaceIds { get; set; } = new List<string>();
        public bool EnableAtAppLaunch { get; set; } = false;
    }
}
