using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Web;

namespace Meting4Net.Core
{
    public class PhpCommon
    {
        #region 指定字符串是否在字符串数组中
        public static bool In_array(string value, string[] array)
        {
            foreach (string str in array)
            {
                if (str == value) return true;
            }
            return false;
        }
        #endregion

        #region 动态调用静态方法
        /// <summary>
        /// 根据命名空间.类名 方法名 动态调用静态方法
        /// </summary>
        /// <param name="fullTypeNameAndMethodName">[0]命名空间.类名 [1]方法名</param>
        /// <param name="parms">方法参数</param>
        /// <returns>返回 指定方法的返回值</returns>
        public static dynamic Call_user_func_array(string[] fullTypeNameAndMethodName, params object[] parms)
        {
            string fullTypeName = fullTypeNameAndMethodName[0];
            // 动态加载
            Assembly assembly = Assembly.Load(fullTypeNameAndMethodName[0].Substring(0, fullTypeNameAndMethodName[0].LastIndexOf(".")));
            // 得到 Type
            Type type = assembly.GetType(fullTypeName);
            // 根据方法名动态调用静态方法
            string methodName = fullTypeNameAndMethodName[1];
            object rtn = type.InvokeMember(methodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, null, parms);
            return rtn;
        }
        #endregion

        #region 将一个字典内的所有值均UrlEncode，且拼接为Url格式参数
        /// <summary>
        /// 将一个字典内的所有值均UrlEncode，且拼接为Url格式参数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Http_build_query(Dictionary<string, object> data)
        {
            if (data != null && data.Count == 0)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            foreach (string key in data.Keys)
            {
                sb.Append(key + "=" + HttpUtility.UrlEncode(data[key].ToString()) + "&");
            }
            string str = sb.ToString();
            return str.Substring(0, str.Length - 1);
        }
        #endregion

        #region MyRegion
        public static dynamic Array_map(string[] fullTypeNameAndMethodName, dynamic array)
        {
            string fullTypeName = fullTypeNameAndMethodName[0];
            // 动态加载
            Assembly assembly = Assembly.Load(fullTypeNameAndMethodName[0].Substring(0, fullTypeNameAndMethodName[0].LastIndexOf(".")));
            // 得到 Type
            Type type = assembly.GetType(fullTypeName);
            // 根据方法名动态调用静态方法
            string methodName = fullTypeNameAndMethodName[1];

            // 将数组中的每个都应用此方法
            JObject jObject = (JObject)array;
            JEnumerable<JToken> jTokens = jObject.Children();
            JObject result = new JObject();
            foreach (JToken item in jTokens)
            {
                object temp = type.InvokeMember(methodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, null, new object[] { item });
                JToken jToken = (JToken)temp;
                result.Add(jToken);
            }

            return result;
        }
        #endregion
    }
}
