using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Meting4Net.Core
{
    class PhpCommon
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
        /// 普通类型： 将一个字典内的所有值均UrlEncode，且拼接为Url格式参数
        /// 普通类型： key=value&  返回拼接后的字符串
        /// 复杂json类型: 当value属性值不为单一值，即为 JObject / JArray时，直接将data转换为 json字符串返回
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
            // 标记是否是 简单类型: key1=value1&key2=value2&
            bool isSimple = true;
            foreach (string key in data.Keys)
            {
                if (data[key] is JArray)
                {
                    isSimple = false;
                    #region 废弃，这样拼接字符串，最后依然失败
                    //// eg. Kugou Url() 的API resource
                    //JArray arr = (JArray)data[key];
                    //int index = 0;
                    //foreach (JToken item in arr.Children())
                    //{
                    //    if (item is JObject)
                    //    {
                    //        JObject jObject = (JObject)item;
                    //        foreach (JProperty property in jObject.Properties())
                    //        {
                    //            // resource[0][type]=audio
                    //            string temp = string.Format("{0}[{1}][{2}]={3}", key, index, property.Name, property.Value.ToString());
                    //            sb.Append(HttpUtility.UrlEncode(temp) + "&");
                    //        }
                    //    }
                    //    index++;
                    //} 
                    #endregion

                    #region 如果属性值又为一个json数组,那么直接将整个 json字符串对象 作为请求体发送
                    sb.Clear();
                    sb.Append(Common.Dict2JObject(data).ToString());
                    break;
                    #endregion
                }
                else if (data[key] is JObject)
                {
                    isSimple = false;
                    #region 如果属性值又为一个json对象, 那么直接将整个 json字符串对象 作为请求体发送
                    sb.Clear();
                    sb.Append(Common.Dict2JObject(data).ToString());
                    break;
                    #endregion
                }
                else
                {
                    sb.Append(key + "=" + HttpUtility.UrlEncode(data[key].ToString()) + "&");
                }
            }
            string str = sb.ToString();
            if (isSimple)
            {
                // 简单类型，发送拼接参数 字符串
                return str.Substring(0, str.Length - 1);
            }
            else
            {
                // 非简单类型，直接发送整个 json字符串
                return str;
            }
        }
        #endregion

        #region MyRegion
        public static dynamic Array_map(string[] fullTypeNameAndMethodName, JArray array)
        {
            string fullTypeName = fullTypeNameAndMethodName[0];
            // 动态加载
            Assembly assembly = Assembly.Load(fullTypeNameAndMethodName[0].Substring(0, fullTypeNameAndMethodName[0].LastIndexOf(".")));
            // 得到 Type
            Type type = assembly.GetType(fullTypeName);
            // 根据方法名动态调用静态方法
            string methodName = fullTypeNameAndMethodName[1];

            // 将数组中的每个都应用此方法
            JArray jArray = array;
            JEnumerable<JToken> jTokens = jArray.Children();
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

        #region 返回相对应于 ascii 所指定的单个字符
        public static char Chr(int ascii)
        {
            //char result = (char)ascii;
            byte[] array = new byte[1];
            array[0] = (byte)(Convert.ToInt32(ascii)); //ASCII码强制转换二进制
            char result = Convert.ToString(System.Text.Encoding.ASCII.GetString(array))[0];

            return result;
        }
        #endregion

        #region 返回字符串中第一个字符的 ASCII 值
        public static int Ord(string str)
        {
            int result = (int)str[0];
            return result;
        }
        #endregion
    }
}
