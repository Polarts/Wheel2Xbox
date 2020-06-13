using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemTrayIcon.Properties;

namespace SystemTrayIcon
{
    public class ContextMenu
    {
        public static Color HoverColor = Color.FromArgb(52, 52, 52);

        public ContextMenuStrip Create()
        {
            ContextMenuStrip menu = new ContextMenuStrip
            {
                Renderer = new BlackToolStripRenderer(),
            };

            #region Create menu items

            #region Logs

            ToolStripMenuItem logs = new ToolStripMenuItem
            {
                Text = "Logs",
                Image = Resources.Logs,
            };
            logs.Click += onLogsClicked;
            menu.Items.Add(logs);

            #endregion

            #region Settings

            ToolStripMenuItem settings = new ToolStripMenuItem
            {
                Text = "Settings",
                Image = Resources.Settings,
            };
            settings.Click += onSettingsClicked;
            menu.Items.Add(settings);

            #endregion

            #region About

            ToolStripMenuItem about = new ToolStripMenuItem
            {
                Text = "About",
                Image = Resources.About,
            };
            about.Click += onAboutClicked;
            menu.Items.Add(about);

            #endregion

            #region Exit

            ToolStripMenuItem exit = new ToolStripMenuItem
            {
                Text = "Exit",
                Image = Resources.Exit,
            };
            exit.Click += onExitClicked;
            menu.Items.Add(exit);

            #endregion

            #endregion

            foreach (ToolStripMenuItem item in menu.Items)
            {
                item.BackColor = Color.Black;
                item.ForeColor = Color.White;
                item.MouseEnter += OnItemMouseEnter;
                item.MouseLeave += OnItemMouseLeave;
            }

            return menu;
        }


        #region UI Event Callbacks

        private void OnItemMouseLeave(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            item.BackColor = Color.Black;
        }

        private void OnItemMouseEnter(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            item.BackColor = HoverColor;
        }

        private void onLogsClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void onSettingsClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void onAboutClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void onExitClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion
    }

    public class BlackToolStripRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
            Color c = e.Item.Selected ? ContextMenu.HoverColor : Color.Black;
            using (SolidBrush brush = new SolidBrush(c))
                e.Graphics.FillRectangle(brush, rc);
        }
    }
}
