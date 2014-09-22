namespace croozshinbun
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labellevel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelage = new System.Windows.Forms.Label();
            this.labelusername = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 15);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 208);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // labellevel
            // 
            this.labellevel.BackColor = System.Drawing.Color.White;
            this.labellevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labellevel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labellevel.ForeColor = System.Drawing.Color.Black;
            this.labellevel.Location = new System.Drawing.Point(0, 0);
            this.labellevel.Margin = new System.Windows.Forms.Padding(0);
            this.labellevel.Name = "labellevel";
            this.labellevel.Size = new System.Drawing.Size(80, 15);
            this.labellevel.TabIndex = 1;
            this.labellevel.Text = "レベル";
            this.labellevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labellevel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labellevel_MouseDown);
            this.labellevel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labellevel_MouseMove);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(0, 252);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 128);
            this.label1.TabIndex = 2;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            // 
            // labelage
            // 
            this.labelage.BackColor = System.Drawing.Color.White;
            this.labelage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelage.Location = new System.Drawing.Point(0, 222);
            this.labelage.Margin = new System.Windows.Forms.Padding(0);
            this.labelage.Name = "labelage";
            this.labelage.Size = new System.Drawing.Size(80, 16);
            this.labelage.TabIndex = 3;
            this.labelage.Text = "コミュ年齢";
            this.labelage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelage_MouseDown);
            this.labelage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelage_MouseMove);
            // 
            // labelusername
            // 
            this.labelusername.BackColor = System.Drawing.Color.White;
            this.labelusername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelusername.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelusername.Location = new System.Drawing.Point(0, 237);
            this.labelusername.Margin = new System.Windows.Forms.Padding(0);
            this.labelusername.Name = "labelusername";
            this.labelusername.Size = new System.Drawing.Size(80, 16);
            this.labelusername.TabIndex = 4;
            this.labelusername.Text = "ユーザ名";
            this.labelusername.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelusername.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelusername_MouseDown);
            this.labelusername.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelusername_MouseMove);
            // 
            // Form2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(80, 380);
            this.Controls.Add(this.labelusername);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labellevel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(80, 380);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(80, 380);
            this.Name = "Form2";
            this.Text = "出走データ";
            this.TopMost = true;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Label labellevel;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label labelage;
        public System.Windows.Forms.Label labelusername;
    }
}