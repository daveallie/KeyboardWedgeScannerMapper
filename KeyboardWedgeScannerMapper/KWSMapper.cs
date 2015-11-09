using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyboardWedgeScannerMapper
{
    static class KWSMapper
    {
        public static SortableBindingList<BarcodeMap> BarcodeMapList;
        public static Window PickedWindow = null;
        public const string BarcodeFile = "barcode_map.txt";
        private const int WM_SETTEXT = 0x000C;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int VK_RETURN = 0x0D;

        [STAThread]
        static void Main()
        {
            LoadBarcodeMaps();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        public static void LoadBarcodeMaps()
        {
            BarcodeMapList = new SortableBindingList<BarcodeMap>(BarcodeMap.LoadBarcodeMapsFromFile(BarcodeFile));
            DeleteEmptyBarcodeMaps();
        }

        public static void SaveBarcodeMaps()
        {
            DeleteEmptyBarcodeMaps();
            BarcodeMap.SaveBarcodeMapToFile(BarcodeFile, BarcodeMapList.ToList());
        }

        private static void DeleteEmptyBarcodeMaps()
        {
            BarcodeMapList.Where(barcodeMap => barcodeMap.IsEmpty()).ToList().ForEach(barcodeMap => BarcodeMapList.Remove(barcodeMap));
        }

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
