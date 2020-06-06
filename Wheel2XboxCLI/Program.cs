using System;
using System.Runtime.InteropServices;
using Wheel2Xbox;

namespace Wheel2XboxCLI
{
    class Program
    {

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(SetConsoleCtrlEventHandler handler, bool add);

        // https://msdn.microsoft.com/fr-fr/library/windows/desktop/ms683242.aspx
        private delegate bool SetConsoleCtrlEventHandler(CtrlType sig);

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        static WheelIOManager manager;

        static void Main(string[] args)
        {
            // Register handler for exit signal
            SetConsoleCtrlHandler(CloseSignalHandler, true);

            Console.WriteLine("Initializing wheel I/O manager...");

            manager = WheelIOManager.Create();

            Console.WriteLine("Wheel I/O Manager initialized! Listening to buttons...\n" +
                "Press any button on your wheel to start.\n" +
                "Press enter to stop.");

            Console.ReadLine();
            manager.Stop();
        }

        // Handles cleanup before the program exits
        private static bool CloseSignalHandler(CtrlType signal)
        {
            switch (signal)
            {
                case CtrlType.CTRL_BREAK_EVENT:
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                    Console.WriteLine("Closing");
                    manager.Stop();
                    Environment.Exit(0);
                    return false;

                default:
                    return false;
            }
        }
    }
}
