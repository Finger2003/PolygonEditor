namespace Lab1
{
    partial class LengthDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            lengthNumericUpDown = new NumericUpDown();
            okButton = new Button();
            cancelButton = new Button();
            ((System.ComponentModel.ISupportInitialize)lengthNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(125, 15);
            label1.TabIndex = 0;
            label1.Text = "Proszę podać długość:";
            // 
            // lengthNumericUpDown
            // 
            lengthNumericUpDown.Location = new Point(12, 36);
            lengthNumericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            lengthNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            lengthNumericUpDown.Name = "lengthNumericUpDown";
            lengthNumericUpDown.Size = new Size(120, 23);
            lengthNumericUpDown.TabIndex = 1;
            lengthNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.Location = new Point(65, 94);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 2;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.MouseClick += okButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancelButton.Location = new Point(146, 94);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 3;
            cancelButton.Text = "Anuluj";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // LengthDialogForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(233, 129);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(lengthNumericUpDown);
            Controls.Add(label1);
            Name = "LengthDialogForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Długość";
            ((System.ComponentModel.ISupportInitialize)lengthNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private NumericUpDown lengthNumericUpDown;
        private Button okButton;
        private Button cancelButton;
    }
}