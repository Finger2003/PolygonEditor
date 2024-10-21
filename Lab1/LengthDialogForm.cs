using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class LengthDialogForm : Form
    {
        public int Length { get; private set; }
        public LengthDialogForm()
        {
            InitializeComponent();
            AcceptButton = okButton;
            CancelButton = cancelButton;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Length = (int)lengthNumericUpDown.Value;
            DialogResult = DialogResult.OK;
            //Close();
        }

    }
}
