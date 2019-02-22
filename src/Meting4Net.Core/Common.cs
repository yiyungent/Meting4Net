using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Security.Cryptography;

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
            else if (data is JArray)
            {
                try
                {
                    return ((JArray)data)[Convert.ToInt32(propertyname)] != null;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                bool flag = data.GetType().GetProperty(propertyname) != null;

                return flag;
            }
        }

        #region JObject2Dict
        public static Dictionary<string, object> JObject2Dict(JObject obj)
        {
            JObject jObject = (JObject)obj;
            Dictionary<string, object> rtn = jObject.ToObject<Dictionary<string, object>>();
            return rtn;
        }
        #endregion

        #region Dict2JObject
        public static JObject Dict2JObject(Dictionary<string, object> dict)
        {
            JObject jObject = new JObject();
            foreach (string key in dict.Keys)
            {
                if (dict[key] is JObject)
                {
                    jObject.Add(key, (JObject)dict[key]);
                }
                else if (dict[key] is JArray)
                {
                    jObject.Add(key, (JArray)dict[key]);
                }
                else
                {
                    jObject.Add(key, dict[key].ToString());
                }
            }

            return jObject;
        }
        #endregion

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

        #region Base64方式的编码与解码
        ///编码
        public static string EncodeBase64(string code_type, string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            //byte[] bytes = Encoding.UTF8.GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }
        ///编码
        public static string EncodeBase64(byte[] bytes)
        {
            string encode = "";
            encode = Convert.ToBase64String(bytes);

            return encode;
        }
        ///解码
        public static string DecodeBase64(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(string str)
        {
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }

        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static byte[] MD5Encrypt16(string str, bool flag)
        {
            //获取加密服务  
            MD5CryptoServiceProvider md5CSP = new MD5CryptoServiceProvider();
            //获取要加密的字段，并转化为Byte[]数组  
            byte[] testEncrypt = System.Text.Encoding.UTF8.GetBytes(str);
            //加密Byte[]数组  
            byte[] resultEncrypt = md5CSP.ComputeHash(testEncrypt);

            return resultEncrypt;
        }
        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encrypt32(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母
                // X 表示大写， x 表示小写， X2和x2表示不省略首位为0的十六进制数
                pwd = pwd + s[i].ToString("x2");
            }
            return pwd;
        }
        /// <summary>
        /// 64位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encrypt64(string str)
        {
            string cl = str;
            //string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }
        #endregion
    }
}
