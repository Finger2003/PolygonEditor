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
        public void SetStartingValue(int value)
        {
            lengthNumericUpDown.Value = value;
        }

    }
}
