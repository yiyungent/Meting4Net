using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;
using System.Security.Cryptography;

namespace Meting4Net.Core
{
    /// <summary>
    /// 加密类
    /// eg.网易云音乐数据加密
    /// </summary>
    class Encrypt
    {
        // 固定值
        private const String nonce = "0CoJUm6Qyw8W8jud";
        private const String iv = "0102030405060708";
        private const String pubKey = "010001";
        private const String modulus = "00e0b509f6259df8642dbc35662901477df22677ec152b5ff68ace615bb7" +
                                       "b725152b3ab17a876aea8a5aa76d2e417629ec4ee341f56135fccf695280" +
                                       "104e0312ecbda92557c93870114af6c9d05c4f7f0c3685b7a46bee255932" +
                                       "575cce10b424d813cfe4875d3e82047b97ddef52741d546b8e289dc6935b" +
                                       "3ece0462db0a22b8e7";

        /// <summary>
        /// 对明文数据进行加密(网易云API加密)
        /// </summary>
        /// <param name="text">明文数据</param>
        /// <returns>加密后的数据</returns>
        public static String EncryptedRequest(String text)
        {
            // key
            var secKey = CreateSecretKey(16);
            // aes
            var encTextFisrt = AesEncrypt(text, nonce, iv);
            var encText = AesEncrypt(encTextFisrt, secKey, iv);
            // rsa
            var encSecKey = RsaEncrypt(secKey, pubKey, modulus);
            var data = encText + "\n" + encSecKey;
            return data;
        }

        /// <summary>
        /// 生成随机SecKey
        /// </summary>
        /// <param name="size">key长度</param>
        /// <returns>SecKey</returns>
        private static String CreateSecretKey(Int32 size)
        {
            // String keys = "0123456789ABCDEF";
            var keys = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var key = String.Empty;
            var rd = new Random();

            for (Int32 i = 0; i < size; i++)
            {
                key += keys[rd.Next(keys.Length)].ToString();
            }

            return key;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="secKey">私钥</param>
        /// <param name="iv">iv</param>
        /// <returns>加密后的数据</returns>
        public static String AesEncrypt(String data, String secKey, String iv)
        {
            var rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 128;

            var keyBytes = Encoding.UTF8.GetBytes(secKey);
            rijndaelCipher.Key = keyBytes;

            var ivBytes = Encoding.UTF8.GetBytes(iv);
            rijndaelCipher.IV = ivBytes;

            var dataBytes = Encoding.UTF8.GetBytes(data);
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            var encryptedData = transform.TransformFinalBlock(dataBytes, 0, dataBytes.Length);

            return Convert.ToBase64String(encryptedData);

        }

        /// <summary>
        /// 生成EncSecKey
        /// </summary>
        /// <param name="text">第一个数据</param>
        /// <param name="pubKey">第二个数据</param>
        /// <param name="modulus">第三个数据</param>
        /// <returns>EncSecKey</returns>
        public static String RsaEncrypt(String text, String pubKey, String modulus)
        {
            var rs = String.Empty;

            // 反转字符串
            text = ReverseText(text);
            // 转为HEX码
            var bt = Encoding.Default.GetBytes(text);
            text = BitConverter.ToString(bt).Replace("-", "");

            // 输入为16进制
            var t = BigInteger.Parse("0" + text, System.Globalization.NumberStyles.HexNumber);
            var p = BigInteger.Parse("0" + pubKey, System.Globalization.NumberStyles.HexNumber);
            var m = BigInteger.Parse("0" + modulus, System.Globalization.NumberStyles.HexNumber);

            // 幂取模，转为10进制
            rs = BigInteger.ModPow(t, p, m).ToString("X");

            // 使用前导零填充到特定的长度
            rs = ZeroFill(rs, 256);

            return rs;
        }

        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>反转后的字符串</returns>
        private static String ReverseText(String str)
        {
            var arr = str.ToCharArray();
            Array.Reverse(arr);
            return new String(arr);
        }

        /// <summary>
        /// 使用前导0填充字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="size">要填充到的长度</param>
        /// <returns>处理后的字符串</returns>
        private static String ZeroFill(String str, Int32 size)
        {
            if (str.Length < size)
            {
                str = str.PadLeft(size, '0');
            }
            return str;
        }

    }
}
