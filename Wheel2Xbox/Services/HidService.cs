using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using HidLibrary;
using Wheel2Xbox.Types;

namespace Wheel2Xbox.Services
{
    public delegate void InputReportChangeEventHandler(InputReportChangeEventArgs args);

    public class HidService
    {
        #region Fields

        HidDevice device;

        /// <summary>
        /// A timer that reads data from the HID device once elapsed.
        /// </summary>
        Timer readTimer;

        /// <summary>
        /// Indicates whether the service has already been created.
        /// </summary>
        static bool isCreated = false;

        /// <summary>
        /// Caching the last report read from the device for comparison, as it is required in change detection.
        /// </summary>
        byte[] lastReport;

        #endregion

        #region Events

        /// <summary>
        /// Fires when an input has been successfully received.<br></br>
        /// Args: an array of changes represented as int values.
        /// </summary>
        public event InputReportChangeEventHandler InputReceived;

        #endregion

        #region Ctor and Factory

        public static HidService Create(ushort vendorId = 0x046D, ushort productId = 0xCA04)
        {
            if (isCreated)
                throw new InvalidOperationException("You cannot create more than one HidService");

            return new HidService(vendorId, productId);
        }

        private HidService(ushort vendorId = 0x046D, ushort productId = 0xCA04)
        {
            device = HidDevices.Enumerate(vendorId, productId).ToArray()[0];
            readTimer = new Timer(onReadTimerElapsed, null, 0, 1000 / 60);
        }

        #endregion

        #region Methods

        private void onReadTimerElapsed(object state)
        {
            var input = device.Read();
            if (input.Status == HidDeviceData.ReadStatus.Success
                && input.Data.Length > 0)
            {
                if (lastReport is null)
                    lastReport = input.Data;
                else
                {
                    var changes = input.Data.Zip(lastReport, subtractBytesAsInt);
                    lastReport = input.Data;
                    InputReceived?.Invoke(
                        new InputReportChangeEventArgs 
                        { 
                            Changes = changes, 
                            FullReport = input.Data 
                        }
                    );
                }
            }

        }

        private  int subtractBytesAsInt(byte current, byte last) => current - last;

        #endregion
    }
}
