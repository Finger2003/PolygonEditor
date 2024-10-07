namespace Lab1
{
    partial class PolygonEditor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip = new MenuStrip();
            helpToolStripMenuItem = new ToolStripMenuItem();
            defaultRadioButton = new RadioButton();
            bresenhamRadioButton = new RadioButton();
            drawingPictureBox = new PictureBox();
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)drawingPictureBox).BeginInit();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { helpToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(800, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(57, 20);
            helpToolStripMenuItem.Text = "Pomoc";
            // 
            // defaultRadioButton
            // 
            defaultRadioButton.AutoSize = true;
            defaultRadioButton.Location = new Point(12, 27);
            defaultRadioButton.Name = "defaultRadioButton";
            defaultRadioButton.Size = new Size(142, 19);
            defaultRadioButton.TabIndex = 1;
            defaultRadioButton.TabStop = true;
            defaultRadioButton.Text = "Algorytm biblioteczny";
            defaultRadioButton.UseVisualStyleBackColor = true;
            // 
            // bresenhamRadioButton
            // 
            bresenhamRadioButton.AutoSize = true;
            bresenhamRadioButton.Location = new Point(160, 27);
            bresenhamRadioButton.Name = "bresenhamRadioButton";
            bresenhamRadioButton.Size = new Size(143, 19);
            bresenhamRadioButton.TabIndex = 2;
            bresenhamRadioButton.TabStop = true;
            bresenhamRadioButton.Text = "Algorytm Bresenhama";
            bresenhamRadioButton.UseVisualStyleBackColor = true;
            // 
            // drawingPictureBox
            // 
            drawingPictureBox.Location = new Point(12, 52);
            drawingPictureBox.Name = "drawingPictureBox";
            drawingPictureBox.Size = new Size(776, 386);
            drawingPictureBox.TabIndex = 3;
            drawingPictureBox.TabStop = false;
            drawingPictureBox.Paint += drawingPictureBox_Paint;
            drawingPictureBox.MouseClick += drawingPictureBox_MouseClick;
            drawingPictureBox.MouseMove += drawingPictureBox_MouseMove;
            // 
            // PolygonEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(drawingPictureBox);
            Controls.Add(bresenhamRadioButton);
            Controls.Add(defaultRadioButton);
            Controls.Add(menuStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip;
            Name = "PolygonEditor";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Polygon Editor";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)drawingPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem helpToolStripMenuItem;
        private RadioButton defaultRadioButton;
        private RadioButton bresenhamRadioButton;
        private PictureBox drawingPictureBox;
    }
}
