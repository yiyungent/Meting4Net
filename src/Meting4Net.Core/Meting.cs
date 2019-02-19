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

            if (string.IsNullOrEmpty(this.Raw))
            {
                return "异常: 未查询到数据";
            }

            // 若不进行格式化，则直接返回原始数据
            if (!this.Format)
            {
                return this.Raw;
            }

            // 进行格式化，不过先将原始数据保存到 Data
            this.Data = this.Raw;

            if (api.decode != null)
            {
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

            Music_search_item[] result = Format_select(raw);

            return result;
        }
        #endregion

        #region 搜索
        public string Search(string keyword, Options options = null)
        {
            #region 当未提供 options 时则为 null,此时对其进行 new ，使其不为 null,但其中的初始化属性仍为null，这样因为默认 options 不为null,所以下方判断时不再需要判断 options!=null
            if (options == null)
            {
                options = new Options();
            }
            #endregion

            Music_api api = null;
            switch (this.Server)
            {
                case "netease":
                    api = new Music_api
                    {
                        method = "POST",
                        url = "http://music.163.com/api/cloudsearch/pc",
                        body = Common.Dynamic2JObject(new
                        {
                            s = keyword,
                            type = options.type != null ? options.type : 1,
                            limit = options.limit != null ? options.limit : 30,
                            total = "true",
                            offset = options.page != null && options.limit != null ? (options.page - 1) * options.limit : 0
                        }),
                        encode = Netease_AESCBC,
                        format = "result.songs"
                    };
                    break;
            }

            return this.Exec(api);
        }
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
                        body = Common.Dynamic2JObject(new
                        {
                            c = "[{\"id\":" + id + ",\"v\":0}]"
                        }),
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
                        body = Common.Dynamic2JObject(new
                        {
                            total = "true",
                            offset = "0",
                            id = id,
                            limit = "1000",
                            ext = "true",
                            private_cloud = "true"
                        }),
                        encode = Netease_AESCBC,
                        format = "songs"
                    };
                    break;
            }

            return this.Exec(api);
        }
        #endregion

        #region 根据作者ID获取
        public string Artist(long id, int limit = 50)
        {
            Music_api api = null;
            switch (this.Server)
            {
                case "netease":
                    api = new Music_api
                    {
                        method = "POST",
                        url = "http://music.163.com/api/v1/artist/" + id,
                        body = Common.Dynamic2JObject(new
                        {
                            ext = "true",
                            private_cloud = "true",
                            top = limit,
                            id = id
                        }),
                        encode = Netease_AESCBC,
                        format = "hotSongs"
                    };
                    break;
            }

            return this.Exec(api);
        }
        #endregion

        #region 根据歌单ID获取
        public string Playlist(long id)
        {
            Music_api api = null;
            switch (this.Server)
            {
                case "netease":
                    api = new Music_api
                    {
                        method = "POST",
                        url = "http://music.163.com/api/v3/playlist/detail",
                        body = Common.Dynamic2JObject(new
                        {
                            s = "0",
                            id = id,
                            n = "1000",
                            t = "0"
                        }),
                        encode = Netease_AESCBC,
                        format = "playlist.tracks"
                    };
                    break;
            }

            return this.Exec(api);
        }
        #endregion

        #region 根据音乐ID获取音乐链接
        public string Url(long id, int br = 320)
        {
            Music_api api = null;
            switch (this.Server)
            {
                case "netease":
                    api = new Music_api
                    {
                        method = "POST",
                        url = "http://music.163.com/api/song/enhance/player/url",
                        body = Common.Dynamic2JObject(new
                        {
                            ids = "[" + id + "]",
                            br = br * 1000
                        }),
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

        #region 根据歌曲ID查歌词
        public string Lyric(long id)
        {
            Music_api api = null;
            switch (this.Server)
            {
                case "netease":
                    api = new Music_api
                    {
                        method = "POST",
                        url = "http://music.163.com/api/song/lyric",
                        body = Common.Dynamic2JObject(new
                        {
                            id = id,
                            os = "linux",
                            lv = -1,
                            kv = -1,
                            tv = -1
                        }),
                        encode = Netease_AESCBC,
                        decode = Netease_lyric
                    };
                    break;
            }

            return this.Exec(api);
        }
        #endregion

        #region 歌曲图片(对指定歌曲编号，返回图片地址)
        public string Pic(long id, int size = 300)
        {
            string picUrl = string.Empty;
            switch (this.Server)
            {
                case "netease":
                    picUrl = "https://p3.music.126.net/" + this.Netease_encryptId(id) + "/" + id + ".jpg?param=" + size + "y" + size;
                    break;
            }

            string jsonStr = Common.Obj2JsonStr(new
            {
                url = picUrl
            });
            return jsonStr;
        }
        #endregion

        #region 网易云音乐API加密
        private static Music_api Netease_AESCBC(Music_api api)
        {
            string bodyJsonStr = Common.Obj2JsonStr(api.body);
            string encryptBody = Encrypt.EncryptedRequest(bodyJsonStr);
            // [0] params  [1] encSecKey
            string[] encryptParms = encryptBody.Split('\n');

            api.url = api.url.Replace("/api/", "/weapi/");

            api.body = Common.Dynamic2JObject(new
            {
                @params = encryptParms[0],
                encSecKey = encryptParms[1]
            });

            return api;
        }
        #endregion

        #region 网易云音乐歌曲ID加密
        public string Netease_encryptId(long id)
        {
            char[] magic = "3go8&$8*3*3h0k(2)2".ToCharArray();
            char[] song_id = id.ToString().ToCharArray();
            for (int i = 0; i < song_id.Length; i++)
            {
                int temp1 = PhpCommon.Ord(song_id[i].ToString());
                int temp2 = PhpCommon.Ord(magic[i % magic.Length].ToString());
                int temp3 = temp1 ^ temp2;
                song_id[i] = PhpCommon.Chr(temp3);
            }

            #region 不可行
            //string md5Temp = Common.MD5Encrypt16(string.Join("", song_id));
            //// 此处不一致
            //string result = Common.EncodeBase64("utf-8", md5Temp); 
            #endregion

            byte[] temp4 = Common.MD5Encrypt16(string.Join("", song_id), true);
            string result = Common.EncodeBase64(temp4);
            result = result.Replace("/", "+").Replace("_", "-");

            return result;
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
            Dictionary<string, string> header = null;
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

        #region 提取(解析)网易云音乐歌词
        private static Music_decode_lyric Netease_lyric(dynamic result)
        {
            string jsonStr = result.ToString();
            Models.Netease.Netease_lyric data = JsonConvert.DeserializeObject<Models.Netease.Netease_lyric>(jsonStr);
            Music_decode_lyric lyric = new Music_decode_lyric
            {
                lyric = data.lrc != null && !string.IsNullOrEmpty(data.lrc.lyric) ? data.lrc.lyric : "",
                tlyric = data.tlyric != null && !string.IsNullOrEmpty(data.tlyric.lyric) ? data.tlyric.lyric : ""
            };

            return lyric;
        }
        #endregion
    }
}
