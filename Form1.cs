using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HideApplicationToTray
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();

                notifyIcon1.BalloonTipTitle = "Программа была спрятана";
                notifyIcon1.BalloonTipText = "Обратите внимание что программа была спрятана в трей и продолжит свою работу.";
                notifyIcon1.ShowBalloonTip(5000);
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }
    }
}
