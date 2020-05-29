using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTA_V_Freezer
{
    public partial class Form1 : Form
    {
        public int Id { private set; get; }
        public string Name { private set; get; }
        public bool IsSuspend { private set; get; }
        public bool InTray { private set; get; }
        private ContextMenu m_menu;
        LowLevelKeyboardHook kbh = new LowLevelKeyboardHook();
        public Form1()
        {
            InitializeComponent();
            InTray = true;
            m_menu = new ContextMenu();
            m_menu.MenuItems.Add(0,
                new MenuItem("Выход", new System.EventHandler(Exit_Click)));
            notifyIcon1.ContextMenu = m_menu;
            kbh = new LowLevelKeyboardHook();
            kbh.OnKeyPressed += KeyUp;
            kbh.HookKeyboard();
            this.Resize += new EventHandler(FormForTray_Resize);
            this.pictureBox1.MouseClick += new MouseEventHandler(OpenDiscord);
            this.notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);
        }
        void OpenDiscord(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/vM92p3H");
        }
        void FormForTray_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)//если окно "свернуто"
            {
                Hide();
                // делаем нашу иконку в трее активной
                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipTitle = "GTA V Одиночная сессия";
                notifyIcon1.BalloonTipText = "Программа работает в трее,активация клавишей F10";
                notifyIcon1.ShowBalloonTip(5000);
                InTray = true;
            }
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }
        protected void Exit_Click(Object sender, System.EventArgs e)
        {
            Close();
        }
        private void KeyUp(object sender, Keys e)
        {
            if (!IsSuspend)
            {
                if (e == Keys.F10)
                {
                    foreach (var process in Process.GetProcessesByName("GTA5"))
                    {
                        IsSuspend = true;
                        Suspend(process.Id);
                        timer1.Start();
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("GTA5"))
            {
                IsSuspend = false;
                Resume(process.Id);
                timer1.Stop();
            }
        }
        public void Suspend(int prid)
        {
            WinAPI.SuspendProcess(prid);
            IsSuspend = true;
            System.Media.SoundPlayer sp2 = new System.Media.SoundPlayer(@"../../start.wav");
            sp2.Play();
        }
        public void Resume(int prid)
        {
            WinAPI.ResumeProcess(prid);
            IsSuspend = false;
            System.Media.SoundPlayer sp2 = new System.Media.SoundPlayer(@"../../stop.wav");
            sp2.Play();
        }
    }
}
