/**
 * Meting music framework
 * https://yiyungent.github.io/Meting4Net
 * https://github.com/yiyungent/Meting4Net
 * Version 0.1.0
 *
 * Copyright 2019, yiyun <yiyungent@gmail.com>
 * Released under the MIT license
 */

using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Meting4Net.Core
{
    public class Meting
    {
        public const string VERSION = "0.1.0";

        private string _raw;
        private dynamic _data;
        private string _info;
        private string _error;
        private string _status;

        public string Raw { get { return _raw; } set { _raw = value; } }
        public dynamic Data { get { return _data; } set { _data = value; } }
        public string Info { get { return _info; } set { _info = value; } }
        public string Error { get { return _error; } set { _error = value; } }
        public string Status { get { return _status; } set { _status = value; } }

        private string _server;
        private string _proxy;
        private bool _format = false;
        private Dictionary<string, string> _header;

        public string Server { get { return _server; } set { _server = value; } }
        public string Proxy { get { return _proxy; } set { _proxy = value; } }
        public bool Format { get { return _format; } set { _format = value; } }
        public Dictionary<string, string> Header { get { return _header; } set { _header = value; } }

        public Meting(string value = "netease")
        {
            this.Site(value);
        }

        public Meting Site(string value)
        {
            string[] suppose = new string[] { "netease", "tencent", "xiami", "kugou", "baidu" };
            this.Server = PhpCommon.In_array(value, suppose) ? value : "netease";
            this.Header = this.CurlSet();

            return this;
        }

        public Meting Cookie(string value)
        {
            this.Header["Cookie"] = value;

            return this;
        }

        public Meting FormatMethod(bool value = true)
        {
            this.Format = value;

            return this;
        }

        public Meting ProxyMethod(string value)
        {
            this.Proxy = value;

            return this;
        }

        private dynamic Exec(dynamic api)
        {
            if (Common.IsPropertyExist(api, "encode") && !string.IsNullOrEmpty(api.encode.ToString()))
            {
                string[] fullTypeNameAndMethodName = new string[2];
                fullTypeNameAndMethodName[0] = this.ToString();
                fullTypeNameAndMethodName[1] = api.encode.ToString().ToUpperInvariant()[0] + api.encode.ToString().Substring(1);

                api = PhpCommon.Call_user_func_array(fullTypeNameAndMethodName, api);
            }

            // GET:url?后参数，POST: POST body内容----均为 key1=value1&key2=value2
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict = Common.Dyn2Dict(api.body);
            string parmsData = PhpCommon.Http_build_query(dict);
            string url = api.url.ToString();
            if (api.method.ToString() == "GET")
            {
                if (Common.IsPropertyExist(api, "body") && !string.IsNullOrEmpty(api.body.ToString()))
                {
                    url = api.url.ToString() + "?" + parmsData;
                    //api.body = null;
                    parmsData = null;
                }
            }
            else if (api.method.ToString() == "POST")
            {
                //api.body = parmsData;
            }

            //this.Curl(url: api.url.ToString(), payload: api.body.ToString());
            this.Curl(url: url, payload: parmsData);

            // 若不进行格式化，则直接返回原始数据
            if (!this.Format)
            {
                return this.Raw;
            }

            // 进行格式化，不过先将原始数据保存到 Data
            this.Data = this.Raw;

            if (Common.IsPropertyExist(api, "decode") && !string.IsNullOrEmpty(api.decode.ToString()))
            {
                // [0]完全限定类名，[1]方法名
                string[] arr = new string[2];
                arr[0] = this.ToString();
                arr[1] = api.decode.ToString();
                this.Data = PhpCommon.Call_user_func_array(arr, this.Data).ToString();
            }
            if (Common.IsPropertyExist(api, "format") && !string.IsNullOrEmpty(api.format.ToString()))
            {
                this.Data = this.Clean(this.Data, api.format.ToString());
            }

            return this.Data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="payload"></param>
        /// <param name="headerHave">返回的字符串中是否包含 响应头(Response Headers)</param>
        /// <returns></returns>
        private Meting Curl(string url, string payload = null, bool headerHave = false)
        {
            List<string> headers = new List<string>();
            foreach (string key in this.Header.Keys)
            {
                headers.Add(key + ": " + this.Header[key]);
            }

            string postDataStr = "";
            string responseData = "";
            StringBuilder responseHeadersSb = new StringBuilder();
            if (payload != null)
            {


                postDataStr = payload;
                responseData = HttpAide.HttpPost(url: url, postDataStr: postDataStr, responseHeadersSb: responseHeadersSb, headers: headers.ToArray());
            }
            else
            {
                responseData = HttpAide.HttpGet(url: url, responseHeadersSb: responseHeadersSb, headers: headers.ToArray());
            }
            if (headerHave)
            {
                responseData = responseHeadersSb.ToString() + "\r\n\r\n" + responseData;
            }
            this.Raw = responseData;

            return this;
        }

        /// <summary>
        /// 按照规则深入查询json, 例如 result.songs 则将查询json第一层的result再进入songs，抓取songs下的所有
        /// </summary>
        /// <param name="array"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        private dynamic PickUp(dynamic array, string rule)
        {
            string[] t = rule.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string vo in t)
            {
                if (!Common.IsPropertyExist(array, vo.ToString()) || string.IsNullOrEmpty(array[vo]))
                {
                    return null;
                }
                array = array[vo];
            }

            return array;
        }

        private dynamic Clean(dynamic raw, string rule)
        {
            raw = Common.JsonStr2Obj(raw.ToString());
            if (!string.IsNullOrEmpty(rule))
            {
                raw = this.PickUp(raw, rule);
            }
            string[] temp = new string[2];
            temp[0] = this.ToString();
            temp[1] = "Format_" + this.Server;

            dynamic result = PhpCommon.Array_map(temp, raw);

            return result;
        }

        public dynamic Url(string id, int br = 320)
        {
            dynamic api = new JObject();
            switch (this.Server)
            {
                case "netease":
                    api = new
                    {
                        method = "POST",
                        url = "http://music.163.com/api/song/enhance/player/url",
                        body = new
                        {
                            ids = "[" + id + "]",
                            br = br * 1000
                        },
                        encode = "netease_AESCBC",
                        decode = "netease_url"
                    };
                    break;
                case "tencent":
                    api = new { };
                    break;
            }

            return this.Exec(api);
        }

        public static dynamic Netease_AESCBC(dynamic api)
        {
            //string body = Common.Obj2JsonStr(api.body);
            string ids = api.body.ids.ToString();
            string br = api.body.br.ToString();
            string body = "{\"ids\": \"" + ids + "\", \"br\": \"" + br + "\"}";
            string encryptBody = Encrypt.EncryptedRequest(body);
            // [0] params  [1] encSecKey
            string[] encryptParms = encryptBody.Split('\n');

            dynamic newApi = new
            {
                method = api.method,
                url = api.url.ToString().Replace("/api/", "/weapi/"),
                body = new
                {
                    @params = encryptParms[0],
                    encSecKey = encryptParms[1]
                },
                encode = api.encode,
                decode = api.decode
            };

            return newApi;
        }

        private dynamic Format_netease(dynamic data)
        {
            dynamic result = new
            {
                id = data.id.ToString(),
                name = data.name.ToString(),
                artist = new Object(),
                album = data.al.name.ToString(),
                pic_id = Common.IsPropertyExist(data.al, "pic_str") ? data.al.pic_str.ToString() : data.al.pic.ToString(),
                url_id = data.id.ToString(),
                lyric_id = data.id.ToString(),
                source = "netease"
            };
            Match match;
            if (Common.IsPropertyExist(data.al, "picUrl"))
            {
                match = Regex.Match(data.al.picUrl.ToString(), @"\/(\d+)\.");
                result.pic_id = match.Groups[1].Value;
            }
            int index = 0;
            foreach (dynamic vo in data.ar)
            {
                result.artist.Add(index.ToString(), vo.name);
                index++;
            }

            return result;
        }

        public Dictionary<string, string> CurlSet()
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            switch (this.Server)
            {
                case "netease":
                    header = new Dictionary<string, string>
                    {
                        { "Referer", "https://music.163.com/" },
                        { "Cookie", "appver=1.5.9; os=osx; __remember_me=true; osver=%E7%89%88%E6%9C%AC%2010.13.5%EF%BC%88%E7%89%88%E5%8F%B7%2017F77%EF%BC%89;" },
                        { "User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_5) AppleWebKit/605.1.15 (KHTML, like Gecko)" },
                        { "X-Real-IP", Common.LongToIp((new Random()).Next(1884815360, 1884890111).ToString()) },
                        { "Accept", "*/*" },
                        { "Accept-Language", "zh-CN,zh;q=0.8,gl;q=0.6,zh-TW;q=0.4" },
                        { "Connection", "keep-alive" },
                        { "Content-Type", "application/x-www-form-urlencoded" }
                    };
                    break;
                case "tencent":
                    break;
            }

            return header;
        }


    }
}
