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
using Newtonsoft.Json;
using Meting4Net.Core.Models.Standard;

namespace Meting4Net.Core
{
    public class Meting
    {
        public const string VERSION = "0.1.0";

        private string _raw;
        private string _data;
        private string _info;
        private string _error;
        private string _status;

        public string Raw { get { return _raw; } set { _raw = value; } }
        public string Data { get { return _data; } set { _data = value; } }
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

        #region 执行，返回数据
        private string Exec(Music_api api)
        {
            if (api.encode != null)
            {
                #region 废弃
                //string[] fullTypeNameAndMethodName = new string[2];
                //fullTypeNameAndMethodName[0] = this.ToString();
                //fullTypeNameAndMethodName[1] = api.encode.ToString().ToUpperInvariant()[0] + api.encode.ToString().Substring(1);
                //api = PhpCommon.Call_user_func_array(fullTypeNameAndMethodName, api); 
                #endregion

                api = api.encode(api);
            }

            // GET:url?后参数，POST: POST body内容----均为 key1=value1&key2=value2
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict = Common.Dynamic2Dict(api.body);
            string parmsData = PhpCommon.Http_build_query(dict);
            string url = api.url;
            if (api.method == "GET")
            {
                if (api.body != null && !string.IsNullOrEmpty(api.body.ToString()))
                {
                    url = api.url + "?" + parmsData;
                    parmsData = null;
                }
            }

            this.Curl(url: url, payload: parmsData);

            // 若不进行格式化，则直接返回原始数据
            if (!this.Format)
            {
                return this.Raw;
            }

            // 进行格式化，不过先将原始数据保存到 Data
            this.Data = this.Raw;

            if (api.decode != null)
            {
                #region 废弃
                //// [0]完全限定类名，[1]方法名
                //string[] arr = new string[2];
                //arr[0] = this.ToString();
                //arr[1] = api.decode.ToString()[0].ToString().ToUpperInvariant() + api.decode.ToString().Substring(1);
                //this.Data = PhpCommon.Call_user_func_array(arr, this.Data).ToString(); 
                #endregion

                this.Data = api.decode(this.Data).ToJsonStr();
            }
            if (api.format != null && !string.IsNullOrEmpty(api.format.ToString()))
            {
                this.Data = Common.Obj2JsonStr(this.Clean(this.Data, api.format));
            }

            return this.Data;
        }
        #endregion

        #region 发起HTTP请求
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
        #endregion

        #region 根据规则检索
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
                if (!Common.IsPropertyExist(array, vo.ToString()) || string.IsNullOrEmpty(array[vo].ToString()))
                {
                    return null;
                }
                array = array[vo];
            }

            return array;
        }
        #endregion

        #region 对原始json进行清理(格式化)
        private Music_search_item[] Clean(dynamic raw, string rule)
        {
            raw = Common.JsonStr2Obj(raw.ToString());
            if (!string.IsNullOrEmpty(rule))
            {
                raw = this.PickUp(raw, rule);
            }
            #region 废弃
            //string[] temp = new string[2];
            //temp[0] = this.ToString();
            //temp[1] = "Format_" + this.Server;

            //raw = Common.Dynamic2JArray(raw);
            //dynamic result = PhpCommon.Array_map(temp, raw); 
            #endregion

            Music_search_item[] result = Format_select(raw);

            return result;
        }
        #endregion

        #region 根据音乐ID获取音乐链接
        public string Url(long id, int br = 320)
        {
            Music_api api = null;
            switch (this.Server)
            {
                case "netease":
                    #region 废弃
                    //api = new
                    //{
                    //    method = "POST",
                    //    url = "http://music.163.com/api/song/enhance/player/url",
                    //    body = new
                    //    {
                    //        ids = "[" + id + "]",
                    //        br = br * 1000
                    //    },
                    //    encode = "netease_AESCBC",
                    //    decode = "netease_url"
                    //}; 
                    #endregion

                    api = new Music_api
                    {
                        method = "POST",
                        url = "http://music.163.com/api/song/enhance/player/url",
                        body = new
                        {
                            data = "{\"ids\": \"[" + id + "]\", \"br\": \"" + br * 1000 + "\"}",
                        },
                        encode = Netease_AESCBC,
                        decode = Netease_url
                    };
                    break;
                case "tencent":
                    api = new Music_api { };
                    break;
            }

            return this.Exec(api);
        }
        #endregion

        #region 搜索

        #endregion

        #region 根据歌曲ID获取
        public string Song(long id)
        {
            Music_api api = null;
            switch (this.Server)
            {
                case "netease":
                    api = new Music_api
                    {
                        method = "POST",
                        url = "http://music.163.com/api/v3/song/detail/",
                        body = new
                        {
                            data = "{\"c\":\"[{\\\"id\\\":" + id + ",\\\"v\\\":0}]\"}"
                        },
                        encode = Netease_AESCBC,
                        format = "songs"
                    };
                    break;
            }

            return this.Exec(api);
        }
        #endregion

        #region 根据专辑ID获取
        public string Album(long id)
        {
            Music_api api = null;
            switch (this.Server)
            {
                case "netease":
                    api = new Music_api
                    {
                        method = "POST",
                        url = "http://music.163.com/api/v1/album/" + id,
                        body = new
                        {
                            data = Common.Obj2JsonStr(new
                            {
                                total = "true",
                                offset = "0",
                                id = id,
                                limit = "1000",
                                ext = "true",
                                private_cloud = "true"
                            })
                        },
                        encode = Netease_AESCBC,
                        format = "songs"
                    };
                    break;
            }

            return this.Exec(api);
        }
        #endregion

        #region 网易云音乐API加密
        private static Music_api Netease_AESCBC(Music_api api)
        {
            #region 废弃
            //string body = Common.Obj2JsonStr(api.body);
            //string ids = api.body.ids.ToString();
            //string br = api.body.br.ToString();
            //string body = "{\"ids\": \"" + ids + "\", \"br\": \"" + br + "\"}"; 
            #endregion

            string encryptBody = Encrypt.EncryptedRequest(api.body.data.ToString());
            // [0] params  [1] encSecKey
            string[] encryptParms = encryptBody.Split('\n');

            #region 废弃
            //Music_api newApi = new Music_api
            //{
            //    method = api.method,
            //    url = api.url.ToString().Replace("/api/", "/weapi/"),
            //    body = new
            //    {
            //        @params = encryptParms[0],
            //        encSecKey = encryptParms[1]
            //    },
            //    encode = api.encode,
            //    decode = api.decode
            //}; 
            #endregion

            api.url = api.url.Replace("/api/", "/weapi/");
            api.body = new
            {
                @params = encryptParms[0],
                encSecKey = encryptParms[1]
            };

            return api;
        }
        #endregion

        #region 格式化选择
        private Music_search_item[] Format_select(JArray rawArray)
        {
            Del_music_item_format del_Music_Item = null;
            switch (this.Server)
            {
                case "netease":
                    del_Music_Item = Format_netease;
                    break;
            }
            List<Music_search_item> list = new List<Music_search_item>();
            JEnumerable<JToken> jTokens = rawArray.Children();
            foreach (JToken item in jTokens)
            {
                Music_search_item songItem = del_Music_Item(item);
                list.Add(songItem);
            }
            Music_search_item[] result = list.ToArray();
            return result;
        }
        #endregion

        #region 对搜索到的(单首)网易云音乐数据进行格式化
        /// <summary>
        /// 对搜索到的(单首)网易云音乐数据进行格式化
        /// </summary>
        /// <param name="songItem">(单首)网易云音乐json数据</param>
        /// <returns></returns>
        public static Music_search_item Format_netease(dynamic songItem)
        {
            #region 废弃
            //dynamic result = new
            //{
            //    id = data.id.ToString(),
            //    name = data.name.ToString(),
            //    artist = new Object(),
            //    album = data.al.name.ToString(),
            //    pic_id = Common.IsPropertyExist(data.al, "pic_str") ? data.al.pic_str.ToString() : data.al.pic.ToString(),
            //    url_id = data.id.ToString(),
            //    lyric_id = data.id.ToString(),
            //    source = "netease"
            //}; 
            #endregion

            Music_search_item result = new Music_search_item
            {
                id = songItem.id,
                name = songItem.name.ToString(),
                artist = null,
                album = songItem.al.name.ToString(),
                pic_id = Common.IsPropertyExist(songItem.al, "pic_str") ? songItem.al.pic_str.ToString() : songItem.al.pic.ToString(),
                url_id = songItem.id,
                lyric_id = songItem.id,
                source = "netease"
            };
            Match match;
            if (Common.IsPropertyExist(songItem.al, "picUrl"))
            {
                match = Regex.Match(songItem.al.picUrl.ToString(), @"\/(\d+)\.");
                result.pic_id = match.Groups[1].Value;
            }
            List<string> artistList = new List<string>();
            foreach (dynamic vo in songItem.ar)
            {
                artistList.Add(vo.name.ToString());
            }
            result.artist = artistList.ToArray();

            return result;
        }
        #endregion

        #region 设置请求头
        /// <summary>
        /// 设置请求头
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CurlSet()
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
                        { "X-Real-IP", Common.Long2Ip((new Random()).Next(1884815360, 1884890111).ToString()) },
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
        #endregion

        #region 提取(解析)网易云音乐链接
        private static Music_decode_url Netease_url(dynamic result)
        {
            string jsonStr = result.ToString();
            Models.Netease.Netease_url data = JsonConvert.DeserializeObject<Models.Netease.Netease_url>(jsonStr);
            Music_decode_url url = null;
            if (!string.IsNullOrEmpty(data.data[0].url))
            {
                url = new Music_decode_url
                {
                    url = data.data[0].url,
                    size = data.data[0].size,
                    br = data.data[0].br / 1000
                };
            }
            else
            {
                url = new Music_decode_url
                {
                    url = "",
                    size = 0,
                    br = -1
                };
            }
            return url;
        }
        #endregion
    }
}
