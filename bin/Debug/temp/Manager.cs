using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using {namespace}.Model;
using {namespace}.DAL;

namespace {namespace}.BLL
{
    /// <summary>
    /// {model pascal}管理类
    /// </summary>
    public class {model pascal}Manager
    {
        public static {model pascal}Service service = new {model pascal}Service();

        #region BasicMethod
		/// <summary>
        /// 增加{model pascal}表信息
        /// </summary>
        /// <param name="{model camel}">{model camel}表对象</param>
        /// <returns>受影响行数</returns>
        public static int Add{model pascal}({model pascal} {model camel})
        {
            return service.Add{model pascal}({model camel});
        }

        /// <summary>
        /// 删除{model pascal}表信息
        /// </summary>
        /// <param name="{model camel}">{model camel}表对象</param>
        /// <returns>受影响行数</returns>
        public static int Delete{model pascal}({model pascal} {model camel} = null)
        {
            return service.Delete{model pascal}({model camel});
        }
{start identity modify}
        /// <summary>
        /// 根据Identity序列号删除{model pascal}表信息
        /// </summary>
        /// <param name="Identity">Identity序列号</param>
        /// <returns>受影响行数</returns>
        public static int Delete{model pascal}ByIdentity(int? Identity)
        {
            {model pascal} {model camel} = new {model pascal}() { {identity field} = Identity };
            return service.Delete{model pascal}({model camel});
        }
{end}
        /// <summary>
        /// 更新{model pascal}表信息
        /// </summary>
        /// <param name="{model camel}">{model camel}表更新对象</param>
        /// <param name="old{model pascal}">{model camel}表查询对象</param>
        /// <returns>受影响行数</returns>
        public static int Update{model pascal}({model pascal} {model camel}, {model pascal} old{model pascal} = null)
        {
            return service.Update{model pascal}({model camel},old{model pascal});
        }
{start identity modify}
        /// <summary>
        /// 更新{model pascal}表信息，按照对象中标识列更新
        /// </summary>
        /// <param name="{model camel}">{model camel}表对象</param>
        /// <returns>受影响行数</returns>
        public static int Update{model pascal}ByIdentity({model pascal} {model camel})
        {
            {model pascal} old{model pascal} = new {model pascal}();
            old{model pascal}.{identity field} = {model camel}.{identity field};
            {model camel}.{identity field} = null;
            return service.Update{model pascal}({model camel}, old{model pascal});
        }
{end}
        /// <summary>
        /// 查询{model pascal}表信息
        /// </summary>
        /// <param name="{model camel}">{model camel}表查询对象</param>
        /// <returns>IList对象集合</returns>
        public static IList<{model pascal}> Get{model pascal}All({model pascal} {model camel} = null)
        {
            return service.Get{model pascal}({model camel});
        }
{start identity modify}
        /// <summary>
        /// 根据Identity序列号查询{model pascal}表信息
        /// </summary>
        /// <param name="Identity">Identity序列号</param>
        /// <returns>IList对象集合</returns>
        public static {model pascal} Get{model pascal}ByIdentity(int? Identity)
        {
            {model pascal} {model camel} = new {model pascal}() { {identity field} = Identity };
            return service.Get{model pascal}({model camel}).Count > 0 ? service.Get{model pascal}({model camel})[0] : null;
        }
{end}
        /// <summary>
        /// 查询{model pascal}表信息
        /// </summary>
        /// <param name="{model camel}">{model camel}表查询对象</param>
        /// <returns>{model pascal}表对象</returns>
        public static {model pascal} Get{model pascal}({model pascal} {model camel})
        {
            return service.Get{model pascal}({model camel}).Count > 0 ? service.Get{model pascal}({model camel})[0] : null;
        }

        /// <summary>
        /// 查询{model pascal}表信息是否存在
        /// </summary>
        /// <param name="{model camel}">{model camel}表查询对象</param>
        /// <returns>bool是否存在</returns>
        public static bool Get{model pascal}Bool({model pascal} {model camel})
        {
            return service.Get{model pascal}({model camel}).Count > 0 ? true : false;
        }
	    #endregion

		#region  ExtensionMethod

		#endregion  ExtensionMethod
    }
}
