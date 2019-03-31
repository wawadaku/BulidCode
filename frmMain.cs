using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace code
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            
        }
        #region 选择数据库连接类型
        //选择数据库连接类型
        private void toolConnType_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            txtLoginId.Enabled = txtLoginPwd.Enabled = menu.Tag.ToString().Equals("win") ? false : true;
            sqlTSMI.Image = winTSMI.Image = null;
            menu.Image = Properties.Resources.menu_check;
        }
        #endregion
        #region 选择存放路径
        private void btnPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = fbd.SelectedPath;
            }
        }
        #endregion
        #region 窗体加载事件
        private void Form1_Load(object sender, EventArgs e)
        {
            const string keyname = @"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL";
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(keyname))
            {
                this.cboServer.Items.Add(".");
                if (rk != null)
                {
                    foreach (string name in rk.GetValueNames())
                    {
                        this.cboServer.Items.Add(".\\" + name);
                    }
                }
                this.cboServer.SelectedIndex = 0;
            }
            txtPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);//默认桌面路径
        }
        #endregion
        #region 自动填充命名空间名称
        private void txtDatabase_TextChanged(object sender, EventArgs e)
        {
            txtNamespace.Text = txtDatabase.Text;
        }
        #endregion
        #region 选择数据库
        private void btnSelectDB_Click(object sender, EventArgs e)
        {
            if (!CheckInfo())//检验输入信息
            {
                return;
            }
            frmSelectDataBase selForm = new frmSelectDataBase(this, winTSMI.Image == null);//true为Sql模式,False为Win模式
            selForm.ShowDialog();
        }
        #endregion
        #region 开始生成
        ProcessDelegate de = null;//订阅事件变量
        private void btnBuild_Click(object sender, EventArgs e)
        {
            if (!CheckInfo())//检验输入信息
            {
                return;
            }
            //订阅事件
            de = new ProcessDelegate(Create_ProcessIn);
            CreateCode.ProcessIn += de;
            //调用方法开始生成
            CreateCode.CreateCodes(txtPath.Text, cboServer.Text, txtNamespace.Text, txtDatabase.Text, txtLoginId.Text, txtLoginPwd.Text, Application.StartupPath + "\\temp", rbWeb.Checked, winTSMI.Image == null);
        }
        private void Create_ProcessIn(int val)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                progressBar1.Value = val;
                if (progressBar1.Value == progressBar1.Maximum)
                {
                    MessageBox.Show("完成！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    progressBar1.Value = 0;
                    CreateCode.ProcessIn -= de;//生成完毕取消订阅
                }
            }));
        }
        #endregion
        #region 检验输入信息
        private bool CheckInfo()
        {
            if (cboServer.Text == string.Empty)
            {
                MessageBox.Show("请输入服务器名或IP", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cboServer.Focus();
                return false;
            }
            if (winTSMI.Image == null)
            {
                if (txtLoginId.Text == string.Empty)
                {
                    MessageBox.Show("请输入登录名", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtLoginId.Focus();
                    return false;
                }
                if (txtLoginPwd.Text == string.Empty)
                {
                    MessageBox.Show("请输入密码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtLoginPwd.Focus();
                    return false;
                }
                string connString = string.Format("server={0};database=master;uid={1};pwd={2}", cboServer.Text, txtLoginId.Text, txtLoginPwd.Text);
                SqlConnection conn = new SqlConnection(connString);
                try
                {
                    conn.Open();
                    return conn.State == ConnectionState.Open;
                }
                catch
                {
                    MessageBox.Show("服务器或登录名密码错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    conn.Close();

                }
                return false;
            }
            return true;
        }

        #endregion
    }
}
