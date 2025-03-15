namespace PolygonEditor
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
            components = new System.ComponentModel.Container();
            menuStrip = new MenuStrip();
            helpToolStripMenuItem = new ToolStripMenuItem();
            defaultRadioButton = new RadioButton();
            bresenhamRadioButton = new RadioButton();
            drawingPictureBox = new PictureBox();
            edgesContextMenuStrip = new ContextMenuStrip(components);
            addVertexToolStripMenuItem = new ToolStripMenuItem();
            addConstraintToolStripMenuItem = new ToolStripMenuItem();
            fixedLengthToolStripMenuItem = new ToolStripMenuItem();
            verticalToolStripMenuItem = new ToolStripMenuItem();
            horizontalToolStripMenuItem = new ToolStripMenuItem();
            bezierToolStripMenuItem = new ToolStripMenuItem();
            removeConstraintToolStripMenuItem = new ToolStripMenuItem();
            verticesContextMenuStrip = new ContextMenuStrip(components);
            deleteVertexToolStripMenuItem = new ToolStripMenuItem();
            setContinuityInVertexToolStripMenuItem = new ToolStripMenuItem();
            g0ToolStripMenuItem = new ToolStripMenuItem();
            g1ToolStripMenuItem = new ToolStripMenuItem();
            c1ToolStripMenuItem = new ToolStripMenuItem();
            clearButton = new Button();
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)drawingPictureBox).BeginInit();
            edgesContextMenuStrip.SuspendLayout();
            verticesContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { helpToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(784, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(57, 20);
            helpToolStripMenuItem.Text = "Pomoc";
            helpToolStripMenuItem.Click += helpToolStripMenuItem_Click;
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
            defaultRadioButton.CheckedChanged += defaultRadioButton_CheckedChanged;
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
            bresenhamRadioButton.CheckedChanged += bresenhamRadioButton_CheckedChanged;
            // 
            // drawingPictureBox
            // 
            drawingPictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            drawingPictureBox.Location = new Point(12, 52);
            drawingPictureBox.Name = "drawingPictureBox";
            drawingPictureBox.Size = new Size(760, 397);
            drawingPictureBox.TabIndex = 3;
            drawingPictureBox.TabStop = false;
            drawingPictureBox.SizeChanged += drawingPictureBox_SizeChanged;
            drawingPictureBox.Paint += drawingPictureBox_Paint;
            drawingPictureBox.MouseClick += drawingPictureBox_MouseClick;
            drawingPictureBox.MouseDown += drawingPictureBox_MouseDown;
            drawingPictureBox.MouseMove += drawingPictureBox_MouseMove;
            drawingPictureBox.MouseUp += drawingPictureBox_MouseUp;
            // 
            // edgesContextMenuStrip
            // 
            edgesContextMenuStrip.Items.AddRange(new ToolStripItem[] { addVertexToolStripMenuItem, addConstraintToolStripMenuItem, removeConstraintToolStripMenuItem });
            edgesContextMenuStrip.Name = "contextMenuStrip1";
            edgesContextMenuStrip.Size = new Size(185, 70);
            // 
            // addVertexToolStripMenuItem
            // 
            addVertexToolStripMenuItem.Name = "addVertexToolStripMenuItem";
            addVertexToolStripMenuItem.Size = new Size(184, 22);
            addVertexToolStripMenuItem.Text = "Dodaj wierzchołek";
            addVertexToolStripMenuItem.Click += addVertexToolStripMenuItem_Click;
            // 
            // addConstraintToolStripMenuItem
            // 
            addConstraintToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { fixedLengthToolStripMenuItem, verticalToolStripMenuItem, horizontalToolStripMenuItem, bezierToolStripMenuItem });
            addConstraintToolStripMenuItem.Name = "addConstraintToolStripMenuItem";
            addConstraintToolStripMenuItem.Size = new Size(184, 22);
            addConstraintToolStripMenuItem.Text = "Dodaj ograniczenie...";
            // 
            // fixedLengthToolStripMenuItem
            // 
            fixedLengthToolStripMenuItem.Name = "fixedLengthToolStripMenuItem";
            fixedLengthToolStripMenuItem.Size = new Size(144, 22);
            fixedLengthToolStripMenuItem.Text = "Stała długość";
            fixedLengthToolStripMenuItem.Click += fixedLengthToolStripMenuItem_Click;
            // 
            // verticalToolStripMenuItem
            // 
            verticalToolStripMenuItem.Name = "verticalToolStripMenuItem";
            verticalToolStripMenuItem.Size = new Size(144, 22);
            verticalToolStripMenuItem.Text = "Pionowa";
            verticalToolStripMenuItem.Click += verticalToolStripMenuItem_Click;
            // 
            // horizontalToolStripMenuItem
            // 
            horizontalToolStripMenuItem.Name = "horizontalToolStripMenuItem";
            horizontalToolStripMenuItem.Size = new Size(144, 22);
            horizontalToolStripMenuItem.Text = "Pozioma";
            horizontalToolStripMenuItem.Click += horizontalToolStripMenuItem_Click;
            // 
            // bezierToolStripMenuItem
            // 
            bezierToolStripMenuItem.Name = "bezierToolStripMenuItem";
            bezierToolStripMenuItem.Size = new Size(144, 22);
            bezierToolStripMenuItem.Text = "Bézier";
            bezierToolStripMenuItem.Click += bezierToolStripMenuItem_Click;
            // 
            // removeConstraintToolStripMenuItem
            // 
            removeConstraintToolStripMenuItem.Name = "removeConstraintToolStripMenuItem";
            removeConstraintToolStripMenuItem.Size = new Size(184, 22);
            removeConstraintToolStripMenuItem.Text = "Usuń ograniczenie";
            removeConstraintToolStripMenuItem.Click += removeConstraintToolStripMenuItem_Click;
            // 
            // verticesContextMenuStrip
            // 
            verticesContextMenuStrip.Items.AddRange(new ToolStripItem[] { deleteVertexToolStripMenuItem, setContinuityInVertexToolStripMenuItem });
            verticesContextMenuStrip.Name = "verticesContextMenuStrip";
            verticesContextMenuStrip.Size = new Size(280, 48);
            // 
            // deleteVertexToolStripMenuItem
            // 
            deleteVertexToolStripMenuItem.Name = "deleteVertexToolStripMenuItem";
            deleteVertexToolStripMenuItem.Size = new Size(279, 22);
            deleteVertexToolStripMenuItem.Text = "Usuń wierczhołek";
            deleteVertexToolStripMenuItem.Click += deleteVertexToolStripMenuItem_Click;
            // 
            // setContinuityInVertexToolStripMenuItem
            // 
            setContinuityInVertexToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { g0ToolStripMenuItem, g1ToolStripMenuItem, c1ToolStripMenuItem });
            setContinuityInVertexToolStripMenuItem.Name = "setContinuityInVertexToolStripMenuItem";
            setContinuityInVertexToolStripMenuItem.Size = new Size(279, 22);
            setContinuityInVertexToolStripMenuItem.Text = "Ustaw żądaną ciągłość w wierzchołku...";
            // 
            // g0ToolStripMenuItem
            // 
            g0ToolStripMenuItem.Checked = true;
            g0ToolStripMenuItem.CheckState = CheckState.Checked;
            g0ToolStripMenuItem.Name = "g0ToolStripMenuItem";
            g0ToolStripMenuItem.Size = new Size(88, 22);
            g0ToolStripMenuItem.Text = "G0";
            g0ToolStripMenuItem.Click += g0ToolStripMenuItem_Click;
            // 
            // g1ToolStripMenuItem
            // 
            g1ToolStripMenuItem.Name = "g1ToolStripMenuItem";
            g1ToolStripMenuItem.Size = new Size(88, 22);
            g1ToolStripMenuItem.Text = "G1";
            g1ToolStripMenuItem.Click += g1ToolStripMenuItem_Click;
            // 
            // c1ToolStripMenuItem
            // 
            c1ToolStripMenuItem.Name = "c1ToolStripMenuItem";
            c1ToolStripMenuItem.Size = new Size(88, 22);
            c1ToolStripMenuItem.Text = "C1";
            c1ToolStripMenuItem.Click += c1ToolStripMenuItem_Click;
            // 
            // clearButton
            // 
            clearButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            clearButton.Location = new Point(697, 27);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(75, 23);
            clearButton.TabIndex = 4;
            clearButton.Text = "Wyczyść";
            clearButton.UseVisualStyleBackColor = true;
            clearButton.Click += clearButton_Click;
            // 
            // PolygonEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(clearButton);
            Controls.Add(drawingPictureBox);
            Controls.Add(bresenhamRadioButton);
            Controls.Add(defaultRadioButton);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            MinimumSize = new Size(500, 300);
            Name = "PolygonEditor";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Polygon Editor";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)drawingPictureBox).EndInit();
            edgesContextMenuStrip.ResumeLayout(false);
            verticesContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem helpToolStripMenuItem;
        private RadioButton defaultRadioButton;
        private RadioButton bresenhamRadioButton;
        private PictureBox drawingPictureBox;
        private ContextMenuStrip edgesContextMenuStrip;
        private ToolStripMenuItem addVertexToolStripMenuItem;
        private ToolStripMenuItem addConstraintToolStripMenuItem;
        private ToolStripMenuItem fixedLengthToolStripMenuItem;
        private ToolStripMenuItem verticalToolStripMenuItem;
        private ToolStripMenuItem horizontalToolStripMenuItem;
        private ToolStripMenuItem bezierToolStripMenuItem;
        private ContextMenuStrip verticesContextMenuStrip;
        private ToolStripMenuItem deleteVertexToolStripMenuItem;
        private ToolStripMenuItem setContinuityInVertexToolStripMenuItem;
        private ToolStripMenuItem g0ToolStripMenuItem;
        private ToolStripMenuItem g1ToolStripMenuItem;
        private ToolStripMenuItem c1ToolStripMenuItem;
        private ToolStripMenuItem removeConstraintToolStripMenuItem;
        private Button clearButton;
    }
}
