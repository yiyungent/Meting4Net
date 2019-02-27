using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Meting4Net.Core
{
    class Common
    {
        #region JsonStr2Obj
        public static dynamic JsonStr2Obj(string jsonStr)
        {
            return JsonConvert.DeserializeObject<dynamic>(jsonStr);
        }
        #endregion

        #region Obj2JsonStr
        public static string Obj2JsonStr(dynamic jsonObj)
        {
            return JsonConvert.SerializeObject(jsonObj);
        }
        #endregion

        #region Dynamic2JObject
        public static JObject Dynamic2JObject(dynamic jsonObj)
        {
            string jsonStr = Obj2JsonStr(jsonObj);
            JObject jObject = JsonConvert.DeserializeObject<JObject>(jsonStr);

            return jObject;
        }
        #endregion

        #region Dynamic2JArray
        public static JArray Dynamic2JArray(dynamic jsonObj)
        {
            string jsonStr = Obj2JsonStr(jsonObj);
            JArray jArray = JsonConvert.DeserializeObject<JArray>(jsonStr);

            return jArray;
        }
        #endregion

        #region 指定对象是否存在指定属性
        /// <summary>
        /// 指定对象是否存在指定属性,适用于JObject,JArray,dynamic
        /// </summary>
        /// <param name="data"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public static bool IsPropertyExist(dynamic data, string propertyname)
        {
            if (data is JObject)
            {
                Dictionary<string, object> dict = JObject2Dict(data);
                return dict.ContainsKey(propertyname);
                // Newtonsoft.Json 10.0.1 不支持 ContainsKey
                //return ((JObject)data).ContainsKey(propertyname);
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
        #endregion

        #region JObject2Dict
        public static Dictionary<string, object> JObject2Dict(JObject obj)
        {
            Dictionary<string, object> rtn = obj.ToObject<Dictionary<string, object>>();
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

        #region IP地址转换为数字
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
        #endregion

        #region 数字转换为IP地址
        /// <summary>
        /// 数字转换为IP地址
        /// </summary>
        /// <param name="ipNum">数字</param>
        /// <returns></returns>
        public static string Long2Ip(string ipNum)
        {
            long IntIp = long.Parse(ipNum);
            StringBuilder sb = new StringBuilder();
            sb.Append(IntIp >> 0x18 & 0xff).Append(".");
            sb.Append(IntIp >> 0x10 & 0xff).Append(".");
            sb.Append(IntIp >> 0x8 & 0xff).Append(".");
            sb.Append(IntIp & 0xff);
            return sb.ToString();
        }
        #endregion

        #region Dynamic2Dict
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
        #endregion

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

        #region 返回 当前 Unix 时间戳
        /// <summary>
        /// 返回 当前 Unix 时间戳（秒）
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            long unixDate = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return unixDate.ToString();
        }
        /// <summary>
        /// 返回 当前 Unix 时间戳（毫秒）
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStampMicro()
        {
            long unixDate = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            return unixDate.ToString();
        }
        #endregion

        #region Unicode编码与解码
        /// <summary>
        /// 字符串转Unicode
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        public static string String2Unicode(string source)
        {
            var bytes = Encoding.Unicode.GetBytes(source);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="source">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, x => Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)).ToString());
        }
        #endregion

        #region 生成随机字符串
        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum = true, bool useLow = true, bool useUpp = true, bool useSpe = false, string custom = "")
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        #endregion

        #region 进制转换
        //return result in specified length
        public static string hex2Bin(string strHex, int bit)
        {
            int decNumber = Hex2Dec(strHex);
            return Dec2Bin(decNumber).PadLeft(bit, '0');
        }

        //return result in specified length
        public static string Dec2Bin(int val, int bit)
        {
            return Convert.ToString(val, 2).PadLeft(bit, '0');
        }

        public static string Hex2Bin(string strHex)
        {
            int decNumber = Hex2Dec(strHex);
            return Dec2Bin(decNumber);
        }

        public static string Bin2Hex(string strBin)
        {
            int decNumber = Bin2Dec(strBin);
            return Dec2Hex(decNumber);
        }

        public static string Dec2Hex(int val)
        {
            return val.ToString("X");
            //return Convert.ToString(val,16);
        }

        public static int Hex2Dec(string strHex)
        {
            return Convert.ToInt16(strHex, 16);
        }

        public static string Dec2Bin(int val)
        {
            return Convert.ToString(val, 2);
        }

        public static int Bin2Dec(string strBin)
        {
            return Convert.ToInt16(strBin, 2);
        }
        #endregion

        #region 将字符串转为16进制字符，允许中文
        public static string StringToHexString(string s, Encoding encode)
        {
            // 按照指定编码将string编程字节数组
            byte[] b = encode.GetBytes(s);
            string result = string.Empty;
            // 逐字节变为16进制字符
            for (int i = 0; i < b.Length; i++)
            {
                result += Convert.ToString(b[i], 16);
            }
            return result;
        }
        #endregion
    }
}
