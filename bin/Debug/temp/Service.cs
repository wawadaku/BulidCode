using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using {namespace}.Model;
using System.Data.SqlClient;
using {namespace}.DBUtility;
using System.Data;

namespace {namespace}.DAL
{
    public class {model pascal}Service
    {
        #region BasicMethod
		/// <summary>
        /// 增加{model pascal}表信息
        /// </summary>
        /// <param name="{model camel}">{model camel}表对象</param>
        /// <returns>受影响行数</returns>
        public int Add{model pascal}({model pascal} {model camel})
        {
            string sql = "insert into {table name}({field list}) values({value list})";
            SqlParameter[] paras = new SqlParameter[]{
{insert param list}
            };
            return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql, paras);
        }

        /// <summary>
        /// 删除{model pascal}表信息
        /// </summary>
        /// <param name="{model camel}">{model camel}表对象</param>
        /// <returns>受影响行数</returns>
        public int Delete{model pascal}({model pascal} {model camel})
        {
            string sql = "delete from {table name} where 1=1";
            List<SqlParameter> paraList = GetCondition({model camel}, ref sql);
            return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql, paraList.ToArray());
        }

        /// <summary>
        /// 更新{model pascal}表信息
        /// </summary>
        /// <param name="{model camel}">{model camel}表更新对象</param>
        /// <param name="old{model pascal}">{model camel}表查询对象</param>
        /// <returns>受影响行数</returns>
        public int Update{model pascal}({model pascal} {model camel}, {model pascal} old{model pascal})
        {
            string fields = "";
            string conditions = "";
            List<SqlParameter> fieldsParameter = GetUpdateFields({model camel}, ref fields);
            List<SqlParameter> conditionParameter = GetCondition(old{model pascal}, ref conditions);
            string sql = "update {table name} set " + fields + 
                         " where 1=1" + conditions;
            fieldsParameter.AddRange(conditionParameter);
            SqlParameter[] paras = fieldsParameter.ToArray();
            return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql, paras);
        }

        /// <summary>
        /// 查询{model pascal}表信息
        /// </summary>
        /// <param name="{model camel}">{model camel}表查询对象</param>
        /// <returns>IList对象集合</returns>
        public IList<{model pascal}> Get{model pascal}({model pascal} {model camel})
        {
            string sql = "select * from {table name} where 1=1";
            List<SqlParameter> paraList = new List<SqlParameter>();
            if({model camel}!=null)
            {
                paraList = GetCondition({model camel}, ref sql);
            }

            IList<{model pascal}> {model camel}List = new List<{model pascal}>();
            SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, sql, paraList.ToArray());
            while (reader.Read())
            {
                {model pascal} obj = new {model pascal}();
{set property}
                {model camel}List.Add(obj);
            }
            reader.Close();
            return {model camel}List;
        }

        private static List<SqlParameter> GetCondition({model pascal} {model camel}, ref string sql)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
{condition list}
            return paraList;
        }

        private static List<SqlParameter> GetUpdateFields({model pascal} {model camel}, ref string fields)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            fields = "";
{updateFields}
            fields = fields.Substring(0, fields.Length - 1);
            return paraList;
        }
	    #endregion

        #region ExtensionMethod
		 
	    #endregion
    }
}
