using HidLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Experimentation
{
    class Program
    {
        static List<HIDDeviceInfo> deviceInfos = new List<HIDDeviceInfo>();
        static object lockObj = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Thread[] threads = new[]
            {
                new Thread(() => CheckDevices(0, 13107)),
                new Thread(() => CheckDevices(13108, 13107*2)),
                new Thread(() => CheckDevices((13107*2)+1, 13107*3)),
                new Thread(() => CheckDevices((13107*3)+1, 13107*4)),
                new Thread(() => CheckDevices((13107*4)+1, 13107*5)),
            };
            foreach (var thread in threads)
                thread.Start();

            deviceInfos.ForEach(di => Console.WriteLine($"VID_{di.VID} PID_{di.PID} Is Activated!"));
            Console.ReadLine();
        }

        static void CheckDevices(int start, int end)
        {
            for (int vid = start; vid < end; vid++)
            {
                for (int pid = 0; pid < 0xffff; pid++)
                {
                    var devices = HidDevices.Enumerate(vid, pid).ToArray();
                    if (devices.Length == 0)
                    {
                        Console.WriteLine($"Failed to fetch device VID_{vid:X4} PID_{pid:X4}");
                    }
                    else
                    {
                        Console.WriteLine($"Success! VID_{vid:X4} PID_{pid:X4}");
                        lock (lockObj)
                        {
                            deviceInfos.Add(new HIDDeviceInfo
                            {
                                VID = vid.ToString("X4"),
                                PID = pid.ToString("X4"),
                                IsActive = true,
                                HidDevice = devices[0]
                            });
                        }
                    }
                }
            }
        }
    }
}
