using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileConnector
{

    public class FrontEndGUI
    {
        Form frm;
        Label header;
        Panel panel;

        public void frmNewFormThread()
        {
            Application.Run(frm);
        }

        public FrontEndGUI(string message)
        {
            Application.EnableVisualStyles();
            frm = new Form();
            panel = new Panel();
            panel.AutoSize = true;

            header = new Label();
            header.TextAlign = ContentAlignment.TopCenter;
            header.ForeColor = Color.Red;
            header.Location = new Point(10, 10);
            header.Font = new Font(header.Font.FontFamily, 28);
            header.Text = message;
            header.TextAlign = ContentAlignment.MiddleCenter;
            header.Size = new Size(10, 10);
            header.AutoSize = true;

            int x = (panel.Size.Width - header.Size.Width) / 2;
            header.Location = new Point(x, header.Location.Y);

            panel.Controls.Add(header);
            frm.Controls.Add(panel);

            frm.TopMost = true;
            frm.ControlBox = false;
            frm.BackColor = Color.Black;
            frm.Text = "";
            frm.WindowState = FormWindowState.Maximized;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Bounds = Screen.PrimaryScreen.Bounds;


            // frm.ResumeLayout(false);
            frm.PerformLayout();
        }
    }
}
