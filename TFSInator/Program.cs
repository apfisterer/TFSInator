using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TFSInator.Properties;

namespace TFSInator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TFSInatorContext());
        }
    }

    public class TFSInatorContext : ApplicationContext
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        
        private NotifyIcon trayIcon;

        public TFSInatorContext()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.Icon1,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("About", About),
                    new MenuItem("Exit", Exit)
            }),
                Visible = true, 
            };

            trayIcon.Text = "TFSInator";
            var HotKeyManager = new HotkeyManager();

            string letter = ConfigurationManager.AppSettings["hotKeyLetter"];
            char c = letter[0];
            int cInt = System.Convert.ToInt16(c);
            if(cInt < 65 || cInt > 90)
            {
                string s = string.Format("Illegal value in App.Config for hotKeyLetter {0}. Must be A - Z", letter);
                throw new ApplicationException(s);
            }

            RegisterHotKey(HotKeyManager.Handle, 123, Constants.CTRL + Constants.ALT, cInt);
        }

        void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        void About(object sender, EventArgs e)
        {
            new AboutBox().Show();
        }

    }
}
