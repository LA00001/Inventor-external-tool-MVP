namespace MyFirstInventorPlugin_VS2017_Lesson5_CSharp
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageIpt = new System.Windows.Forms.TabPage();
            this.labelIptFeatureList = new System.Windows.Forms.Label();
            this.ButtonIptRemoveSelectedFeature = new System.Windows.Forms.Button();
            this.ButtonIptCopyFeatureList = new System.Windows.Forms.Button();
            this.ButtonIptClearFeatureList = new System.Windows.Forms.Button();
            this.listBoxIptFeatures = new System.Windows.Forms.ListBox();
            this.labelIptGroupList = new System.Windows.Forms.Label();
            this.ButtonIptRemoveSelected = new System.Windows.Forms.Button();
            this.ButtonIptCopyList = new System.Windows.Forms.Button();
            this.ButtonIptClearList = new System.Windows.Forms.Button();
            this.listBoxIptBodies = new System.Windows.Forms.ListBox();
            this.labelIptInfo = new System.Windows.Forms.Label();
            this.ButtonIptToggle = new System.Windows.Forms.Button();
            this.ButtonIptAddInsideBox = new System.Windows.Forms.Button();
            this.ButtonIptAdd = new System.Windows.Forms.Button();
            this.tabPageIam = new System.Windows.Forms.TabPage();
            this.labelIamInfo = new System.Windows.Forms.Label();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageIpt.SuspendLayout();
            this.tabPageIam.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageIpt);
            this.tabControl1.Controls.Add(this.tabPageIam);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(984, 641);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageIpt
            // 
            this.tabPageIpt.Controls.Add(this.labelIptFeatureList);
            this.tabPageIpt.Controls.Add(this.ButtonIptRemoveSelectedFeature);
            this.tabPageIpt.Controls.Add(this.ButtonIptCopyFeatureList);
            this.tabPageIpt.Controls.Add(this.ButtonIptClearFeatureList);
            this.tabPageIpt.Controls.Add(this.listBoxIptFeatures);
            this.tabPageIpt.Controls.Add(this.labelIptGroupList);
            this.tabPageIpt.Controls.Add(this.ButtonIptRemoveSelected);
            this.tabPageIpt.Controls.Add(this.ButtonIptCopyList);
            this.tabPageIpt.Controls.Add(this.ButtonIptClearList);
            this.tabPageIpt.Controls.Add(this.listBoxIptBodies);
            this.tabPageIpt.Controls.Add(this.labelIptInfo);
            this.tabPageIpt.Controls.Add(this.ButtonIptToggle);
            this.tabPageIpt.Controls.Add(this.ButtonIptAddInsideBox);
            this.tabPageIpt.Controls.Add(this.ButtonIptAdd);
            this.tabPageIpt.Location = new System.Drawing.Point(4, 25);
            this.tabPageIpt.Name = "tabPageIpt";
            this.tabPageIpt.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIpt.Size = new System.Drawing.Size(976, 612);
            this.tabPageIpt.TabIndex = 0;
            this.tabPageIpt.Text = ".ipt";
            this.tabPageIpt.UseVisualStyleBackColor = true;
            // 
            // labelIptFeatureList
            // 
            this.labelIptFeatureList.AutoSize = true;
            this.labelIptFeatureList.Location = new System.Drawing.Point(320, 312);
            this.labelIptFeatureList.Name = "labelIptFeatureList";
            this.labelIptFeatureList.Size = new System.Drawing.Size(261, 16);
            this.labelIptFeatureList.TabIndex = 13;
            this.labelIptFeatureList.Text = "Features / элементы, связанные с телами";
            // 
            // ButtonIptRemoveSelectedFeature
            // 
            this.ButtonIptRemoveSelectedFeature.Location = new System.Drawing.Point(616, 535);
            this.ButtonIptRemoveSelectedFeature.Name = "ButtonIptRemoveSelectedFeature";
            this.ButtonIptRemoveSelectedFeature.Size = new System.Drawing.Size(134, 56);
            this.ButtonIptRemoveSelectedFeature.TabIndex = 12;
            this.ButtonIptRemoveSelectedFeature.Text = "Remove selected\r\nfeature";
            this.ButtonIptRemoveSelectedFeature.UseVisualStyleBackColor = true;
            this.ButtonIptRemoveSelectedFeature.Click += new System.EventHandler(this.ButtonIptRemoveSelectedFeature_Click);
            // 
            // ButtonIptCopyFeatureList
            // 
            this.ButtonIptCopyFeatureList.Location = new System.Drawing.Point(470, 535);
            this.ButtonIptCopyFeatureList.Name = "ButtonIptCopyFeatureList";
            this.ButtonIptCopyFeatureList.Size = new System.Drawing.Size(134, 56);
            this.ButtonIptCopyFeatureList.TabIndex = 11;
            this.ButtonIptCopyFeatureList.Text = "Copy features\r\nto clipboard";
            this.ButtonIptCopyFeatureList.UseVisualStyleBackColor = true;
            this.ButtonIptCopyFeatureList.Click += new System.EventHandler(this.ButtonIptCopyFeatureList_Click);
            // 
            // ButtonIptClearFeatureList
            // 
            this.ButtonIptClearFeatureList.Location = new System.Drawing.Point(323, 535);
            this.ButtonIptClearFeatureList.Name = "ButtonIptClearFeatureList";
            this.ButtonIptClearFeatureList.Size = new System.Drawing.Size(134, 56);
            this.ButtonIptClearFeatureList.TabIndex = 10;
            this.ButtonIptClearFeatureList.Text = "Clear feature\r\nlist";
            this.ButtonIptClearFeatureList.UseVisualStyleBackColor = true;
            this.ButtonIptClearFeatureList.Click += new System.EventHandler(this.ButtonIptClearFeatureList_Click);
            // 
            // listBoxIptFeatures
            // 
            this.listBoxIptFeatures.FormattingEnabled = true;
            this.listBoxIptFeatures.HorizontalScrollbar = true;
            this.listBoxIptFeatures.ItemHeight = 16;
            this.listBoxIptFeatures.Location = new System.Drawing.Point(323, 335);
            this.listBoxIptFeatures.Name = "listBoxIptFeatures";
            this.listBoxIptFeatures.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxIptFeatures.Size = new System.Drawing.Size(627, 180);
            this.listBoxIptFeatures.TabIndex = 9;
            this.listBoxIptFeatures.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxIptFeatures_KeyDown);
            // 
            // labelIptGroupList
            // 
            this.labelIptGroupList.AutoSize = true;
            this.labelIptGroupList.Location = new System.Drawing.Point(320, 23);
            this.labelIptGroupList.Name = "labelIptGroupList";
            this.labelIptGroupList.Size = new System.Drawing.Size(279, 16);
            this.labelIptGroupList.TabIndex = 8;
            this.labelIptGroupList.Text = "Bodies in current group / список тел в группе";
            // 
            // ButtonIptRemoveSelected
            // 
            this.ButtonIptRemoveSelected.Location = new System.Drawing.Point(616, 240);
            this.ButtonIptRemoveSelected.Name = "ButtonIptRemoveSelected";
            this.ButtonIptRemoveSelected.Size = new System.Drawing.Size(134, 56);
            this.ButtonIptRemoveSelected.TabIndex = 7;
            this.ButtonIptRemoveSelected.Text = "Remove selected\r\nbody";
            this.ButtonIptRemoveSelected.UseVisualStyleBackColor = true;
            this.ButtonIptRemoveSelected.Click += new System.EventHandler(this.ButtonIptRemoveSelected_Click);
            // 
            // ButtonIptCopyList
            // 
            this.ButtonIptCopyList.Location = new System.Drawing.Point(470, 240);
            this.ButtonIptCopyList.Name = "ButtonIptCopyList";
            this.ButtonIptCopyList.Size = new System.Drawing.Size(134, 56);
            this.ButtonIptCopyList.TabIndex = 6;
            this.ButtonIptCopyList.Text = "Copy bodies\r\nto clipboard";
            this.ButtonIptCopyList.UseVisualStyleBackColor = true;
            this.ButtonIptCopyList.Click += new System.EventHandler(this.ButtonIptCopyList_Click);
            // 
            // ButtonIptClearList
            // 
            this.ButtonIptClearList.Location = new System.Drawing.Point(323, 240);
            this.ButtonIptClearList.Name = "ButtonIptClearList";
            this.ButtonIptClearList.Size = new System.Drawing.Size(134, 56);
            this.ButtonIptClearList.TabIndex = 5;
            this.ButtonIptClearList.Text = "Clear ALL\r\nlists/groups";
            this.ButtonIptClearList.UseVisualStyleBackColor = true;
            this.ButtonIptClearList.Click += new System.EventHandler(this.ButtonIptClearList_Click);
            // 
            // listBoxIptBodies
            // 
            this.listBoxIptBodies.FormattingEnabled = true;
            this.listBoxIptBodies.HorizontalScrollbar = true;
            this.listBoxIptBodies.ItemHeight = 16;
            this.listBoxIptBodies.Location = new System.Drawing.Point(323, 50);
            this.listBoxIptBodies.Name = "listBoxIptBodies";
            this.listBoxIptBodies.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxIptBodies.Size = new System.Drawing.Size(627, 180);
            this.listBoxIptBodies.TabIndex = 4;
            this.listBoxIptBodies.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxIptBodies_KeyDown);
            // 
            // labelIptInfo
            // 
            this.labelIptInfo.AutoSize = true;
            this.labelIptInfo.Location = new System.Drawing.Point(28, 23);
            this.labelIptInfo.Name = "labelIptInfo";
            this.labelIptInfo.Size = new System.Drawing.Size(253, 112);
            this.labelIptInfo.TabIndex = 3;
            this.labelIptInfo.Text = "Part mode (.ipt)\r\n1) Add selected: selected bodies.\r\n2) Select Frame: drag window in Inventor.\r\n3) Toggle grouped bodies.\r\n\r\nRight side has two editable lists:\r\nBodies + Features.";
            // 
            // ButtonIptToggle
            // 
            this.ButtonIptToggle.Location = new System.Drawing.Point(58, 315);
            this.ButtonIptToggle.Name = "ButtonIptToggle";
            this.ButtonIptToggle.Size = new System.Drawing.Size(186, 56);
            this.ButtonIptToggle.TabIndex = 2;
            this.ButtonIptToggle.Text = "Hide or Show\r\nBodies in Group";
            this.ButtonIptToggle.UseVisualStyleBackColor = true;
            this.ButtonIptToggle.Click += new System.EventHandler(this.ButtonIptToggle_Click);
            // 
            // ButtonIptAddInsideBox
            // 
            this.ButtonIptAddInsideBox.Location = new System.Drawing.Point(58, 225);
            this.ButtonIptAddInsideBox.Name = "ButtonIptAddInsideBox";
            this.ButtonIptAddInsideBox.Size = new System.Drawing.Size(186, 58);
            this.ButtonIptAddInsideBox.TabIndex = 1;
            this.ButtonIptAddInsideBox.Text = "Select Frame + Add\r\nInner/Hidden Bodies";
            this.ButtonIptAddInsideBox.UseVisualStyleBackColor = true;
            this.ButtonIptAddInsideBox.Click += new System.EventHandler(this.ButtonIptAddInsideBox_Click);
            // 
            // ButtonIptAdd
            // 
            this.ButtonIptAdd.Location = new System.Drawing.Point(58, 145);
            this.ButtonIptAdd.Name = "ButtonIptAdd";
            this.ButtonIptAdd.Size = new System.Drawing.Size(186, 58);
            this.ButtonIptAdd.TabIndex = 0;
            this.ButtonIptAdd.Text = "Add selected\r\nBodies to Group";
            this.ButtonIptAdd.UseVisualStyleBackColor = true;
            this.ButtonIptAdd.Click += new System.EventHandler(this.ButtonIptAdd_Click);
            // 
            // tabPageIam
            // 
            this.tabPageIam.Controls.Add(this.labelIamInfo);
            this.tabPageIam.Controls.Add(this.Button2);
            this.tabPageIam.Controls.Add(this.Button1);
            this.tabPageIam.Location = new System.Drawing.Point(4, 25);
            this.tabPageIam.Name = "tabPageIam";
            this.tabPageIam.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIam.Size = new System.Drawing.Size(976, 612);
            this.tabPageIam.TabIndex = 1;
            this.tabPageIam.Text = ".iam";
            this.tabPageIam.UseVisualStyleBackColor = true;
            // 
            // labelIamInfo
            // 
            this.labelIamInfo.AutoSize = true;
            this.labelIamInfo.Location = new System.Drawing.Point(28, 23);
            this.labelIamInfo.Name = "labelIamInfo";
            this.labelIamInfo.Size = new System.Drawing.Size(305, 48);
            this.labelIamInfo.TabIndex = 2;
            this.labelIamInfo.Text = "Assembly mode (.iam)\r\nThis is the original Lesson 5 component code.\r\nSelect a component or subassembly.";
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(99, 164);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(186, 56);
            this.Button2.TabIndex = 1;
            this.Button2.Text = "Hide or Show\r\nComponents in Group";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(99, 91);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(186, 58);
            this.Button1.TabIndex = 0;
            this.Button1.Text = "Add selected\r\nComponents to Group";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 641);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Inventor external tool";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tabPageIpt.ResumeLayout(false);
            this.tabPageIpt.PerformLayout();
            this.tabPageIam.ResumeLayout(false);
            this.tabPageIam.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageIpt;
        private System.Windows.Forms.TabPage tabPageIam;
        private System.Windows.Forms.Button Button1;
        private System.Windows.Forms.Button Button2;
        private System.Windows.Forms.Button ButtonIptAdd;
        private System.Windows.Forms.Button ButtonIptAddInsideBox;
        private System.Windows.Forms.Button ButtonIptToggle;
        private System.Windows.Forms.Label labelIptInfo;
        private System.Windows.Forms.Label labelIamInfo;
        private System.Windows.Forms.ListBox listBoxIptBodies;
        private System.Windows.Forms.Button ButtonIptClearList;
        private System.Windows.Forms.Button ButtonIptCopyList;
        private System.Windows.Forms.Button ButtonIptRemoveSelected;
        private System.Windows.Forms.Label labelIptGroupList;
        private System.Windows.Forms.ListBox listBoxIptFeatures;
        private System.Windows.Forms.Button ButtonIptClearFeatureList;
        private System.Windows.Forms.Button ButtonIptCopyFeatureList;
        private System.Windows.Forms.Button ButtonIptRemoveSelectedFeature;
        private System.Windows.Forms.Label labelIptFeatureList;
    }
}
