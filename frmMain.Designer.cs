namespace code
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Sysmenu = new System.Windows.Forms.MenuStrip();
            this.设置身份验证ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.winTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelectDB = new System.Windows.Forms.Button();
            this.txtLoginPwd = new System.Windows.Forms.TextBox();
            this.txtLoginId = new System.Windows.Forms.TextBox();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.cboServer = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPath = new System.Windows.Forms.Button();
            this.rbWin = new System.Windows.Forms.RadioButton();
            this.rbWeb = new System.Windows.Forms.RadioButton();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnBuild = new System.Windows.Forms.Button();
            this.sqlTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.Sysmenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Sysmenu
            // 
            this.Sysmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置身份验证ToolStripMenuItem});
            this.Sysmenu.Location = new System.Drawing.Point(0, 0);
            this.Sysmenu.Name = "Sysmenu";
            this.Sysmenu.Size = new System.Drawing.Size(497, 25);
            this.Sysmenu.TabIndex = 1;
            this.Sysmenu.Text = "menuStrip1";
            // 
            // 设置身份验证ToolStripMenuItem
            // 
            this.设置身份验证ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.winTSMI,
            this.sqlTSMI});
            this.设置身份验证ToolStripMenuItem.Name = "设置身份验证ToolStripMenuItem";
            this.设置身份验证ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.设置身份验证ToolStripMenuItem.Text = "设置身份验证";
            // 
            // winTSMI
            // 
            this.winTSMI.Name = "winTSMI";
            this.winTSMI.Size = new System.Drawing.Size(187, 22);
            this.winTSMI.Tag = "win";
            this.winTSMI.Text = "Windows 身份验证";
            this.winTSMI.Click += new System.EventHandler(this.toolConnType_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelectDB);
            this.groupBox1.Controls.Add(this.txtLoginPwd);
            this.groupBox1.Controls.Add(this.txtLoginId);
            this.groupBox1.Controls.Add(this.txtDatabase);
            this.groupBox1.Controls.Add(this.cboServer);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 144);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务器信息";
            // 
            // btnSelectDB
            // 
            this.btnSelectDB.BackColor = System.Drawing.Color.Firebrick;
            this.btnSelectDB.FlatAppearance.BorderSize = 0;
            this.btnSelectDB.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectDB.Location = new System.Drawing.Point(191, 52);
            this.btnSelectDB.Name = "btnSelectDB";
            this.btnSelectDB.Size = new System.Drawing.Size(35, 23);
            this.btnSelectDB.TabIndex = 4;
            this.btnSelectDB.Text = "...";
            this.btnSelectDB.UseVisualStyleBackColor = false;
            this.btnSelectDB.Click += new System.EventHandler(this.btnSelectDB_Click);
            // 
            // txtLoginPwd
            // 
            this.txtLoginPwd.Location = new System.Drawing.Point(63, 110);
            this.txtLoginPwd.Name = "txtLoginPwd";
            this.txtLoginPwd.PasswordChar = '*';
            this.txtLoginPwd.Size = new System.Drawing.Size(127, 21);
            this.txtLoginPwd.TabIndex = 3;
            this.txtLoginPwd.Text = "sa";
            // 
            // txtLoginId
            // 
            this.txtLoginId.Location = new System.Drawing.Point(63, 82);
            this.txtLoginId.Name = "txtLoginId";
            this.txtLoginId.Size = new System.Drawing.Size(127, 21);
            this.txtLoginId.TabIndex = 3;
            this.txtLoginId.Text = "sa";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(63, 54);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.ReadOnly = true;
            this.txtDatabase.Size = new System.Drawing.Size(127, 21);
            this.txtDatabase.TabIndex = 2;
            this.txtDatabase.TextChanged += new System.EventHandler(this.txtDatabase_TextChanged);
            // 
            // cboServer
            // 
            this.cboServer.FormattingEnabled = true;
            this.cboServer.Location = new System.Drawing.Point(63, 25);
            this.cboServer.Name = "cboServer";
            this.cboServer.Size = new System.Drawing.Size(157, 20);
            this.cboServer.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "密  码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "登录名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "数据库：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPath);
            this.groupBox2.Controls.Add(this.rbWin);
            this.groupBox2.Controls.Add(this.rbWeb);
            this.groupBox2.Controls.Add(this.txtPath);
            this.groupBox2.Controls.Add(this.txtNamespace);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(250, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(235, 144);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "项目信息";
            // 
            // btnPath
            // 
            this.btnPath.Location = new System.Drawing.Point(184, 65);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(45, 24);
            this.btnPath.TabIndex = 2;
            this.btnPath.Text = "路径";
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // rbWin
            // 
            this.rbWin.AutoSize = true;
            this.rbWin.Checked = true;
            this.rbWin.Location = new System.Drawing.Point(139, 107);
            this.rbWin.Name = "rbWin";
            this.rbWin.Size = new System.Drawing.Size(65, 16);
            this.rbWin.TabIndex = 1;
            this.rbWin.TabStop = true;
            this.rbWin.Text = "WinForm";
            this.rbWin.UseVisualStyleBackColor = true;
            // 
            // rbWeb
            // 
            this.rbWeb.AutoSize = true;
            this.rbWeb.Location = new System.Drawing.Point(76, 107);
            this.rbWeb.Name = "rbWeb";
            this.rbWeb.Size = new System.Drawing.Size(41, 16);
            this.rbWeb.TabIndex = 1;
            this.rbWeb.Text = "Web";
            this.rbWeb.UseVisualStyleBackColor = true;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(71, 68);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(113, 21);
            this.txtPath.TabIndex = 0;
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(71, 30);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(153, 21);
            this.txtNamespace.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "项目类型：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "存放路径：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "命名空间：";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 192);
            this.progressBar1.Maximum = 7;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(368, 16);
            this.progressBar1.TabIndex = 4;
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(399, 183);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(75, 31);
            this.btnBuild.TabIndex = 5;
            this.btnBuild.Text = "生成";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // sqlTSMI
            // 
            this.sqlTSMI.Image = global::code.Properties.Resources.menu_check;
            this.sqlTSMI.Name = "sqlTSMI";
            this.sqlTSMI.Size = new System.Drawing.Size(187, 22);
            this.sqlTSMI.Tag = "sql";
            this.sqlTSMI.Text = "Sql Server 身份验证";
            this.sqlTSMI.Click += new System.EventHandler(this.toolConnType_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 224);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Sysmenu);
            this.MainMenuStrip = this.Sysmenu;
            this.MaximumSize = new System.Drawing.Size(513, 262);
            this.MinimumSize = new System.Drawing.Size(513, 262);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "代码生成器";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Sysmenu.ResumeLayout(false);
            this.Sysmenu.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip Sysmenu;
        private System.Windows.Forms.ToolStripMenuItem 设置身份验证ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem winTSMI;
        private System.Windows.Forms.ToolStripMenuItem sqlTSMI;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectDB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbWin;
        private System.Windows.Forms.RadioButton rbWeb;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnBuild;
        internal System.Windows.Forms.TextBox txtDatabase;
        public System.Windows.Forms.ComboBox cboServer;
        public System.Windows.Forms.TextBox txtLoginPwd;
        public System.Windows.Forms.TextBox txtLoginId;
    }
}

