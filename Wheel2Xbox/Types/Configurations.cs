using System;
using System.Collections.Generic;
using System.Text;

namespace Wheel2Xbox.Types
{
    public class Configurations
    {
        public double SteeringSensitivity { get; set; }

        public Dictionary<string, ButtonIdentity> ButtonIdentities { get; set; }

        public Dictionary<string, AxisIdentity> AxisIdentities { get; set; }
    }
}
