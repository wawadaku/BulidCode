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
    public partial class frmSelectDataBase : Form
    {
        public frmSelectDataBase()
        {
            InitializeComponent();
        }
        #region 重载构造函数
        frmMain main;
        bool ConnType;
        public frmSelectDataBase(frmMain Main, bool connTyoe)
        {
            InitializeComponent();
            this.main = Main;
            this.ConnType = connTyoe;
        } 
        #endregion
        #region 选择数据库
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (lbDatabases.SelectedItem != null)
                main.txtDatabase.Text = lbDatabases.SelectedItem.ToString();
            this.Close();
        }
        #endregion
        #region 窗口加载时载入数据库列表
        private void frmSelectDataBase_Load(object sender, EventArgs e)
        {
            string serverName = main.cboServer.Text;
            string loginName = main.txtLoginId.Text;
            string loginPwd = main.txtLoginPwd.Text;
            string connString = ConnType ?
                string.Format("server={0};database=master;uid={1};pwd={2}", serverName, loginName, loginPwd) :
                string.Format("server={0};database=master;Integrated Security=True", serverName);
            //绑定数据库
            string sql = "select name from sysdatabases where dbid>6";
            SqlDataReader red = SQLHelper.ExecuteReader(connString, CommandType.Text, sql);
            while (red.Read())
            {
                this.lbDatabases.Items.Add(red["name"].ToString());
            }
            red.Close();
        } 
        #endregion
    }
}
