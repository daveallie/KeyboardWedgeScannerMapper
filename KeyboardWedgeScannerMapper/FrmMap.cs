using System.Windows.Forms;

namespace KeyboardWedgeScannerMapper
{
    public partial class FrmMap : Form
    {
        public FrmMap()
        {
            InitializeComponent();
            dgvMap.DataSource = KWSMapper.BarcodeMapList;
            foreach (DataGridViewColumn column in dgvMap.Columns)
            {
                dgvMap.Columns[column.Name].SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }
    }
}
