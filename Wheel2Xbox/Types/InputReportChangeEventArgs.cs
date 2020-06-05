using System;
using System.Collections.Generic;
using System.Text;

namespace Wheel2Xbox.Types
{
    public class InputReportChangeEventArgs : EventArgs
    {
        public IEnumerable<int> Changes { get; set; }

        public IEnumerable<byte> FullReport { get; set; }
    }
}
