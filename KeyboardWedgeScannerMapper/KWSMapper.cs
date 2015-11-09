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
        public const string BarcodeFile = "barcode_map.dat";
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
        static extern bool SetForegroundWindow(IntPtr hWnd);

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

        public static bool SendMessageToInput(string barcode)
        {
            BarcodeMap barcodeMap = BarcodeMapList.FirstOrDefault(bMap => bMap.Barcode.Equals(barcode.Trim()));
            bool error = false;
            string eMessage = "";

            if (PickedWindow == null)
            {
                error = true;
                eMessage = "No input field has been selected!";
            }

            if (!error && barcodeMap == null)
            {
                error = true;
                eMessage = "No matching barcode in the map!";
            }

            if (error)
            {
                MessageBox.Show(eMessage, "Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                Debug.Print("Sending: " + barcodeMap.PartNumber);
                SetForegroundWindow(PickedWindow.hWnd);

                SendKeys.Send(barcodeMap.PartNumber);
                SendKeys.Send("{ENTER}");
            }

            return !error;
        }
    }
}
