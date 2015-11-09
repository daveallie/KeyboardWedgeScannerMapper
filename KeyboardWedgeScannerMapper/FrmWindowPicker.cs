using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

using Common;

namespace KeyboardWedgeScannerMapper
{
    public delegate void MouseActionEventHandler(object sender, MouseActionEventArgs e);

    public partial class FrmWindowPicker : Form
    {
        public Window PickedWindow;
        private IntPtr _mouseHookId = IntPtr.Zero;
        private IntPtr _keyboardHookId = IntPtr.Zero;
        private readonly Tool _tool;

        public const int WH_KEYBOARD_LL = 13;
        public const int WH_MOUSE_LL = 14;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        private readonly HookProc _mouseHookProc;

        public event MouseActionEventHandler MouseAction;

        protected virtual void OnMouseAction(MouseActionEventArgs e)
        {
            if (MouseAction != null)
            {
                // Invokes the delegates. 
                MouseAction(this, e);
            }
        }

        public FrmWindowPicker()
        {
            InitializeComponent();
            _tool = new Tool();

            _mouseHookProc = Mouse_HookCallback;
            SetHook(WH_KEYBOARD_LL);

            MouseActionProcess mouseActionProcess = new MouseActionProcess();
            // set local mouse event handler
            MouseAction += mouseActionProcess.MouseAction;
        }

        /// <summary>
        ///  Set global low-level keyboard and mouse event hook
        /// </summary>
        /// <param name="idHook"></param>
        public void SetHook(int idHook)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                switch (idHook)
                {
                    case WH_MOUSE_LL:
                        if (_mouseHookId != IntPtr.Zero) User32.UnhookWindowsHookEx(_mouseHookId);
                        // Call GC.Coolect() to avoid system calling it in hook
                        GC.Collect();
                        // Mouse event hook
                        _mouseHookId = SetWindowsHookEx(idHook, _mouseHookProc,
                            Kernel32.GetModuleHandle(curModule.ModuleName), 0);
                        break;
                }
            }
        }

        /// <summary>
        /// The list keeps suspended windows
        /// </summary>
        private List<IntPtr> _suspendHWnds = new List<IntPtr>();


        /// <summary>
        ///  Mouse event hook callback
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public IntPtr Mouse_HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode < 0) return User32.CallNextHookEx(_mouseHookId, nCode, wParam, lParam);

                POINT point = ((MSLLHOOKSTRUCT) Marshal.PtrToStructure(lParam, typeof (MSLLHOOKSTRUCT))).pt;

                MouseMessages mouseMessage;
                switch ((MouseMessages) wParam)
                {
                    case MouseMessages.WM_LBUTTONDOWN:
                    case MouseMessages.WM_LBUTTONUP:
                        mouseMessage = (MouseMessages) wParam;
                        break;

                    default:
                        // Do not process other events.
                        return User32.CallNextHookEx(_mouseHookId, nCode, wParam, lParam);
                }

                IntPtr hWnd;
                IntPtr hOwner = _tool.getRootOwner(point, out hWnd);

                // The event happend on window picker form, don't process it
                if (hOwner == Handle)
                {
                    // Allow buttons on window picker form to receive clicking immediately.
                    this.Activate();
                    Debug.Print("Ignoring Action!");
                }
                else
                {
                    // Prepare parementers and raise a local mouse event 
                    MouseActionEventArgs e = new MouseActionEventArgs(mouseMessage, point, hWnd);
                    OnMouseAction(e);
                    
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(
                    "Error in private IntPtr Mouse_HookCallback(int nCode, IntPtr wParam, IntPtr lParam): " + e.Message);
            }
            return User32.CallNextHookEx(_mouseHookId, nCode, wParam, lParam);
        }

        /// <summary>
        /// Resume all the suspended windows
        /// </summary>
        private void ResumeWindows()
        {
            foreach (IntPtr hWnd in _suspendHWnds)
            {
                uint thrdId = User32.GetWindowThreadProcessId(hWnd, IntPtr.Zero);
                IntPtr hThrd = Kernel32.OpenThread(Kernel32.ThreadAccess.SUSPEND_RESUME, true, thrdId);
                while (Kernel32.ResumeThread(hThrd) > 0) ;
                bool b = Kernel32.CloseHandle(hThrd);
            }
            _suspendHWnds.Clear();
        }

        private void FrmWindowPicker_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                User32.UnhookWindowsHookEx(_mouseHookId);
            }
            catch
            {
                // ignored
            }
            try
            {
                User32.UnhookWindowsHookEx(_keyboardHookId);
            }
            catch
            {
                // ignored
            }

            ResumeWindows();
        }

        private void StartTracking_Click(object sender, EventArgs e)
        {
            ResumeWindows();
            if (_keyboardHookId != IntPtr.Zero) User32.UnhookWindowsHookEx(_keyboardHookId);
            _keyboardHookId = IntPtr.Zero;

            SetHook(WH_MOUSE_LL);

            StartTracking.Enabled = false;
            StopTracking.Enabled = true;
        }

        public void StopTracking_Click(object sender, EventArgs e)
        {
            if (_mouseHookId != IntPtr.Zero) User32.UnhookWindowsHookEx(_mouseHookId);
            _mouseHookId = IntPtr.Zero;

            SetHook(WH_KEYBOARD_LL);

            StartTracking.Enabled = true;
            StopTracking.Enabled = false;

            if (MouseActionProcess.lastWin != null)
            {
                ControlPaint.DrawReversibleFrame(
                    new Rectangle(MouseActionProcess.lastWin.rect.X, MouseActionProcess.lastWin.rect.Y,
                        MouseActionProcess.lastWin.rect.Width, MouseActionProcess.lastWin.rect.Height),
                    Color.Black,
                    FrameStyle.Thick);
                MouseActionProcess.lastWin = null;
            }
        }

        /// <summary>
        /// Show window information in dataGridView2
        /// </summary>
        /// <param name="win"></param>
        public void ShowWindowProperties(Window win)
        {
            if (win == null) return;

            int n = 0;
            if (win.extendedStyles.Length > 0) n++;
            if (win.hMenu != IntPtr.Zero) n++;
            if (win.itemType != null) n++;
            if (win.owner != IntPtr.Zero) n++;
            if (win.parent != IntPtr.Zero) n++;
            if (win.child != IntPtr.Zero) n++;
            if (win.previous != IntPtr.Zero) n++;
            if (win.next != IntPtr.Zero) n++;
            if (win.itemStrings != null) n += win.itemStrings.Length;
            if (win.itemStyles != null) n += win.itemStyles.Length;
            dgwWindowInfo.Rows.Clear();
            dgwWindowInfo.Refresh();
            dgwWindowInfo.Rows.Add(5+n);

            int i = 0;
            dgwWindowInfo.Rows[i].Cells[0].Value = "HWnd";
            dgwWindowInfo.Rows[i++].Cells[1].Value = ((long)win.hWnd).ToString("D8") + " / " + ((long)win.hWnd).ToString("X8");

            dgwWindowInfo.Rows[i].Cells[0].Value = "Class Name";
            dgwWindowInfo.Rows[i++].Cells[1].Value = win.className;

            dgwWindowInfo.Rows[i].Cells[0].Value = "Text ";
            dgwWindowInfo.Rows[i++].Cells[1].Value = win.text;

            dgwWindowInfo.Rows[i].Cells[0].Value = "Location";
            dgwWindowInfo.Rows[i++].Cells[1].Value = string.Format("{0}, {1}, {2}, {3}", win.rect.X, win.rect.Y, win.rect.Right, win.rect.Bottom);

            dgwWindowInfo.Rows[i].Cells[0].Value = "Size";
            dgwWindowInfo.Rows[i++].Cells[1].Value = string.Format("{0} x {1}", win.rect.Width, win.rect.Height);

            dgwWindowInfo.Rows[i].Cells[0].Value = "Window Style";
            dgwWindowInfo.Rows[i++].Cells[1].Value = win.styles;

            if (win.extendedStyles.Length > 0)
            {
                dgwWindowInfo.Rows[i].Cells[0].Value = "Extended Window Style";
                dgwWindowInfo.Rows[i++].Cells[1].Value = win.extendedStyles;
            }

            if (win.owner != IntPtr.Zero)
            {
                dgwWindowInfo.Rows[i].Cells[1].Value = ((long)win.owner).ToString("D8") + " / " + ((long)win.owner).ToString("X8");
                dgwWindowInfo.Rows[i].Cells[1].Style.ForeColor = Color.Blue;
                dgwWindowInfo.Rows[i++].Cells[0].Value = "Owner HWnd";
            }

            if (win.parent != IntPtr.Zero)
            {
                dgwWindowInfo.Rows[i].Cells[1].Value = ((long)win.parent).ToString("D8") + " / " + ((long)win.parent).ToString("X8");
                dgwWindowInfo.Rows[i].Cells[1].Style.ForeColor = Color.Blue;
                dgwWindowInfo.Rows[i++].Cells[0].Value = "Parent HWnd";
            }

            if (win.child != IntPtr.Zero)
            {
                dgwWindowInfo.Rows[i].Cells[1].Value = ((long)win.child).ToString("D8") + " / " + ((long)win.child).ToString("X8");
                dgwWindowInfo.Rows[i].Cells[1].Style.ForeColor = Color.Blue;
                dgwWindowInfo.Rows[i++].Cells[0].Value = "Child HWnd";
            }
            
            if (win.previous != IntPtr.Zero)
            {
                dgwWindowInfo.Rows[i].Cells[1].Value = ((long)win.previous).ToString("D8") + " / " + ((long)win.previous).ToString("X8");
                dgwWindowInfo.Rows[i].Cells[1].Style.ForeColor = Color.Blue;
                dgwWindowInfo.Rows[i++].Cells[0].Value = "Previous HWnd";
            }
            
            if (win.next != IntPtr.Zero)
            {
                dgwWindowInfo.Rows[i].Cells[1].Value = ((long)win.next).ToString("D8") + " / " + ((long)win.next).ToString("X8");
                dgwWindowInfo.Rows[i].Cells[1].Style.ForeColor = Color.Blue;
                dgwWindowInfo.Rows[i++].Cells[0].Value = "Next HWnd";
            }
            
            if (win.hMenu != IntPtr.Zero)
            {
                dgwWindowInfo.Rows[i].Cells[0].Value = "HMenu";
                dgwWindowInfo.Rows[i++].Cells[1].Value = ((long)win.hMenu).ToString("D8") + " / " + ((long)win.hMenu).ToString("X8");
            }

            if (win.itemType != null)
            {
                dgwWindowInfo.Rows[i].Cells[0].Value = "Type of Items";
                dgwWindowInfo.Rows[i++].Cells[1].Value = win.itemType;
            }

            if (win.itemStrings != null)
            {
                for (int j = 0; j < win.itemStrings.Length; j++)
                {
                    dgwWindowInfo.Rows[i].Cells[0].Value = "Item String " + j.ToString();
                    dgwWindowInfo.Rows[i++].Cells[1].Value = win.itemStrings[j];

                    if (win.itemStyles == null) continue;
                    dgwWindowInfo.Rows[i].Cells[0].Value = "Item Style " + j.ToString();
                    dgwWindowInfo.Rows[i++].Cells[1].Value = win.itemStyles[j];
                }
            }
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_mouseHookId != IntPtr.Zero) SetHook(WH_MOUSE_LL);
        }
    }


    public class MouseActionEventArgs : EventArgs
    {
        public MouseMessages mouseMessage;
        public readonly POINT point;
        public readonly IntPtr hWnd;

        public MouseActionEventArgs(MouseMessages mouseMessage, POINT point, IntPtr hWnd)
        {
            this.mouseMessage = mouseMessage;
            this.point = point;
            this.hWnd = hWnd;
        }
    }

    public class MouseActionProcess
    {
        public static Window lastWin;

        void drawFrame(object win)
        {
            if (Thread.CurrentThread.Name != null) Thread.Sleep(200);

            ControlPaint.DrawReversibleFrame(
                new Rectangle(((Window)win).rect.X, ((Window)win).rect.Y, ((Window)win).rect.Width, ((Window)win).rect.Height),
                Color.Black,
                FrameStyle.Thick);
        }

        // Local mouse action event handler
        public void MouseAction(object sender, MouseActionEventArgs e)
        {
            FrmWindowPicker form = (FrmWindowPicker) sender;

            Window win = new Tool().getWindow(e.hWnd, e.point);
            if (win != null)
            {
                //high light window's fame
                switch (e.mouseMessage)
                {
                    case MouseMessages.WM_LBUTTONDOWN:
                        drawFrame(win);
                        lastWin = win;
                        break;

                    case MouseMessages.WM_LBUTTONUP:
                        drawFrame(lastWin);

                        if (lastWin.hWnd != win.hWnd)
                        {
                            drawFrame(win);
                            Thread thd = new Thread(drawFrame);
                            thd.Name = "delay";
                            thd.Start(win);
                        }
                        lastWin = null;

                        break;
                }

                // Exits the global low level mouse hook, calls GC.Collect() actively, and then reenters a new hook
                form.SetHook(FrmWindowPicker.WH_MOUSE_LL);
                form.ShowWindowProperties(win);
                form.PickedWindow = win;
            }
        }
    }
}