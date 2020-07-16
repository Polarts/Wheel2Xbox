using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Wheel2Xbox;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static WheelIOManager WheelIOManager;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //WheelIOManager = WheelIOManager.Create();
        }
    }
}
