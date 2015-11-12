using System;
using System.Runtime.InteropServices;
using System.Text;
using Common;

namespace KeyboardWedgeScannerMapper
{
    class Tool
    {
        public IntPtr getRootOwner(POINT point, out IntPtr hWnd)
        {
            hWnd = User32.WindowFromPoint(point);
            IntPtr hOwner = User32.GetAncestor(hWnd, 3); // GA_ROOTOWNER = 3
            return hOwner;
        }

        public IntPtr getOwner(POINT pt) // for suspend Thread
        {
            try
            {
                IntPtr hWnd = User32.WindowFromPoint(pt);
                return User32.GetAncestor(hWnd, 3);
            }
            catch
            {
                // ignored
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Window getWindow(IntPtr hWnd, POINT pt)
        {
            Window win = null;
            try
            {
                // Get the window under the cursor
                if (hWnd == IntPtr.Zero) hWnd = User32.WindowFromPoint(pt);
                if (hWnd == IntPtr.Zero) return null;

                win = new Window {hWnd = hWnd};

                // Get the rect of window
                bool b = User32.GetWindowRect(hWnd, out win.rect);

                StringBuilder sb = new StringBuilder(128);
                // Get the class name
                User32.GetClassName(hWnd, sb, sb.Capacity);
                win.className = sb.ToString();
                //Console.Write("cls: " + win.clsName);

                // Get the text length
                int n = (int) User32.SendMessage(hWnd, WM.GETTEXTLENGTH, 0, 0);
                if (n > 0)
                {
                    // Get the text of window
                    n = (int) User32.SendMessage(hWnd, WM.GETTEXT, (uint) sb.Capacity, sb);
                    win.text = sb.ToString();
                    //Console.WriteLine("; text: " + win.text);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in public Window getWindow(POINT pt): " + e.Message);
            }
            return win;
        }
    }
}
