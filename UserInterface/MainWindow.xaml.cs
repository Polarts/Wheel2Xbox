using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserInterface.Views.Windows;

namespace UserInterface
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            switch(menuItem.Header)
            {
                case "Exit":
                    TaskbarIcon.Visibility = Visibility.Collapsed;
                    Application.Current.Shutdown();
                    break;

                case "Settings":
                    X360ControllerMapWindow controllerMapWindow = new X360ControllerMapWindow();
                    controllerMapWindow.Show();
                    break;
            }
        }
    }
}
