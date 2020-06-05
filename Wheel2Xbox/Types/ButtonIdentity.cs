using ScpDriverInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wheel2Xbox.Types
{
    public class ButtonIdentity
    {
        public string Name { get; set; }

        public int Index { get; set; }

        public int Value { get; set; }

        public X360Buttons OutputButton { get; set; }
    }
}
