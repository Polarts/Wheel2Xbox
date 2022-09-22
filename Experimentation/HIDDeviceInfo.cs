using HidLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimentation
{
    public class HIDDeviceInfo
    {
        public string VID { get; set; }
        public string PID { get; set; }

        public bool IsActive { get; set; }
        public string InactivityReason { get; set; }
        public HidDevice HidDevice { get; set; }
    }
}
