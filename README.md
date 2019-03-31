# BulidCode
这是一个三层代码生成器的源码，生成之后，只需要实现ui层就可以啦。亲测，但是需要增加复杂功能还是要修改的。
..bin\Debug\temp目录下的文件是生成代码的模板文件。一定得有。
只能是sqlserver数据库。注释都有，供大家参考。

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
