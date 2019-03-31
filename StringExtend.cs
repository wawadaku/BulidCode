using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace code
{
    /// <summary>
    /// String类的扩展方法
    /// </summary>
    public static class StringExtend
    {
        public static string ToPascal(this string str)
        {
            str = str.Trim(new char[] { '[', ']' }).Trim();
            string[] strArray = str.Split(new char[] { ' ', '_' });
            StringBuilder ret = new StringBuilder();
            foreach (string s in strArray)
            {
                ret.Append(s.Substring(0, 1).ToUpper() + s.Substring(1));
            }
            return ret.ToString();
        }

        public static string ToCamel(this string str)
        {
            str = str.Trim(new char[] { '[', ']' }).Trim();
            string[] strArray = str.Split(new char[] { ' ', '_' });
            StringBuilder ret = new StringBuilder();
            ret.Append(strArray[0].Substring(0, 1).ToLower() + strArray[0].Substring(1));
            for (int i = 1; i < strArray.Length; i++)
            {
                ret.Append(strArray[i].Substring(0, 1).ToUpper() + strArray[i].Substring(1));
            }
            return ret.ToString();
        }

        public static string ToDelKh(this string str)
        {
            return str.Trim(new char[] { '[', ']' });
        }
    }
}
