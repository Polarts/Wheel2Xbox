using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SystemTrayIcon.Properties;
using Wheel2Xbox;

namespace SystemTrayIcon
{
    public class ProcessIcon : IDisposable
    {

        NotifyIcon notifyIcon;
        WheelIOManager manager;

        public ProcessIcon()
        {
            notifyIcon = new NotifyIcon();
            manager = WheelIOManager.Create();
        }

        public void Display()
        {
            notifyIcon.MouseClick += new MouseEventHandler(onMouseClicked);
            notifyIcon.Icon = Resources.WheelIcon;
            notifyIcon.Text = "Wheel2Xbox Service";
            notifyIcon.Visible = true;
            notifyIcon.ContextMenuStrip = new ContextMenu().Create();
        }

        private void onMouseClicked(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // TODO display default GUI
            }
        }

        public void Dispose()
        {
            notifyIcon.Dispose();
            manager.Stop();
        }
    }
}
