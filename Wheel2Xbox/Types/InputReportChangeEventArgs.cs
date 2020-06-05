using System;
using System.Collections.Generic;
using System.Text;

namespace Wheel2Xbox.Types
{
    public class InputReportChangeEventArgs : EventArgs
    {
        public int[] Changes { get; set; }

        public byte[] FullReport { get; set; }
    }
}
