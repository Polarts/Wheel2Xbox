using System;
using System.Linq;
using System.Threading;
using HidLibrary;

namespace HidInputTesterCLI
{
    class Program
    {
        static HidDevice device;

        static void Main(string[] args)
        {
            device = HidDevices.Enumerate(0x04D8, 0xFD0A).ToArray()[0];
            var readTimer = new Timer(onReadTimerElapsed, null, 0, 1000 / 60);
            Console.ReadLine();
        }

        static byte[] lastReport;
        private static void onReadTimerElapsed(object state)
        {
            var input = device.Read();
            if (input.Status == HidDeviceData.ReadStatus.Success
                && input.Data.Length > 0)
            {
                Console.Write("Report: "+string.Join(' ', input.Data)+" | ");

                if (lastReport is null)
                {
                    lastReport = input.Data;
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Changes: " + string.Join(' ', input.Data.Zip(lastReport, (current, last) => current - last)));
                    lastReport = input.Data;
                }

            }

        }
    }
}
