using System.Windows.Forms;

namespace KeyboardWedgeScannerMapper
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void CheckEnterKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char) Keys.Return) return;

            bool ok = KWSMapper.SendMessageToInput(txtInput.Text);
            if (ok)
            {
                Activate();
            }
            txtInput.Clear();
            e.Handled = true;
        }

        private void btnSelectWindow_Click(object sender, System.EventArgs e)
        {
            FrmWindowPicker frmWindowPicker = new FrmWindowPicker {TopMost = true};
            frmWindowPicker.ShowDialog(this);
            if (frmWindowPicker.PickedWindow == null) return;
            KWSMapper.PickedWindow = frmWindowPicker.PickedWindow;
            lblCurrentWindowTitle.Text = ((long)frmWindowPicker.PickedWindow.hWnd).ToString("D8") + " / " + ((long)frmWindowPicker.PickedWindow.hWnd).ToString("X8");
            txtInput.Enabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void viewToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            FrmMap frmMap = new FrmMap();
            frmMap.ShowDialog(this);

            // Write barcode maps to file
            KWSMapper.SaveBarcodeMaps();
        }
    }
}
