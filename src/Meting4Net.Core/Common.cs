using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace Meting4Net.Core
{
    public class Common
    {
        public static dynamic JsonStr2Obj(string jsonStr)
        {
            return JsonConvert.DeserializeObject<dynamic>(jsonStr);
        }

        public static string Obj2JsonStr(dynamic jsonObj)
        {
            return JsonConvert.SerializeObject(jsonObj);
        }

        public static JObject Dynamic2JObject(dynamic jsonObj)
        {
            string jsonStr = Obj2JsonStr(jsonObj);
            JObject jObject = JsonConvert.DeserializeObject<JObject>(jsonStr);

            return jObject;
        }

        public static JArray Dynamic2JArray(dynamic jsonObj)
        {
            string jsonStr = Obj2JsonStr(jsonObj);
            JArray jArray = JsonConvert.DeserializeObject<JArray>(jsonStr);

            return jArray;
        }

        public static bool IsPropertyExist(dynamic data, string propertyname)
        {
            if (data is JObject)
            {
                return ((JObject)data).ContainsKey(propertyname);
            }
            else
            {
                bool flag = data.GetType().GetProperty(propertyname) != null;

                return flag;
            }
        }

        public static Dictionary<string, string> JObject2Dict(dynamic obj)
        {
            if (obj is JObject)
            {
                JObject jObject = (JObject)obj;
                Dictionary<string, string> rtn = jObject.ToObject<Dictionary<string, string>>();
                return rtn;
            }
            return null;
        }

        /// <summary>
        /// IP地址转换为数字
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns></returns>
        public static string Ip2Long(string ip)
        {
            long IntIp = 0;
            string[] ips = ip.Split('.');
            IntIp = long.Parse(ips[0]) << 0x18 | long.Parse(ips[1]) << 0x10 | long.Parse(ips[2]) << 0x8 | long.Parse(ips[3]);
            return IntIp.ToString();
        }

        /// <summary>
        /// IP地址转换为数字
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns></returns>
        public static string Long2Ip(string ip)
        {
            long IntIp = long.Parse(ip);
            StringBuilder sb = new StringBuilder();
            sb.Append(IntIp >> 0x18 & 0xff).Append(".");
            sb.Append(IntIp >> 0x10 & 0xff).Append(".");
            sb.Append(IntIp >> 0x8 & 0xff).Append(".");
            sb.Append(IntIp & 0xff);
            return sb.ToString();
        }

        public static Dictionary<String, Object> Dynamic2Dict(dynamic dynObj)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(dynObj))
            {
                object obj = propertyDescriptor.GetValue(dynObj);
                dictionary.Add(propertyDescriptor.Name, obj);
            }
            return dictionary;
        }
    }
}
