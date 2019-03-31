using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace code
{
    public delegate void ProcessDelegate(int val);//声明委托
    /// <summary>
    /// 创建解决方案和生成代码
    /// </summary>
    public class CreateCode
    {
        #region 变量
        public static event ProcessDelegate ProcessIn;//事件
        private static string server;//服务器
        private static string database;//数据库
        private static string name;//登录名
        private static string pwd;//密码
        private static bool connType;//true为Sql模式,False为Win模式

        private static string dbutilityGuid;//db类
        private static string modelGuid;//模型层
        private static string sqlserverdalGuid;//DAL层
        private static string bllGuid;//BLL层
        private static string uilGuid;//UI层 
        #endregion
        #region 生成代码
        public static void CreateCodes(string serverPath, string serverName, string nameSpace, string databaseName, string loginName, string loginPwd, string sourcePath, bool projectType, bool ConnType)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                //获得链接信息
                server = serverName;
                database = databaseName;
                name = loginName;
                pwd = loginPwd;
                connType = ConnType;
                //存储路径
                string solutionPath = serverPath + "\\" + nameSpace;
                //初始化进度条
                ProcessIn?.Invoke(0);
                CreateFolders(solutionPath, projectType);//创建项目目录
                ProcessIn?.Invoke(1);
                CreateDBUtility(serverPath, nameSpace, sourcePath);//创建DBUtility项目
                ProcessIn?.Invoke(2);
                CreateModel(serverPath, nameSpace, sourcePath);//生成Model项目
                ProcessIn?.Invoke(3);
                CreateDAL(serverPath, nameSpace, sourcePath);//生成DAL层
                ProcessIn?.Invoke(4);
                CreateBLL(serverPath, nameSpace, sourcePath);//生成BLL层
                ProcessIn?.Invoke(5);
                if (projectType)
                {
                    CreateWeb(serverPath, nameSpace, sourcePath);//生成web
                }
                else
                {
                    CreateWin(serverPath, nameSpace, sourcePath);//生成窗体
                }
                ProcessIn?.Invoke(6);
                CreateSolution(serverPath, nameSpace, sourcePath, projectType);//生成解决方案
                ProcessIn?.Invoke(7);
            }));
            thread.IsBackground = true;
            thread.Start();
        }
        #endregion
        #region 数据库有关的公用方法
        #region 返回连接语句
        /// <summary>
        /// 连接语句
        /// </summary>
        /// <returns>连接数据库字符串</returns>
        private static string GetConn()
        {
            //链接模式 true为Sql模式,False为Win模式
            return connType ?
                string.Format("server={0};database={1};uid={2};pwd={3}", server, database, name, pwd) :
                string.Format("server={0};database={1};Integrated Security=True", server, database);
        }
        #endregion
        #region 获取所有当前数据库中的表
        /// <summary>
        /// 获取所有当前数据库中的表
        /// </summary>
        /// <returns>当前数据库中的表名数组</returns>
        private static string[] GetTableNames()
        {
            string sql = "select name from sysobjects where type='U' and name<>'sysdiagrams'";
            SqlDataReader reader = SQLHelper.ExecuteReader(GetConn(), CommandType.Text, sql);
            List<string> tableList = new List<string>();
            while (reader.Read())
            {
                tableList.Add("[" + reader[0].ToString() + "]");
            }
            reader.Close();
            return tableList.ToArray();
        }
        #endregion
        #region 获取表中的自增列字段
        /// <summary>
        /// 获取表中的自增列字段
        /// </summary>
        /// <param name="tableName">要查询的表</param>
        /// <returns>单个自增列字段</returns>
        private static string GetIdentityField(string tableName)
        {
            string fieldName = null;
            string sql = "SELECT column_name FROM INFORMATION_SCHEMA.columns " +
                         "WHERE TABLE_NAME='" + tableName + "' AND  " +
                         "COLUMNPROPERTY(OBJECT_ID('" + tableName + "'),COLUMN_NAME,'IsIdentity')=1";
            SqlDataReader reader = SQLHelper.ExecuteReader(GetConn(), CommandType.Text, sql);
            if (reader.Read())
            {
                fieldName = reader[0].ToString();
            }
            reader.Close();
            return fieldName;
        }
        #endregion
        #region 将数据库字段类型转换成C#中使用类型
        /// <summary>
        /// 将数据库字段类型转换成C#中使用类型
        /// </summary>
        /// <param name="typeName">数据库类型</param>
        /// <returns>C#类型</returns>
        private static string ChangeTypeName(string typeName)
        {
            string type = null;
            switch (typeName.ToLower())
            {
                case "int":
                    type = "int?";
                    break;
                case "bigint":
                    type = "Int64?";
                    break;
                case "text":
                case "ntext":
                case "char":
                case "varchar":
                case "nchar":
                case "nvarchar":
                    type = "string";
                    break;
                case "numeric":
                case "decimal":
                case "money":
                case "smallmoney":
                    type = "decimal?";
                    break;
                case "real":
                    type = "Single?";
                    break;
                case "image":
                case "timestamp":
                case "binary":
                case "uniqueidentifier":
                case "varbinary":
                case "xml":
                    type = "Object";
                    break;
                case "tinyint":
                    type = "byte?";
                    break;
                case "float":
                    type = "double?";
                    break;
                case "date":
                case "datetime":
                case "smalldatetime":
                    type = "DateTime?";
                    break;
                case "bit":
                    type = "bool?";
                    break;
                default:
                    type = "string";
                    break;
            }
            return type;
        }
        #endregion
        #region 获取表的列信息
        /// <summary>
        /// 获取表的列信息
        /// </summary>
        /// <param name="tebleName">要查询的表</param>
        /// <returns>返回列信息集合</returns>
        private static List<ColumnContent> GetColumns(string tebleName)
        {
            string sql = string.Format(@"SELECT  CASE WHEN EXISTS
                           (SELECT     1    FROM   sysobjects
                            WHERE   xtype = 'PK' AND parent_obj = a.id AND name IN
                            (SELECT  name   FROM    sysindexes    WHERE      indid IN
                            (SELECT  indid   FROM   sysindexkeys                                
                            WHERE  id = a.id AND colid = a.colid))) THEN '1' ELSE '0' END AS 'key',
                            CASE WHEN COLUMNPROPERTY(a.id, a.name, 'IsIdentity') = 1 
                            THEN '1' ELSE '0' END AS 'identity', a.name AS ColName, c.name AS TypeName, a.length AS 'byte', 
                            COLUMNPROPERTY(a.id, a.name, 
                      'PRECISION') AS 'length', a.xscale, a.isnullable, ISNULL(e.text, '') AS 'default', ISNULL(p.value, '') AS 'comment'
FROM         sys.syscolumns AS a INNER JOIN 
                      sys.sysobjects AS b ON a.id = b.id INNER JOIN
                      sys.systypes AS c ON a.xtype = c.xtype LEFT OUTER JOIN
                      sys.syscomments AS e ON a.cdefault = e.id LEFT OUTER JOIN
                      sys.extended_properties AS p ON a.id = p.major_id AND a.colid = p.minor_id
WHERE     (b.name = @Table) AND (c.status <> '1')
            ");
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("Table",tebleName)
            };
            //获取表的列信息
            SqlDataReader red = SQLHelper.ExecuteReader(GetConn(), CommandType.Text, sql, par);
            List<ColumnContent> list = new List<ColumnContent>();
            while (red.Read())
            {
                ColumnContent cc = new ColumnContent();
                cc.Key = Convert.ToInt32(red["Key"]) == 1;
                cc.Identity = Convert.ToInt32(red["Identity"]) == 1;
                cc.ColName = (string)red["ColName"];
                cc.TypeName = ChangeTypeName((string)red["TypeName"]);
                cc.Byte = Convert.ToInt32(red["Byte"]);
                cc.Length = (int)red["Length"];
                cc.Xscale = Convert.ToInt32(red["Xscale"]);
                cc.Isnullable = Convert.ToInt32(red["Isnullable"]) == 1;
                cc.Default = (string)red["Default"];
                cc.Comment = (string)red["Comment"];
                list.Add(cc);
            }
            red.Close();
            return list;
        }
        #endregion
        #endregion
        #region 创建解决方案并生成代码
        #region 在磁盘创建项目目录
        /// <summary>
        /// 创建项目目录
        /// </summary>
        /// <param name="solutionPath">解决方案路径</param>
        /// <param name="projectType">解决方案类型</param>
        private static void CreateFolders(string solutionPath, bool projectType)
        {
            Directory.CreateDirectory(solutionPath + "\\DBUtility\\Properties");
            Directory.CreateDirectory(solutionPath + "\\Model\\Properties");
            Directory.CreateDirectory(solutionPath + "\\DAL\\Properties");
            Directory.CreateDirectory(solutionPath + "\\BLL\\Properties");
            //是web项目
            if (projectType)
            {
                Directory.CreateDirectory(solutionPath + "\\WebUI");
            }
            else
            {//是winform项目
                Directory.CreateDirectory(solutionPath + "\\WinUI\\Properties");
            }
        }
        #endregion
        #region 生成DBUtility项目
        /// <summary>
        /// 生成DBUtility项目
        /// </summary>
        /// <param name="savePath">存放路径</param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="sourcePath">模版路径</param>
        private static void CreateDBUtility(string savePath, string nameSpace, string sourcePath)
        {
            string path = savePath + "\\" + nameSpace + "\\DBUtility";

            //写AssemblyInfo文件
            StreamReader sr = new StreamReader(sourcePath + "\\AssemblyInfo.cs", Encoding.UTF8);
            string str = sr.ReadToEnd();
            str = str.Replace("{project name}", "DBUtility")
                     .Replace("{guid}", Guid.NewGuid().ToString().ToLower())
                     .Replace("{year}", DateTime.Today.Year.ToString());
            sr.Close();
            StreamWriter sw = new StreamWriter(path + "\\Properties\\AssemblyInfo.cs", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();

            //写SQLHelper文件
            sr = new StreamReader(sourcePath + "\\SQLHelper.cs", Encoding.UTF8);
            str = sr.ReadToEnd();
            sr.Close();
            str = str.Replace("{namespace}", nameSpace);
            sw = new StreamWriter(path + "\\SQLHelper.cs", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();

            //写csproj项目文件
            dbutilityGuid = Guid.NewGuid().ToString().ToUpper();
            sr = new StreamReader(sourcePath + "\\Project.csproj", Encoding.UTF8);
            str = sr.ReadToEnd();
            sr.Close();
            str = str.Replace("{guid}", dbutilityGuid)
                     .Replace("{namespace}", nameSpace)
                     .Replace("{project name}", "DBUtility")
                     .Replace("{reference assembly}", "<Reference Include=\"System.configuration\" />")
                     .Replace("{reference project}", "")
                     .Replace("{include files}", "<Compile Include=\"SQLHelper.cs\" />");
            sw = new StreamWriter(path + "\\DBUtility.csproj", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
        }
        #endregion
        #region 生成Model项目
        /// <summary>
        /// 生成Model项目
        /// </summary>
        ///<param name="savePath">存放路径</param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="sourcePath">模版路径</param>
        private static void CreateModel(string savePath, string nameSpace, string sourcePath)
        {
            string path = savePath + "\\" + nameSpace + "\\Model";
            //写AssemblyInfo文件
            StreamReader sr = new StreamReader(sourcePath + "\\AssemblyInfo.cs", Encoding.UTF8);
            string str = sr.ReadToEnd();
            str = str.Replace("{project name}", "Model")
                     .Replace("{guid}", Guid.NewGuid().ToString().ToLower())
                     .Replace("{year}", DateTime.Today.Year.ToString());
            sr.Close();
            StreamWriter sw = new StreamWriter(path + "\\Properties\\AssemblyInfo.cs", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
            //实体类
            string[] tableNames = GetTableNames();
            foreach (string tableName in tableNames)
            {
                List<ColumnContent> list = GetColumns(tableName.ToDelKh());
                StringBuilder sb = new StringBuilder();
                StringBuilder Par = new StringBuilder();
                StringBuilder Set = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    ColumnContent ct = list[i];
                    //带参构造参数
                    Par.Append(ct.TypeName + " " + ct.ColName.ToPascal() + (i < list.Count - 1 ? ", " : ""));
                    //带参构造赋值
                    Set.Append("            this." + ct.ColName.ToPascal() + " = " + ct.ColName.ToPascal() + ";" + (i < list.Count - 1 ? "\r\n" : ""));
                    //自动属性
                    sb.Append("        /// <summary>\r\n");
                    sb.Append("        /// " + ct.Comment.Replace("\r", "").Replace("\n", "").Trim() + "\r\n");
                    sb.Append("        /// </summary>\r\n");
                    sb.Append("        public " + ct.TypeName + " " + ct.ColName.ToPascal() + " { get; set; }" + (i < list.Count - 1 ? "\r\n\r\n" : ""));
                }
                sr = new StreamReader(sourcePath + "\\Model.cs", Encoding.UTF8);
                str = sr.ReadToEnd();
                sr.Close();
                str = str.Replace("{namespace}", nameSpace)
                         .Replace("{classname}", tableName.ToPascal())
                         .Replace("{properties}", sb.ToString())
                         .Replace("{propertiesPar}", Par.ToString())
                         .Replace("{propertiesSet}", Set.ToString());
                sw = new StreamWriter(path + "\\" + tableName.ToPascal() + ".cs", false, Encoding.UTF8);
                sw.Write(str);
                sw.Close();
            }
            //写csproj项目文件
            modelGuid = Guid.NewGuid().ToString().ToUpper();
            sr = new StreamReader(sourcePath + "\\Project.csproj", Encoding.UTF8);
            str = sr.ReadToEnd();
            sr.Close();
            StringBuilder includeFiles = new StringBuilder();
            foreach (string table in tableNames)
            {
                includeFiles.Append("    <Compile Include=\"" + table.ToPascal() + ".cs\" />\r\n");
            }
            str = str.Replace("{guid}", modelGuid)
                     .Replace("{namespace}", nameSpace)
                     .Replace("{project name}", "Model")
                     .Replace("{reference assembly}", "")
                     .Replace("{reference project}", "")
                     .Replace("{include files}", includeFiles.ToString());
            sw = new StreamWriter(path + "\\Model.csproj", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
        }
        #endregion
        #region 生成DAL项目
        /// <summary>
        /// 生成DAL项目
        /// </summary>
        ///<param name="savePath">存放路径</param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="sourcePath">模版路径</param>
        private static void CreateDAL(string savePath, string nameSpace, string sourcePath)
        {
            string path = savePath + "\\" + nameSpace + "\\DAL";
            //写AssemblyInfo文件
            StreamReader sr = new StreamReader(sourcePath + "\\AssemblyInfo.cs", Encoding.UTF8);
            string str = sr.ReadToEnd();
            str = str.Replace("{project name}", "DAL")
                     .Replace("{guid}", Guid.NewGuid().ToString().ToLower())
                     .Replace("{year}", DateTime.Today.Year.ToString());
            sr.Close();
            StreamWriter sw = new StreamWriter(path + "\\Properties\\AssemblyInfo.cs", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
            //写Service文件
            string[] tables = GetTableNames();
            foreach (string table in tables)
            {
                List<ColumnContent> list = GetColumns(table.ToDelKh());
                string insertFieldList = string.Empty;
                string insertValueList = string.Empty;
                string insertParamList = string.Empty;
                StringBuilder selectConditionList = new StringBuilder();
                StringBuilder selectSetProperty = new StringBuilder();
                StringBuilder updateFields = new StringBuilder();

                for (int i = 0; i < list.Count; i++)
                {
                    ColumnContent ct = list[i];
                    if (!ct.Identity)
                    {
                        insertFieldList += string.Format("{0},", ct.ColName);
                        insertValueList += string.Format("@{0},", ct.ColName.ToPascal().ToUpper());
                        insertParamList += string.Format("                new SqlParameter(\"@{0}\",{1}.{2} == null ? Convert.DBNull : {1}.{2}),\r\n", ct.ColName.ToPascal().ToUpper(), table.ToCamel(), ct.ColName.ToPascal());
                        updateFields.AppendFormat(
                                    "            if ({0}.{1} != null)\r\n" +
                                    "            {{\r\n" +
                                    "                fields += \"{2}=@Update{3},\";\r\n" +
                                    "                paraList.Add(new SqlParameter(\"@Update{3}\", {0}.{1}));\r\n" +
                                    "            }}{4}",
                                    table.ToCamel(), ct.ColName.ToPascal(), ct.ColName, ct.ColName.ToPascal().ToUpper(), (i < list.Count - 1 ? "\r\n" : ""));

                    }

                    selectConditionList.AppendFormat(
                                    "            if ({0}.{1} != null)\r\n" +
                                    "            {{\r\n" +
                                    "                sql += \" and {2}=@{3}\";\r\n" +
                                    "                paraList.Add(new SqlParameter(\"@{3}\", {0}.{1}));\r\n" +
                                    "            }}{4}",
                                    table.ToCamel(), ct.ColName.ToPascal(), ct.ColName, ct.ColName.ToPascal().ToUpper(), (i < list.Count - 1 ? "\r\n" : ""));
                    selectSetProperty.AppendFormat(
                                    "                obj.{0} = Convert.IsDBNull(reader[\"{1}\"]) ? null : ({2})reader[\"{1}\"];{3}",
                                    ct.ColName.ToPascal(), ct.ColName.Trim(new char[] { '[', ']' }), ct.TypeName, (i < list.Count - 1 ? "\r\n" : ""));
                }
                insertFieldList = insertFieldList.Substring(0, insertFieldList.Length - 1);
                insertValueList = insertValueList.Substring(0, insertValueList.Length - 1);
                insertParamList = insertParamList.Substring(0, insertParamList.Length - 3);

                sr = new StreamReader(sourcePath + "\\Service.cs", Encoding.UTF8);
                str = sr.ReadToEnd();
                sr.Close();
                str = str.Replace("{namespace}", nameSpace)
                         .Replace("{model pascal}", table.ToPascal())
                         .Replace("{model camel}", table.ToCamel())
                         .Replace("{table name}", table)
                         .Replace("{field list}", insertFieldList)
                         .Replace("{value list}", insertValueList)
                         .Replace("{insert param list}", insertParamList)
                         .Replace("{updateFields}", updateFields.ToString())
                         .Replace("{condition list}", selectConditionList.ToString())
                         .Replace("{set property}", selectSetProperty.ToString())
                         ;
                sw = new StreamWriter(path + "\\" + table.ToPascal() + "Service.cs", false, Encoding.UTF8);
                sw.Write(str);
                sw.Close();
            }
            sqlserverdalGuid = Guid.NewGuid().ToString().ToUpper();
            sr = new StreamReader(sourcePath + "\\Project.csproj", Encoding.UTF8);
            str = sr.ReadToEnd();
            sr.Close();
            StringBuilder includeFiles = new StringBuilder();
            foreach (string table in tables)
            {
                includeFiles.Append("    <Compile Include=\"" + table.ToPascal() + "Service.cs\" />\r\n");
            }
            string refProj =
                "    <ProjectReference Include=\"..\\Model\\Model.csproj\">\r\n" +
                "      <Project>{" + modelGuid + "}</Project>\r\n" +
                "      <Name>Model</Name>\r\n" +
                "    </ProjectReference>\r\n" +
                "    <ProjectReference Include=\"..\\DBUtility\\DBUtility.csproj\">\r\n" +
                "      <Project>{" + dbutilityGuid + "}</Project>\r\n" +
                "      <Name>DBUtility</Name>\r\n" +
                "    </ProjectReference>\r\n";
            str = str.Replace("{guid}", sqlserverdalGuid)
                     .Replace("{namespace}", nameSpace)
                     .Replace("{project name}", "DAL")
                     .Replace("{reference assembly}", "")
                     .Replace("{reference project}", refProj)
                     .Replace("{include files}", includeFiles.ToString());
            sw = new StreamWriter(path + "\\DAL.csproj", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
        }
        #endregion
        #region 生成BLL项目
        /// <summary>
        /// 生成BLL项目
        /// </summary>
        ///<param name="savePath">存放路径</param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="sourcePath">模版路径</param>
        private static void CreateBLL(string savePath, string nameSpace, string sourcePath)
        {
            string path = savePath + "\\" + nameSpace + "\\BLL";
            //写入AssemblyInfo.cs文件
            StreamReader sr = new StreamReader(sourcePath + "\\AssemblyInfo.cs", Encoding.UTF8);
            string str = sr.ReadToEnd();
            str = str.Replace("{project name}", "BLL")
                     .Replace("{guid}", Guid.NewGuid().ToString().ToLower())
                     .Replace("{year}", DateTime.Today.Year.ToString());
            sr.Close();
            StreamWriter sw = new StreamWriter(path + "\\Properties\\AssemblyInfo.cs", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
            //写manager类
            string[] tables = GetTableNames();
            foreach (string table in tables)
            {
                sr = new StreamReader(sourcePath + "\\Manager.cs", Encoding.UTF8);
                str = sr.ReadToEnd();
                sr.Close();
                str = str.Replace("{namespace}", nameSpace)
                         .Replace("{model pascal}", table.ToPascal())
                         .Replace("{model camel}", table.ToCamel());
                string identityField = GetIdentityField(table);
                if (identityField == null)
                {
                    do
                    {
                        str = str.Remove(str.IndexOf("{start identity modify}"), str.IndexOf("{end}") - str.IndexOf("{start identity modify}") + 5);
                    } while (str.IndexOf("{start identity modify}") > 0);
                }
                else
                {
                    str = str.Replace("{identity field}", identityField.ToPascal())
                             .Replace("{start identity modify}", "")
                             .Replace("{end}", "");
                }
                sw = new StreamWriter(path + "\\" + table.ToPascal() + "Manager.cs", false, Encoding.UTF8);
                sw.Write(str);
                sw.Close();
            }
            //写工程文件
            bllGuid = Guid.NewGuid().ToString().ToUpper();
            sr = new StreamReader(sourcePath + "\\Project.csproj", Encoding.UTF8);
            str = sr.ReadToEnd();
            sr.Close();
            StringBuilder includeFiles = new StringBuilder();
            foreach (string table in tables)
            {
                includeFiles.Append("    <Compile Include=\"" + table.ToPascal() + "Manager.cs\" />\r\n");
            }
            string refProj =
                "    <ProjectReference Include=\"..\\Model\\Model.csproj\">\r\n" +
                "      <Project>{" + modelGuid + "}</Project>\r\n" +
                "      <Name>Model</Name>\r\n" +
                "    </ProjectReference>\r\n" +
                "    <ProjectReference Include=\"..\\DAL\\DAL.csproj\">\r\n" +
                "      <Project>{" + sqlserverdalGuid + "}</Project>\r\n" +
                "      <Name>DAL</Name>\r\n" +
                "    </ProjectReference>\r\n";
            str = str.Replace("{guid}", bllGuid)
                     .Replace("{namespace}", nameSpace)
                     .Replace("{project name}", "BLL")
                     .Replace("{reference assembly}", "")
                     .Replace("{reference project}", refProj)
                     .Replace("{include files}", includeFiles.ToString());
            sw = new StreamWriter(path + "\\BLL.csproj", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
        }
        #endregion
        #region 生成web项目
        /// <summary>
        /// 生成web项目
        /// </summary>
        ///<param name="savePath">存放路径</param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="sourcePath">模版路径</param>
        private static void CreateWeb(string savePath, string nameSpace, string sourcePath)
        {
            string path = savePath + "\\" + nameSpace + "\\WebUI";
            uilGuid = Guid.NewGuid().ToString().ToUpper();
            StreamReader sr = new StreamReader(sourcePath + "\\web.config", Encoding.UTF8);
            string str = sr.ReadToEnd();
            sr.Close();

            string connString = string.Format("server={0};database={1};uid={2};pwd={3}", server, database, name, pwd);
            str = str.Replace("{namespace}", nameSpace)
                     .Replace("{connectionString}", connString);
            StreamWriter sw = new StreamWriter(path + "\\Web.config", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
        }
        #endregion
        #region 生成winform项目
        /// <summary>
        /// 生成winform项目
        /// </summary>
        ///<param name="savePath">存放路径</param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="sourcePath">模版路径</param>
        private static void CreateWin(string savePath, string nameSpace, string sourcePath)
        {
            string path = savePath + "\\" + nameSpace + "\\WinUI";
            //写AssemblyInfo文件
            StreamReader sr = new StreamReader(sourcePath + "\\AssemblyInfo.cs", Encoding.UTF8);
            string str = sr.ReadToEnd();
            str = str.Replace("{project name}", "WinUI")
                     .Replace("{guid}", Guid.NewGuid().ToString().ToLower())
                     .Replace("{year}", DateTime.Today.Year.ToString());
            sr.Close();
            StreamWriter sw = new StreamWriter(path + "\\Properties\\AssemblyInfo.cs", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
            //写Resources.Designer.cs文件
            sr = new StreamReader(sourcePath + "\\Resources.Designer.cs", Encoding.UTF8);
            str = sr.ReadToEnd();
            str = str.Replace("{namespace}", nameSpace);
            sr.Close();
            sw = new StreamWriter(path + "\\Properties\\Resources.Designer.cs", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
            //复制Resources.resx文件
            File.Copy(sourcePath + "\\Resources.resx", path + "\\Properties\\Resources.resx", true);
            //写Settings.Designer.cs文件
            sr = new StreamReader(sourcePath + "\\Settings.Designer.cs", Encoding.UTF8);
            str = sr.ReadToEnd();
            str = str.Replace("{namespace}", nameSpace);
            sr.Close();
            sw = new StreamWriter(path + "\\Properties\\Settings.Designer.cs", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
            //复制Settings.settings文件
            File.Copy(sourcePath + "\\Settings.settings", path + "\\Properties\\Settings.settings", true);
            //写Program.cs文件
            sr = new StreamReader(sourcePath + "\\Program.cs", Encoding.UTF8);
            str = sr.ReadToEnd();
            str = str.Replace("{namespace}", nameSpace);
            sr.Close();
            sw = new StreamWriter(path + "\\Program.cs", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
            //写FrmMain.cs文件
            sr = new StreamReader(sourcePath + "\\FrmMain.cs", Encoding.UTF8);
            str = sr.ReadToEnd();
            str = str.Replace("{namespace}", nameSpace);
            sr.Close();
            sw = new StreamWriter(path + "\\FrmMain.cs", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
            //写FrmMain.Designer.cs文件
            sr = new StreamReader(sourcePath + "\\FrmMain.Designer.cs", Encoding.UTF8);
            str = sr.ReadToEnd();
            str = str.Replace("{namespace}", nameSpace);
            sr.Close();
            sw = new StreamWriter(path + "\\FrmMain.Designer.cs", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
            //写config文件
            uilGuid = Guid.NewGuid().ToString().ToUpper();
            StreamReader AppSr = new StreamReader(sourcePath + "\\web.config", Encoding.UTF8);
            string AppStr = AppSr.ReadToEnd();
            AppSr.Close();

            string connString = string.Format("server={0};database={1};uid={2};pwd={3}", server, database, name, pwd);
            AppStr = AppStr.Replace("{namespace}", nameSpace)
                     .Replace("{connectionString}", connString);
            StreamWriter AppSw = new StreamWriter(path + "\\App.config", false, Encoding.UTF8);
            AppSw.Write(AppStr);
            AppSw.Close();

            //写csproj项目文件
            uilGuid = Guid.NewGuid().ToString().ToUpper();
            string refProj =
                "    <ProjectReference Include=\"..\\Model\\Model.csproj\">\r\n" +
                "      <Project>{" + modelGuid + "}</Project>\r\n" +
                "      <Name>Model</Name>\r\n" +
                "    </ProjectReference>\r\n" +
                "    <ProjectReference Include=\"..\\BLL\\BLL.csproj\">\r\n" +
                "      <Project>{" + bllGuid + "}</Project>\r\n" +
                "      <Name>BLL</Name>\r\n" +
                "    </ProjectReference>\r\n";
            sr = new StreamReader(sourcePath + "\\WinProject.csproj", Encoding.UTF8);
            str = sr.ReadToEnd();
            sr.Close();
            str = str.Replace("{guid}", uilGuid)
                     .Replace("{namespace}", nameSpace)
                     .Replace("{reference project}", refProj);
            sw = new StreamWriter(path + "\\WinUI.csproj", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
        }
        #endregion
        #region 生成项目解决方案
        /// <summary>
        /// 生成项目解决方案
        /// </summary>
        ///<param name="savePath">存放路径</param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="sourcePath">模版路径</param>
        /// <param name="projectType">项目类型</param>
        private static void CreateSolution(string savePath, string nameSpace, string sourcePath, bool projectType)
        {
            string path = savePath + "\\" + nameSpace;
            string solutionGuid = Guid.NewGuid().ToString().ToUpper();
            string projectInfo = null;
            //web项目
            if (projectType)
            {
                projectInfo =
                  "Project(\"{E24C65DC-7377-472B-9ABA-BC803B73C61A}\") = \"WebUI\", \"WebUI\\\", \"{" + uilGuid + "}\"\r\n" +
                  "	ProjectSection(WebsiteProperties) = preProject\r\n" +
                  "		TargetFrameworkMoniker = \".NETFramework,Version%3Dv4.0\"\r\n" +
                  "		ProjectReferences = \"{" + modelGuid + "}|" + nameSpace + ".Model.dll;{" + bllGuid + "}|" + nameSpace + ".BLL.dll;{" + sqlserverdalGuid + "}|" + nameSpace + ".DAL.dll;{" + dbutilityGuid + "}|" + nameSpace + ".DBUtility.dll;\"\r\n" +
                  "		Debug.AspNetCompiler.VirtualPath = \"/WebUI\"\r\n" +
                  "		Debug.AspNetCompiler.PhysicalPath = \"WebUI\\\"\r\n" +
                  "		Debug.AspNetCompiler.TargetPath = \"PrecompiledWeb\\WebUI\\\"\r\n" +
                  "		Debug.AspNetCompiler.Updateable = \"true\"\r\n" +
                  "		Debug.AspNetCompiler.ForceOverwrite = \"true\"\r\n" +
                  "		Debug.AspNetCompiler.FixedNames = \"false\"\r\n" +
                  "		Debug.AspNetCompiler.Debug = \"True\"\r\n" +
                  "		Release.AspNetCompiler.VirtualPath = \"/WebUI\"\r\n" +
                  "		Release.AspNetCompiler.PhysicalPath = \"WebUI\\\"\r\n" +
                  "		Release.AspNetCompiler.TargetPath = \"PrecompiledWeb\\WebUI\"\r\n" +
                  "		Release.AspNetCompiler.Updateable = \"true\"\r\n" +
                  "		Release.AspNetCompiler.ForceOverwrite = \"true\"\r\n" +
                  "		Release.AspNetCompiler.FixedNames = \"false\"\r\n" +
                  "		Release.AspNetCompiler.Debug = \"False\"\r\n" +
                  "		VWDPort = \"" + (new Random().Next(1030, 32700)) + "\"\r\n" +
                  "		DefaultWebSiteLanguage = \"Visual C#\"\r\n" +
                  "	EndProjectSection\r\n" +
                  "EndProject";
            }
            else
            {//winform项目
                projectInfo =
                    "Project(\"{{" + solutionGuid + "}}\") = \"WinUI\", \"WinUI\\WinUI.csproj\", \"{{" + uilGuid + "}}\"\r\n" +
                    "EndProject";
            }
            //写sln文件
            StreamReader sr = new StreamReader(sourcePath + "\\Solution.sln", Encoding.UTF8);
            string str = sr.ReadToEnd();
            sr.Close();
            str = str.Replace("{{solutionGUID}}", solutionGuid)
                     .Replace("{uiGUID}", uilGuid)
                     .Replace("{ui project}", projectInfo)
                     .Replace("{bllGUID}", bllGuid)
                     .Replace("{sqlserverdalGUID}", sqlserverdalGuid)
                     .Replace("{modelGUID}", modelGuid)
                     .Replace("{dbutilityGUID}", dbutilityGuid);
            StreamWriter sw = new StreamWriter(path + "\\" + nameSpace + ".sln", false, Encoding.UTF8);
            sw.Write(str);
            sw.Close();
        } 
        #endregion
        #endregion
    }
}
