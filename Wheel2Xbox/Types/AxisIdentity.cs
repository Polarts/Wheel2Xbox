using System;
using System.Collections.Generic;
using System.Text;

namespace Wheel2Xbox.Types
{
    public class AxisIdentity
    {
        public int Index { get; set; }

        /// <summary>
        /// The wheel itself has 4 sectors with max value 255, while pedals only have a single sector.<br></br>
        /// Thus, the sector index can be null.
        /// </summary>
        public int? SectorIndex { get; set; }

        public X360Axis OutputAxis { get; set; }
    }
}
