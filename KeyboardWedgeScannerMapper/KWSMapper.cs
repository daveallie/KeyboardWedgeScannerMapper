using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyboardWedgeScannerMapper
{
    static class KWSMapper
    {
        public static Window PickedWindow = null;
        private const int WM_SETTEXT = 0x000C;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int VK_RETURN = 0x0D;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        public static void SendMessageToInput(string output)
        {
            Debug.Print(output);
            if (PickedWindow != null)
            {
                SendMessage(PickedWindow.hWnd, WM_SETTEXT, 0, output + "\n");
//                SendMessage(PickedWindow.hWnd, WM_KEYDOWN, VK_RETURN, "");
//                SendMessage(PickedWindow.hWnd, WM_KEYUP, VK_RETURN, "");
            }
        }
    }
}
