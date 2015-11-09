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

            KWSMapper.SwitchAndOutput(txtInput.Text);
            txtInput.Clear();
            e.Handled = true;
        }

        private void btnSelectWindow_Click(object sender, System.EventArgs e)
        {
            FrmWindowPicker f1 = new FrmWindowPicker();
            f1.TopMost = true;
            f1.ShowDialog(this);
            if (f1.PickedWindow == null) return;
            KWSMapper.PickedWindow = f1.PickedWindow;
            lblCurrentWindowTitle.Text = ((long)f1.PickedWindow.hWnd).ToString("D8") + " / " + ((long)f1.PickedWindow.hWnd).ToString("X8");
            txtInput.Enabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }
    }
}
