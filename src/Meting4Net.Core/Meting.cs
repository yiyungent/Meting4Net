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
    #region 音乐API 服务提供者
    /// <summary>
    /// 音乐API 服务提供者
    /// </summary>
    public enum ServerProvider
    {
        /// <summary>
        /// 网易云音乐
        /// </summary>
        Netease = 0,
        /// <summary>
        /// 腾讯QQ音乐
        /// </summary>
        Tencent = 1,
        /// <summary>
        /// 酷狗音乐
        /// </summary>
        Kugou = 2,
        /// <summary>
        /// 虾米音乐
        /// </summary>
        Xiami = 3,
        /// <summary>
        /// 百度(千千)音乐
        /// </summary>
        Baidu = 4
    }
    #endregion

    /// <summary>
    /// 音乐API
    /// </summary>
    public class Meting
    {
        /// <summary>
        /// 当前版本
        /// </summary>
        public const string VERSION = "1.1.2";

        private string _raw;
        private string _data;
        //private string _info;
        //private string _error;
        //private string _status;
        private int _tryCount = 3;
        private int _br;

        /// <summary>
        /// 获取的原始json数据
        /// </summary>
        public string Raw { get { return _raw; } protected set { _raw = value; } }

        /// <summary>
        /// 如果格式化则为格式化后json,未格式化则同 Raw
        /// </summary>
        public string Data { get { return _data; } protected set { _data = value; } }

        //public string Info { get { return _info; } set { _info = value; } }
        //public string Error { get { return _error; } set { _error = value; } }
        //public string Status { get { return _status; } set { _status = value; } }

        /// <summary>
        /// HTTP请求尝试次数，默认 3（当未查询到数据时，或查询出错时，尝试再次查询的次数）
        /// </summary>
        public int TryCount { get { return _tryCount; } set { _tryCount = value; } }

        /// <summary>
        /// 歌曲 比特率
        /// </summary>
        protected int Br { get { return _br; } set { _br = value; } }

        private ServerProvider _server;
        private MetingProxy _proxy;
        private bool _format = false;
        private Dictionary<string, string> _header;

        /// <summary>
        /// 音乐API 服务提供者
        /// </summary>
        public ServerProvider Server { get { return _server; } set { _server = value; } }

        /// <summary>
        /// 代理
        /// </summary>
        public MetingProxy Proxy { get { return _proxy; } set { _proxy = value; } }

        /// <summary>
        /// 是否格式化, 默认 false
        /// </summary>
        public bool Format { get { return _format; } set { _format = value; } }

        /// <summary>
        /// 请求头
        /// </summary>
        protected Dictionary<string, string> Header { get { return _header; } set { _header = value; } }

        #region 初始化
        /// <summary>
        /// 初始化音乐API 服务提供者
        /// </summary>
        /// <param name="value"></param>
        public Meting(ServerProvider value = ServerProvider.Netease)
        {
            this.Site(value);
        }
        #endregion

        #region 设置音乐API 服务提供者 (初始化Server, Header)
        /// <summary>
        /// 设置音乐API 服务提供者 (初始化Server, Header)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Meting Site(ServerProvider value)
        {
            this.Server = value;
            this.Header = this.CurlSet();

            return this;
        }
        #endregion

        #region 自定义Cookie
        /// <summary>
        /// 自定义Cookie
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Meting Cookie(string value)
        {
            this.Header["Cookie"] = value;

            return this;
        }
        #endregion

        #region 是否格式化
        /// <summary>
        /// 是否格式化
        /// </summary>
        /// <param name="value">默认格式化 true</param>
        /// <returns></returns>
        public Meting FormatMethod(bool value = true)
        {
            this.Format = value;

            return this;
        }
        #endregion

        #region 设置代理
        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Meting ProxyMethod(MetingProxy value)
        {
            this.Proxy = value;

            return this;
        }
        #endregion

        #region 执行，返回数据
        /// <summary>
        /// 执行，返回数据
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        protected string Exec(Music_api api)
        {
            if (api.encode != null)
            {
                api = api.encode(api);
            }

            string sendData = string.Empty;
            #region 判断发送数据的类型
            if (api.sendDataType == SendDataType.KeyValueParm)
            {
                // GET:url?后参数，POST: POST body内容----均为 key1=value1&key2=value2
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict = Common.JObject2Dict((JObject)api.body);
                // 转换为 key1=value1&key2=value2
                string parmsData = PhpCommon.Http_build_query(dict);

                sendData = parmsData;
            }
            else if (api.sendDataType == SendDataType.Json)
            {
                // PS: 其实发送 json 数据的话，只能是 post 请求，将发送的json放到请求体中
                sendData = api.body.ToString();
            }
            #endregion

            string url = api.url;
            if (api.method.Equals("GET", StringComparison.InvariantCultureIgnoreCase))
            {
                if (api.body != null && !string.IsNullOrEmpty(api.body.ToString()))
                {
                    // GET 请求，则直接将要发送数据放到 url 后
                    url = api.url + "?" + sendData;
                    sendData = null;
                }
            }

            // Curl() 会根据 sendData是否为null, 而选择 GET 还是 POST, sendData为null,则GET
            this.Curl(url: url, payload: sendData);

            #region 判断查询结果是否有误
            if (string.IsNullOrEmpty(this.Raw) || this.Raw.Contains("参数错误"))
            {
                string errJsonStr = Common.Obj2JsonStr(new
                {
                    code = -1,
                    msg = "未查询到数据",
                    message = this.Raw
                });
                return errJsonStr;
            }
            #endregion

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
            if (api.format != null)
            {
                this.Data = Common.Obj2JsonStr(this.Clean(this.Data, api.format));
            }

            return this.Data;
        }
        #endregion

        #region 发起HTTP请求
        /// <summary>
        /// 发起HTTP请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="payload"></param>
        /// <param name="headerHave">返回的字符串中是否包含 响应头(Response Headers)</param>
        /// <returns></returns>
        protected Meting Curl(string url, string payload = null, bool headerHave = false)
        {
            List<string> headers = new List<string>();
            foreach (string key in this.Header.Keys)
            {
                headers.Add(key + ": " + this.Header[key]);
            }

            string postDataStr = "";
            string responseData = "";
            StringBuilder responseHeadersSb = new StringBuilder();
            // HTTP 请求尝试，若响应体 为 空，则会再次尝试，直到不为空 或则 尝试次数用完
            for (int i = 0; i < this.TryCount; i++)
            {
                if (payload != null)
                {
                    postDataStr = payload;
                    responseData = HttpAide.HttpPost(url: url, postDataStr: postDataStr, responseHeadersSb: responseHeadersSb, headers: headers.ToArray(), proxy: this.Proxy != null ? this.Proxy.Proxy : null);
                }
                else
                {
                    responseData = HttpAide.HttpGet(url: url, responseHeadersSb: responseHeadersSb, headers: headers.ToArray(), proxy: this.Proxy != null ? this.Proxy.Proxy : null);
                }
                if (!string.IsNullOrEmpty(responseData)) break;
            }
            // 若需要响应头，则添加响应头
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
        protected dynamic PickUp(dynamic array, string rule)
        {
            string[] t = rule.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string vo in t)
            {
                if (!Common.IsPropertyExist(array, vo.ToString()))
                {
                    return null;
                }
                if (array is JObject)
                {
                    array = array[vo];
                }
                else if (array is JArray)
                {
                    int voInt = Convert.ToInt32(vo);
                    array = array[voInt];
                }
                else
                {
                    return null;
                }
            }

            return array;
        }
        #endregion

        #region 对原始json进行清理(格式化)
        /// <summary>
        /// 对原始json进行清理(格式化)
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        protected Music_search_item[] Clean(dynamic raw, string rule)
        {
            // 根据 json字符串 第一层是 {}: JObject , 还是 []: JArray 确定转换为哪种类型的对象
            raw = Common.JsonStr2Obj(raw.ToString());

            if (!string.IsNullOrEmpty(rule))
            {
                raw = this.PickUp(raw, rule);
            }
            if (raw is JObject)
            {
                raw = new JArray
                {
                    new JObject(raw)
                };
            }
            // Note: 向 Format_select() 的参数类型必须是 JArray
            Music_search_item[] result = Format_select(raw);

            return result;
        }
        #endregion

        #region 搜索歌曲
        /// <summary>
        /// 搜索歌曲
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="options"></param>
        /// <returns>返回json字符串</returns>
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
                case ServerProvider.Netease:
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
                case ServerProvider.Tencent:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "https://c.y.qq.com/soso/fcgi-bin/client_search_cp",
                        body = Common.Dynamic2JObject(new
                        {
                            format = "json",
                            p = options.page != null ? options.page : 1,
                            n = options.limit != null ? options.limit : 30,
                            w = keyword,
                            aggr = 1,
                            lossless = 1,
                            cr = 1,
                            new_json = 1
                        }),
                        format = "data.song.list"
                    };
                    break;
                case ServerProvider.Kugou:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://mobilecdn.kugou.com/api/v3/search/song",
                        body = Common.Dynamic2JObject(new
                        {
                            api_ver = 1,
                            area_code = 1,
                            correct = 1,
                            pagesize = options.limit != null ? options.limit : 30,
                            plat = 2,
                            tag = 1,
                            sver = 5,
                            showtype = 10,
                            page = options.page != null ? options.page : 1,
                            keyword = keyword,
                            version = 8990
                        }),
                        format = "data.info"
                    };
                    break;
                case ServerProvider.Xiami:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://h5api.m.xiami.com/h5/mtop.alimusic.search.searchservice.searchsongs/1.0/",
                        body = Common.Dynamic2JObject(new
                        {
                            data = new JObject
                            {
                                // 注意: 需对搜索关键字 unicode
                                { "key",Common.String2Unicode( keyword) },
                                { "pagingVO", new JObject {
                                    { "page", options.page != null ? options.page: 1 },
                                    { "pageSize", options.limit != null ? options.limit : 30 }
                                } }
                            },
                            r = "mtop.alimusic.search.searchservice.searchsongs"
                        }),
                        encode = Xiami_sign,
                        format = "data.data.songs"
                    };
                    break;
                case ServerProvider.Baidu:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://musicapi.taihe.com/v1/restserver/ting",
                        body = Common.Dynamic2JObject(new
                        {
                            from = "qianqianmini",
                            method = "baidu.ting.search.merge",
                            isNew = 1,
                            platform = "darwin",
                            page_no = options.page != null ? options.page : 1,
                            query = keyword,
                            version = "11.2.1",
                            page_size = options.limit != null ? options.limit : 30
                        }),
                        format = "result.song_info.song_list"
                    };
                    break;
            }

            return this.Exec(api);
        }

        /// <summary>
        /// 搜索歌曲
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="options"></param>
        /// <returns>返回实体对象</returns>
        public Music_search_item[] SearchObj(string keyword, Options options = null)
        {
            // 先保存其当前 Format
            bool tempFormat = this.Format;
            // 必须临时设置 Format=true
            this.Format = true;
            string jsonStr = Search(keyword, options);
            Music_search_item[] rtn = MetingTool.MusicJson2Obj(jsonStr);
            // 用完后设置回原来Format, 从而不影响 this.Format
            this.Format = tempFormat;

            return rtn;
        }
        #endregion

        #region 根据歌曲ID获取
        /// <summary>
        /// 根据歌曲ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回json字符串</returns>
        public string Song(string id)
        {
            Music_api api = null;
            switch (this.Server)
            {
                case ServerProvider.Netease:
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
                case ServerProvider.Tencent:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "https://c.y.qq.com/v8/fcg-bin/fcg_play_single_song.fcg",
                        body = Common.Dynamic2JObject(new
                        {
                            songmid = id,
                            platform = "yqq",
                            format = "json"
                        }),
                        format = "data"
                    };
                    break;
                case ServerProvider.Kugou:
                    api = new Music_api
                    {
                        method = "POST",
                        url = "http://m.kugou.com/app/i/getSongInfo.php",
                        body = Common.Dynamic2JObject(new
                        {
                            cmd = "playInfo",
                            hash = id,
                            from = "mkugou"
                        }),
                        format = ""
                    };
                    break;
                case ServerProvider.Xiami:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://h5api.m.xiami.com/h5/mtop.alimusic.music.songservice.getsongdetail/1.0/",
                        body = Common.Dynamic2JObject(new
                        {
                            data = new JObject
                            {
                                { "songId", id }
                            },
                            r = "mtop.alimusic.music.songservice.getsongdetail"
                        }),
                        encode = Xiami_sign,
                        format = "data.data.songDetail"
                    };
                    break;
                case ServerProvider.Baidu:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://musicapi.taihe.com/v1/restserver/ting",
                        body = Common.Dynamic2JObject(new
                        {
                            from = "qianqianmini",
                            method = "baidu.ting.song.getInfos",
                            songid = id,
                            res = 1,
                            platform = "darwin",
                            version = "1.0.0"
                        }),
                        encode = Baidu_AESCBC,
                        format = "songinfo"
                    };
                    break;
            }

            return this.Exec(api);
        }

        /// <summary>
        /// 根据歌曲ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回实体对象</returns>
        public Music_search_item SongObj(string id)
        {
            bool tempFormat = this.Format;
            this.Format = true;
            string jsonStr = Song(id);
            Music_search_item rtn = MetingTool.MusicJson2Obj(jsonStr)[0];
            this.Format = tempFormat;

            return rtn;
        }
        #endregion

        #region 根据专辑ID获取
        /// <summary>
        /// 根据专辑ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回json字符串</returns>
        public string Album(string id)
        {
            Music_api api = null;
            switch (this.Server)
            {
                case ServerProvider.Netease:
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
                case ServerProvider.Tencent:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "https://c.y.qq.com/v8/fcg-bin/fcg_v8_album_detail_cp.fcg",
                        body = Common.Dynamic2JObject(new
                        {
                            albummid = id,
                            platform = "mac",
                            format = "json",
                            newsong = 1
                        }),
                        format = "data.getSongInfo"
                    };
                    break;
                case ServerProvider.Kugou:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://mobilecdn.kugou.com/api/v3/album/song",
                        body = Common.Dynamic2JObject(new
                        {
                            albumid = id,
                            area_code = 1,
                            plat = 2,
                            page = 1,
                            pagesize = -1,
                            version = 8990
                        }),
                        format = "data.info"
                    };
                    break;
                case ServerProvider.Xiami:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://h5api.m.xiami.com/h5/mtop.alimusic.music.albumservice.getalbumdetail/1.0/",
                        body = Common.Dynamic2JObject(new
                        {
                            data = new JObject
                            {
                                { "albumId", id }
                            },
                            r = "mtop.alimusic.music.albumservice.getalbumdetail"
                        }),
                        encode = Xiami_sign,
                        format = "data.data.albumDetail.songs"
                    };
                    break;
                case ServerProvider.Baidu:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://musicapi.taihe.com/v1/restserver/ting",
                        body = Common.Dynamic2JObject(new
                        {
                            from = "qianqianmini",
                            method = "baidu.ting.album.getAlbumInfo",
                            album_id = id,
                            platform = "darwin",
                            version = "11.2.1"
                        }),
                        format = "songlist"
                    };
                    break;
            }

            return this.Exec(api);
        }

        /// <summary>
        /// 根据专辑ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回实体对象</returns>
        public Music_search_item[] AlbumObj(string id)
        {
            bool tempFormat = this.Format;
            this.Format = true;
            string jsonStr = Album(id);
            Music_search_item[] rtn = MetingTool.MusicJson2Obj(jsonStr);
            this.Format = tempFormat;

            return rtn;
        }
        #endregion

        #region 根据作家ID获取
        /// <summary>
        /// 根据作家ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="limit"></param>
        /// <returns>返回json字符串</returns>
        public string Artist(string id, int limit = 50)
        {
            Music_api api = null;
            switch (this.Server)
            {
                case ServerProvider.Netease:
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
                case ServerProvider.Tencent:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "https://c.y.qq.com/v8/fcg-bin/fcg_v8_singer_track_cp.fcg",
                        body = Common.Dynamic2JObject(new
                        {
                            singermid = id,
                            begin = 0,
                            num = limit,
                            order = "listen",
                            platform = "mac",
                            newsong = 1
                        }),
                        format = "data.list"
                    };
                    break;
                case ServerProvider.Kugou:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://mobilecdn.kugou.com/api/v3/singer/song",
                        body = Common.Dynamic2JObject(new
                        {
                            singerid = id,
                            area_code = 1,
                            page = 1,
                            plat = 0,
                            pagesize = limit,
                            version = 8990
                        }),
                        format = "data.info"
                    };
                    break;
                case ServerProvider.Xiami:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://h5api.m.xiami.com/h5/mtop.alimusic.music.songservice.getartistsongs/1.0/",
                        body = Common.Dynamic2JObject(new
                        {
                            data = new JObject
                            {
                                { "artistId", id },
                                { "pagingVO", new JObject
                                {
                                    { "page", 1 },
                                    { "pageSize", limit }
                                } }
                            },
                            r = "mtop.alimusic.music.songservice.getartistsongs"
                        }),
                        encode = Xiami_sign,
                        format = "data.data.songs"
                    };
                    break;
                case ServerProvider.Baidu:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://musicapi.taihe.com/v1/restserver/ting",
                        body = Common.Dynamic2JObject(new
                        {
                            from = "qianqianmini",
                            method = "baidu.ting.artist.getSongList",
                            artistid = id,
                            limits = limit,
                            platform = "darwin",
                            offset = 0,
                            tinguid = 0,
                            version = "11.2.1"
                        }),
                        format = "songlist"
                    };
                    break;
            }

            return this.Exec(api);
        }

        /// <summary>
        /// 根据作家ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="limit"></param>
        /// <returns>返回实体对象</returns>
        public Music_search_item[] ArtistObj(string id, int limit = 50)
        {
            bool tempFormat = this.Format;
            this.Format = true;
            string jsonStr = Artist(id, limit);
            Music_search_item[] rtn = MetingTool.MusicJson2Obj(jsonStr);
            this.Format = tempFormat;

            return rtn;
        }
        #endregion

        #region 根据歌单ID获取
        /// <summary>
        /// 根据歌单ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回json字符串</returns>
        public string Playlist(string id)
        {
            Music_api api = null;
            switch (this.Server)
            {
                case ServerProvider.Netease:
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
                case ServerProvider.Tencent:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "https://c.y.qq.com/v8/fcg-bin/fcg_v8_playlist_cp.fcg",
                        body = Common.Dynamic2JObject(new
                        {
                            id = id,
                            format = "json",
                            newsong = 1,
                            platform = "jqspaframe.json"
                        }),
                        format = "data.cdlist.0.songlist"
                    };
                    break;
                case ServerProvider.Kugou:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://mobilecdn.kugou.com/api/v3/special/song",
                        body = Common.Dynamic2JObject(new
                        {
                            specialid = id,
                            area_code = 1,
                            page = 1,
                            plat = 2,
                            pagesize = -1,
                            version = 8990
                        }),
                        format = "data.info"
                    };
                    break;
                case ServerProvider.Xiami:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://h5api.m.xiami.com/h5/mtop.alimusic.music.list.collectservice.getcollectdetail/1.0/",
                        body = Common.Dynamic2JObject(new
                        {
                            data = new JObject
                            {
                                { "listId", id },
                                { "isFullTags", false },
                                { "pagingVO", new JObject
                                {
                                    { "page", 1 },
                                    { "pageSize", 1000 }
                                } }
                            },
                            r = "mtop.alimusic.music.list.collectservice.getcollectdetail"
                        }),
                        encode = Xiami_sign,
                        format = "data.data.collectDetail.songs"
                    };
                    break;
                case ServerProvider.Baidu:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://musicapi.taihe.com/v1/restserver/ting",
                        body = Common.Dynamic2JObject(new
                        {
                            from = "qianqianmini",
                            method = "baidu.ting.diy.gedanInfo",
                            listid = id,
                            platform = "darwin",
                            version = "11.2.1"
                        }),
                        format = "content"
                    };
                    break;
            }

            return this.Exec(api);
        }

        /// <summary>
        /// 根据歌单ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回实体对象</returns>
        public Music_search_item[] PlaylistObj(string id)
        {
            bool tempFormat = this.Format;
            this.Format = true;
            string jsonStr = Playlist(id);
            Music_search_item[] rtn = MetingTool.MusicJson2Obj(jsonStr);
            this.Format = tempFormat;

            return rtn;
        }
        #endregion

        #region 根据音乐ID获取音乐链接
        /// <summary>
        /// 根据音乐ID获取音乐链接
        /// </summary>
        /// <param name="id"></param>
        /// <param name="br"></param>
        /// <returns>返回json字符串</returns>
        public string Url(string id, int br = 320)
        {
            Music_api api = null;
            switch (this.Server)
            {
                case ServerProvider.Netease:
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
                case ServerProvider.Tencent:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "https://c.y.qq.com/v8/fcg-bin/fcg_play_single_song.fcg",
                        body = Common.Dynamic2JObject(new
                        {
                            songmid = id,
                            platform = "yqq",
                            format = "json"
                        }),
                        decode = Tencent_url
                    };
                    break;
                case ServerProvider.Kugou:
                    api = new Music_api
                    {
                        method = "POST",
                        url = "http://media.store.kugou.com/v1/get_res_privilege",
                        sendDataType = SendDataType.Json,
                        body = Common.Dynamic2JObject(new
                        {
                            relate = 1,
                            userid = "0",
                            vip = 0,
                            appid = 1000,
                            token = "",
                            behavior = "download",
                            area_code = "1",
                            clientver = "8990",
                            resource = new JArray
                            {
                                Common.Dynamic2JObject(new
                                {
                                    id = 0,
                                    type = "audio",
                                    hash = id
                                })
                            },
                        }),
                        decode = Kugou_url
                    };
                    break;
                case ServerProvider.Xiami:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://h5api.m.xiami.com/h5/mtop.alimusic.music.songservice.getsongs/1.0/",
                        body = Common.Dynamic2JObject(new
                        {
                            data = new JObject
                            {
                                { "songIds", new JArray
                                {
                                    id
                                } }
                            },
                            r = "mtop.alimusic.music.songservice.getsongs"
                        }),
                        encode = Xiami_sign,
                        decode = Xiami_url
                    };
                    break;
                case ServerProvider.Baidu:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://musicapi.taihe.com/v1/restserver/ting",
                        body = Common.Dynamic2JObject(new
                        {
                            from = "qianqianmini",
                            method = "baidu.ting.song.getInfos",
                            songid = id,
                            res = 1,
                            platform = "darwin",
                            version = "1.0.0"
                        }),
                        encode = Baidu_AESCBC,
                        decode = Baidu_url
                    };
                    break;
            }
            this.Br = br;

            return this.Exec(api);
        }

        /// <summary>
        /// 根据音乐ID获取音乐链接
        /// </summary>
        /// <param name="id"></param>
        /// <param name="br"></param>
        /// <returns>返回实体对象</returns>
        public Music_url UrlObj(string id, int br = 320)
        {
            bool tempFormat = this.Format;
            this.Format = true;
            string jsonStr = Url(id, br);
            Music_url rtn = MetingTool.UrlJson2Obj(jsonStr);
            this.Format = tempFormat;

            return rtn;
        }
        #endregion

        #region 根据歌曲ID查歌词
        /// <summary>
        /// 根据歌曲ID查歌词
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回json字符串</returns>
        public string Lyric(string id)
        {
            Music_api api = null;
            switch (this.Server)
            {
                case ServerProvider.Netease:
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
                case ServerProvider.Tencent:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "https://c.y.qq.com/lyric/fcgi-bin/fcg_query_lyric_new.fcg",
                        body = Common.Dynamic2JObject(new
                        {
                            songmid = id,
                            g_tk = "5381"
                        }),
                        decode = Tencent_lyric
                    };
                    break;
                case ServerProvider.Kugou:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://krcs.kugou.com/search",
                        body = Common.Dynamic2JObject(new
                        {
                            keyword = "%20-%20",
                            ver = 1,
                            hash = id,
                            client = "mobi",
                            man = "yes"
                        }),
                        decode = Kugou_lyric
                    };
                    break;
                case ServerProvider.Xiami:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://h5api.m.xiami.com/h5/mtop.alimusic.music.lyricservice.getsonglyrics/1.0/",
                        body = Common.Dynamic2JObject(new
                        {
                            data = new JObject
                            {
                                { "songId", id }
                            },
                            r = "mtop.alimusic.music.lyricservice.getsonglyrics"
                        }),
                        encode = Xiami_sign,
                        decode = Xiami_lyric
                    };
                    break;
                case ServerProvider.Baidu:
                    api = new Music_api
                    {
                        method = "GET",
                        url = "http://musicapi.taihe.com/v1/restserver/ting",
                        body = Common.Dynamic2JObject(new
                        {
                            from = "qianqianmini",
                            method = "baidu.ting.song.lry",
                            songid = id,
                            platform = "darwin",
                            version = "1.0.0"
                        }),
                        decode = Baidu_lyric
                    };
                    break;
            }

            return this.Exec(api);
        }

        /// <summary>
        /// 根据歌曲ID查歌词
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回实体对象</returns>
        public Music_lyric LyricObj(string id)
        {
            bool tempFormat = this.Format;
            this.Format = true;
            string jsonStr = Lyric(id);
            Music_lyric rtn = MetingTool.LyricJson2Obj(jsonStr);
            this.Format = tempFormat;

            return rtn;
        }
        #endregion

        #region 歌曲图片(对指定歌曲编号，返回图片地址)
        /// <summary>
        /// 歌曲图片(对指定歌曲编号，返回图片地址)
        /// </summary>
        /// <param name="id">eg.传递通过 api.Song("35847388") 获取到的 pic_id</param>
        /// <param name="size"></param>
        /// <returns>返回json字符串</returns>
        public string Pic(string id, int size = 300)
        {
            string picUrl = string.Empty;
            bool tempFormat;
            switch (this.Server)
            {
                case ServerProvider.Netease:
                    picUrl = "https://p3.music.126.net/" + this.Netease_encryptId(id) + "/" + id + ".jpg?param=" + size + "y" + size;
                    break;
                case ServerProvider.Tencent:
                    picUrl = "https://y.gtimg.cn/music/photo_new/T002R" + size + "x" + size + "M000" + id + ".jpg?max_age=2592000";
                    break;
                case ServerProvider.Kugou:
                    tempFormat = this.Format;
                    string kugouRawJsonStr = this.FormatMethod(false).Song(id);
                    this.Format = tempFormat;
                    dynamic jsonObj = Common.JsonStr2Obj(kugouRawJsonStr);
                    // 发现酷狗的图片大小有限，对于 e64025c53de70ba1d91aec1f8c38f1ae，尝试 100,200,400可行，其它均 404没有，不知道其它歌曲图片情况如何，这里于是暂时写死
                    picUrl = jsonObj.imgUrl.ToString().Replace("{size}", "400");
                    break;
                case ServerProvider.Xiami:
                    tempFormat = this.Format;
                    string xiamiRawJsonStr = this.FormatMethod(false).Song(id);
                    this.Format = tempFormat;
                    dynamic xiamiSongObj = Common.JsonStr2Obj(xiamiRawJsonStr);
                    picUrl = xiamiSongObj.data.data.songDetail.albumLogo.ToString();
                    picUrl = picUrl.Replace("http:", "https:") + "@1e_1c_100Q_" + size + "h_" + size + "w";
                    break;
                case ServerProvider.Baidu:
                    tempFormat = this.Format;
                    string baiduRawJsonStr = this.FormatMethod(false).Song(id);
                    this.Format = tempFormat;
                    dynamic baiduRawJsonObj = Common.JsonStr2Obj(baiduRawJsonStr);
                    picUrl = Common.IsPropertyExist(baiduRawJsonObj, "songinfo") && Common.IsPropertyExist(baiduRawJsonObj.songinfo, "pic_radio") ? baiduRawJsonObj.songinfo.pic_radio : baiduRawJsonObj.songinfo.pic_small;
                    break;
            }

            string jsonStr = Common.Obj2JsonStr(new
            {
                url = picUrl
            });

            return jsonStr;
        }

        /// <summary>
        /// 歌曲图片(对指定歌曲编号，返回图片地址)
        /// </summary>
        /// <param name="id">eg.传递通过 api.Song("35847388") 获取到的 pic_id</param>
        /// <param name="size"></param>
        /// <returns>返回实体对象</returns>
        public Music_pic PicObj(string id, int size = 300)
        {
            bool tempFormat = this.Format;
            this.Format = true;
            string jsonStr = Pic(id, size);
            Music_pic rtn = MetingTool.PicJson2Obj(jsonStr);
            this.Format = tempFormat;

            return rtn;
        }
        #endregion

        #region 网易云音乐API加密
        /// <summary>
        /// 网易云音乐API加密
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        protected Music_api Netease_AESCBC(Music_api api)
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
        /// <summary>
        /// 网易云音乐歌曲ID加密
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Netease_encryptId(string id)
        {
            char[] magic = "3go8&$8*3*3h0k(2)2".ToCharArray();
            char[] song_id = id.ToCharArray();
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

        #region 虾米音乐API加密
        protected Music_api Xiami_sign(Music_api api)
        {
            string url = "http://h5api.m.xiami.com/h5/mtop.alimusic.search.searchservice.searchsongs/1.0/?appKey=12574478&t=1511168684000&dataType=json&data=%7B%22requestStr%22%3A%22%7B%5C%22model%5C%22%3A%7B%5C%22key%5C%22%3A%5C%22Dangerous+Woman%5C%22%2C%5C%22pagingVO%5C%22%3A%7B%5C%22page%5C%22%3A1%2C%5C%22pageSize%5C%22%3A30%7D%7D%7D%22%7D&api=mtop.alimusic.search.searchservice.searchsongs&v=1.0&type=originaljson&sign=f6c99a429e9ef703ea955f7cd113a467";
            string resData = this.Curl(url, null, true).Raw;
            MatchCollection matchColl = Regex.Matches(resData, "_m_h5[^;]+");
            this.Header["Cookie"] = matchColl[0].Value + "; " + matchColl[1].Value;

            string jsonData = Common.Obj2JsonStr(new JObject
            {
                new JProperty("requestStr", Common.Obj2JsonStr(new JObject
                {
                    new JProperty("header", new JObject
                    {
                        new JProperty("platformId", "mac")
                    }),
                    new JProperty("model", api.body["data"])
                }))
                // 注意: 此句必须有，去除 unicode 后，再生成json后导致多出的 \\
                // 并且必须在此处就进行去除，后面sign的生成与data的生成必须是对同一数据
            }).Replace(@"\\u", "u");
            string appkey = "12574478";
            string cookie = this.Header["Cookie"];
            string token = Regex.Match(cookie, "_m_h5_tk=([^_]+)").Groups[1].Value;
            long t = Convert.ToInt64(Common.GetTimeStamp()) * 1000;
            string sign = Common.MD5Encrypt32($"{token}&{t}&{appkey}&{jsonData}");
            api.body = Common.Dynamic2JObject(new
            {
                appKey = appkey,
                t = t,
                dataType = "json",
                data = jsonData,
                api = api.body["r"].ToString(),
                v = "1.0",
                type = "originaljson",
                sign = sign
            });

            return api;
        }
        #endregion

        #region 百度音乐API加密
        protected Music_api Baidu_AESCBC(Music_api api)
        {
            string key = "DBEECF8C50FD160E";
            string vi = "1231021386755796";

            string data = "songid=" + api.body["songid"].ToString() + "&ts=" + Common.GetTimeStampMicro();

            data = Encrypt.AesEncrypt(data, key, vi);
            api.body.Last.AddAfterSelf(new JProperty("e", data));

            return api;
        }
        #endregion

        #region 格式化选择
        /// <summary>
        /// 格式化选择
        /// </summary>
        /// <param name="rawArray"></param>
        /// <returns></returns>
        protected Music_search_item[] Format_select(JArray rawArray)
        {
            Del_music_item_format del_Music_Item = null;
            switch (this.Server)
            {
                case ServerProvider.Netease:
                    del_Music_Item = Format_netease;
                    break;
                case ServerProvider.Tencent:
                    del_Music_Item = Format_tencent;
                    break;
                case ServerProvider.Kugou:
                    del_Music_Item = Format_kugou;
                    break;
                case ServerProvider.Xiami:
                    del_Music_Item = Format_xiami;
                    break;
                case ServerProvider.Baidu:
                    del_Music_Item = Format_baidu;
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

        #region 对(单首)网易云音乐数据进行格式化
        /// <summary>
        /// 对(单首)网易云音乐数据进行格式化
        /// </summary>
        /// <param name="songItem">(单首)网易云音乐json数据</param>
        /// <returns></returns>
        protected Music_search_item Format_netease(dynamic songItem)
        {
            Music_search_item result = new Music_search_item
            {
                id = songItem.id.ToString(),
                name = songItem.name.ToString(),
                artist = new string[0],
                album = songItem.al.name.ToString(),
                pic_id = Common.IsPropertyExist(songItem.al, "pic_str") ? songItem.al.pic_str.ToString() : songItem.al.pic.ToString(),
                url_id = songItem.id.ToString(),
                lyric_id = songItem.id.ToString(),
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

        #region 对(单首)腾讯音乐数据进行格式化
        protected Music_search_item Format_tencent(dynamic songItem)
        {
            if (Common.IsPropertyExist(songItem, "musicData"))
            {
                songItem = songItem.musicData;
            }

            Music_search_item result = new Music_search_item
            {
                id = songItem.mid.ToString(),
                name = songItem.name.ToString(),
                artist = new string[0],
                album = songItem.album.title.ToString().Trim(),
                pic_id = songItem.album.mid.ToString(),
                url_id = songItem.mid.ToString(),
                lyric_id = songItem.mid.ToString(),
                source = "tencent"
            };
            List<string> artistList = new List<string>();
            foreach (dynamic vo in songItem.singer)
            {
                artistList.Add(vo.name.ToString());
            }
            result.artist = artistList.ToArray();

            return result;
        }
        #endregion

        #region 对(单首)酷狗音乐数据进行格式化
        /// <summary>
        /// 对搜索到的(单首)酷狗音乐数据进行格式化
        /// </summary>
        /// <param name="songItem"></param>
        /// <returns></returns>
        protected Music_search_item Format_kugou(dynamic songItem)
        {
            Music_search_item result = new Music_search_item
            {
                id = songItem.hash.ToString(),
                // 千月兔、hanser - 千里邀月 (人声版)
                name = Common.IsPropertyExist(songItem, "filename") ? songItem.filename.ToString() : songItem.fileName.ToString(),
                // Note: 不能写完 artist = null,
                // 不然当后面未给与 artist 值时，json化后会 变为 artist: null , 而不是 artist: []
                artist = new string[0],
                album = Common.IsPropertyExist(songItem, "album_name") ? songItem.album_name.ToString() : "",
                url_id = songItem.hash.ToString(),
                pic_id = songItem.hash.ToString(),
                lyric_id = songItem.hash.ToString(),
                source = "kugou"
            };
            // [0] 千月兔、hanser    [1] 千里邀月 (人声版)
            string[] artistsAndName = result.name.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

            result.artist = artistsAndName[0].Split(new string[] { "、" }, StringSplitOptions.RemoveEmptyEntries);
            result.name = artistsAndName[1];

            return result;
        }
        #endregion

        #region 对(单首)虾米音乐数据进行格式化
        protected Music_search_item Format_xiami(dynamic songItem)
        {
            Music_search_item result = new Music_search_item
            {
                id = songItem.songId,
                name = songItem.songName,
                artist = new string[0],
                album = songItem.albumName,
                pic_id = songItem.songId,
                url_id = songItem.songId,
                lyric_id = songItem.songId,
                source = "xiami"
            };
            List<string> list = new List<string>();
            foreach (dynamic vo in songItem.singerVOs)
            {
                list.Add(vo.artistName.ToString());
            }
            result.artist = list.ToArray();

            return result;
        }
        #endregion

        #region 对(单首)百度音乐数据进行格式化
        protected Music_search_item Format_baidu(dynamic songItem)
        {
            Music_search_item result = new Music_search_item
            {
                id = songItem.song_id,
                name = songItem.title,
                artist = songItem.author.ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                album = songItem.album_title,
                pic_id = songItem.song_id,
                url_id = songItem.song_id,
                lyric_id = songItem.song_id,
                source = "baidu"
            };

            return result;
        }
        #endregion

        #region 设置请求头
        /// <summary>
        /// 设置请求头
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, string> CurlSet()
        {
            Dictionary<string, string> header = null;
            switch (this.Server)
            {
                case ServerProvider.Netease:
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
                case ServerProvider.Tencent:
                    header = new Dictionary<string, string>
                    {
                        { "Referer", "http://y.qq.com" },
                        { "Cookie", "pgv_pvi=22038528; pgv_si=s3156287488; pgv_pvid=5535248600; yplayer_open=1; ts_last=y.qq.com/portal/player.html; ts_uid=4847550686; yq_index=0; qqmusic_fromtag=66; player_exist=1" },
                        { "User-Agent", "QQ%E9%9F%B3%E4%B9%90/54409 CFNetwork/901.1 Darwin/17.6.0 (x86_64)" },
                        { "Accept", "*/*" },
                        { "Accept-Language", "zh-CN,zh;q=0.8,gl;q=0.6,zh-TW;q=0.4" },
                        { "Connection", "keep-alive" },
                        { "Content-Type", "application/x-www-form-urlencoded" }
                    };
                    break;
                case ServerProvider.Kugou:
                    header = new Dictionary<string, string>
                    {
                        { "User-Agent", "IPhone-8990-searchSong" },
                        { "UNI-UserAgent", "iOS11.4-Phone8990-1009-0-WiFi" }
                    };
                    break;
                case ServerProvider.Xiami:
                    header = new Dictionary<string, string>
                    {
                      { "Cookie", "_m_h5_tk=15d3402511a022796d88b249f83fb968_1511163656929; _m_h5_tk_enc=b6b3e64d81dae577fc314b5c5692df3c" },
                        { "User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_5) AppleWebKit/537.36 (KHTML, like Gecko) XIAMI-MUSIC/3.1.1 Chrome/56.0.2924.87 Electron/1.6.11 Safari/537.36" },
                        { "Accept", "application/json" },
                        { "Content-type", "application/x-www-form-urlencoded" },
                        { "Accept-Language", "zh-CN" }
                    };
                    break;
                case ServerProvider.Baidu:
                    string randomStr = Common.GetRandomString(16);
                    string randomHex = Common.StringToHexString(randomStr, Encoding.UTF8);
                    header = new Dictionary<string, string>
                    {
                        { "Cookie", "BAIDUID=" + randomHex + ":FG=1"  },
                        { "User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_6) AppleWebKit/537.36 (KHTML, like Gecko) baidu-music/1.2.1 Chrome/66.0.3359.181 Electron/3.0.5 Safari/537.36" },
                        { "Accept", "*/*" },
                        { "Content-type", "application/json;charset=UTF-8" },
                        { "Accept-Language", "zh-CN" }
                    };
                    break;
            }

            return header;
        }
        #endregion

        #region 提取(解析)网易云音乐链接
        /// <summary>
        /// 提取(解析)网易云音乐链接
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected Music_url Netease_url(dynamic result)
        {
            string jsonStr = result.ToString();
            //Models.Netease.Netease_url data = JsonConvert.DeserializeObject<Models.Netease.Netease_url>(jsonStr);
            dynamic data = Common.JsonStr2Obj(jsonStr);
            Music_url rtn = null;
            if (!string.IsNullOrEmpty(data.data[0].url.ToString()))
            {
                rtn = new Music_url
                {
                    url = data.data[0].url,
                    size = data.data[0].size,
                    br = data.data[0].br / 1000
                };
            }
            else
            {
                rtn = new Music_url
                {
                    url = "",
                    size = 0,
                    br = -1
                };
            }

            return rtn;
        }
        #endregion

        #region 提取(解析)腾讯音乐链接
        protected Music_url Tencent_url(dynamic result)
        {
            string jsonStr = result.ToString();
            dynamic dataInit = Common.JsonStr2Obj(jsonStr);
            int guid = new Random().Next(100000000, 999999999);

            JArray type = new JArray
            {
                new JArray("size_320mp3", 320, "M800", "mp3"),
                new JArray("size_192aac", 192, "C600", "m4a"),
                new JArray("size_128mp3", 128, "M500", "mp3"),
                new JArray("size_96aac", 96, "C400", "m4a"),
                new JArray("size_48aac", 48, "C200", "m4a"),
                new JArray("size_24aac", 24, "C100", "m4a")
            };

            JObject payload = new JObject
            {
               new  JProperty("req_0", new JObject
               {
                   new JProperty("module", "vkey.GetVkeyServer"),
                   new JProperty("method", "CgiGetVkey"),
                   new JProperty("param", new JObject
                   {
                       new JProperty("guid", guid.ToString()),
                       new JProperty("songmid", new JArray()),
                       new JProperty("filename", new JArray()),
                       new JProperty("songtype", new JArray()),
                       new JProperty("uin", "0"),
                       new JProperty("loginflag", 1),
                       new JProperty("platform", "20")
                   })
               })
            };

            JArray arr1 = new JArray();
            JArray arr2 = new JArray();
            JArray arr3 = new JArray();
            foreach (dynamic vo in type)
            {
                dynamic temp1 = dataInit["data"][0]["mid"];
                arr1.Add(temp1);
                dynamic temp2 = vo[2].ToString() + dataInit["data"][0]["file"]["media_mid"] + "." + vo[3].ToString();
                arr2.Add(temp2);
                dynamic temp3 = dataInit["data"][0]["type"];
                arr3.Add(temp3);
            }
            payload["req_0"]["param"]["songmid"] = arr1;
            payload["req_0"]["param"]["filename"] = arr2;
            payload["req_0"]["param"]["songtype"] = arr3;


            Music_api api = new Music_api
            {
                method = "GET",
                url = "https://u.y.qq.com/cgi-bin/musicu.fcg",
                body = Common.Dynamic2JObject(new
                {
                    format = "json",
                    platform = "yqq.json",
                    needNewCode = 0,
                    data = Common.Obj2JsonStr(payload)
                })
            };
            dynamic response = Common.JsonStr2Obj(this.Exec(api));
            dynamic vkeys = response["req_0"]["data"]["midurlinfo"];


            Music_url url = null;
            int index = 0;
            foreach (JToken vo in type.Children())
            {
                if (dataInit["data"][0]["file"][vo[0].ToString()] != null && Convert.ToInt32(vo[1].ToString()) <= this.Br)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(vkeys[index]["vkey"].ToString()))
                        {
                            url = new Music_url
                            {
                                url = response["req_0"]["data"]["sip"][0].ToString() + vkeys[index]["purl"].ToString(),
                                size = dataInit["data"][0]["file"][vo[0].ToString()],
                                br = Convert.ToInt32(vo[1].ToString())
                            };
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                index++;
            }
            if (url == null || url.url == null)
            {
                url = new Music_url
                {
                    url = "",
                    size = 0,
                    br = -1
                };
            }

            return url;
        }
        #endregion

        #region 提取(解析)酷狗音乐链接
        protected Music_url Kugou_url(dynamic result)
        {
            string jsonStr = result.ToString();
            dynamic data = Common.JsonStr2Obj(jsonStr);

            int max = 0;
            Music_url rtn = null;
            foreach (JObject vo in data.data[0].relate_goods)
            {
                int br = Convert.ToInt32(vo["info"]["bitrate"].ToString());
                if (br == this.Br && br > max)
                {
                    Music_api api = new Music_api
                    {
                        method = "GET",
                        url = "http://trackercdn.kugou.com/i/v2/",
                        body = Common.Dynamic2JObject(new
                        {
                            hash = vo["hash"].ToString(),
                            key = Common.MD5Encrypt32(vo["hash"].ToString() + "kgcloudv2"),
                            pid = 3,
                            behavior = "play",
                            cmd = "25",
                            version = 8990
                        }),
                    };
                    dynamic t = Common.JsonStr2Obj(this.Exec(api));
                    if (Common.IsPropertyExist(t, "url"))
                    {
                        max = Convert.ToInt32(t.bitRate.ToString()) / 1000;
                        rtn = new Music_url
                        {
                            url = t.url[0].ToString(),
                            size = t.fileSize,
                            br = Convert.ToInt32(t.bitRate.ToString()) / 1000
                        };
                    }
                } // if br
            } // end foreach
            if (Common.IsPropertyExist(data, "url"))
            {
                rtn = new Music_url
                {
                    url = "",
                    size = 0,
                    br = -1
                };
            }

            return rtn;
        }
        #endregion

        #region 提取(解析)虾米音乐链接
        protected Music_url Xiami_url(dynamic result)
        {
            string jsonStr = result.ToString();
            dynamic songData = Common.JsonStr2Obj(jsonStr);
            JObject types = new JObject
            {
                { "s", 740 },
                { "h", 320 },
                { "l", 128 },
                { "f", 64 },
                { "e", 32 }
            };
            int max = 0;
            Music_url rtn = null;
            foreach (var vo in songData["data"]["data"]["songs"][0]["listenFiles"])
            {
                // 找歌曲质量 小于指定比特率，但最接近的 指定比特率的
                if (Convert.ToInt32(types[vo["quality"].ToString()].ToString()) <= this.Br && Convert.ToInt32(types[vo["quality"].ToString()].ToString()) > max)
                {
                    max = Convert.ToInt32(types[vo["quality"].ToString()].ToString());
                    rtn = new Music_url
                    {
                        url = vo["listenFile"],
                        size = vo["fileSize"],
                        br = types[vo["quality"].ToString()]
                    };
                }
            } // end foreach
            if (rtn == null || string.IsNullOrEmpty(rtn.url))
            {
                rtn = new Music_url
                {
                    url = "",
                    size = 0,
                    br = -1
                };
            }

            return rtn;
        }
        #endregion

        #region 提取(解析)百度音乐链接
        protected Music_url Baidu_url(dynamic result)
        {
            string jsonStr = result.ToString();
            dynamic data = Common.JsonStr2Obj(jsonStr);

            int max = 0;
            Music_url rtn = null;
            foreach (dynamic vo in data.songurl.url)
            {
                int fileBr = Convert.ToInt32(vo.file_bitrate.ToString());
                if (fileBr <= this.Br && fileBr > max)
                {
                    rtn = new Music_url
                    {
                        url = vo.file_link,
                        br = fileBr,
                        size = -2
                    };
                }
            }
            if (rtn == null)
            {
                // 若找不到满足的,则无
                rtn = new Music_url
                {
                    url = "",
                    br = -1,
                    size = -1
                };
            }

            return rtn;
        }
        #endregion

        #region 提取(解析)网易云音乐歌词
        /// <summary>
        /// 提取(解析)网易云音乐歌词
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected Music_lyric Netease_lyric(dynamic result)
        {
            string jsonStr = result.ToString();
            //Models.Netease.Netease_lyric data = JsonConvert.DeserializeObject<Models.Netease.Netease_lyric>(jsonStr);
            dynamic data = Common.JsonStr2Obj(jsonStr);
            Music_lyric rtn = new Music_lyric
            {
                //lyric = data.lrc != null && !string.IsNullOrEmpty(data.lrc.lyric) ? data.lrc.lyric : "",
                lyric = data.lrc != null && data.lrc.lyric != null ? data.lrc.lyric : "",
                //tlyric = data.tlyric != null && !string.IsNullOrEmpty(data.tlyric.lyric) ? data.tlyric.lyric : ""
                tlyric = data.tlyric != null && data.tlyric.lyric != null ? data.tlyric.lyric : ""
            };

            return rtn;
        }
        #endregion

        #region 提取(解析)腾讯音乐歌词
        protected Music_lyric Tencent_lyric(dynamic result)
        {
            string str = result.ToString();
            string jsonStr = Regex.Match(str, @"MusicJsonCallback\((.*)\)").Groups[1].Value;
            dynamic data = Common.JsonStr2Obj(jsonStr);
            Music_lyric rtn = new Music_lyric
            {
                lyric = data.lyric != null ? Common.DecodeBase64("utf-8", data.lyric.ToString()) : "",
                tlyric = data.trans != null ? Common.DecodeBase64("utf-8", data.trans.ToString()) : ""
            };

            return rtn;
        }
        #endregion

        #region 提取(解析)酷狗音乐歌词
        protected Music_lyric Kugou_lyric(dynamic result)
        {
            string jsonStr = result.ToString();
            dynamic data = Common.JsonStr2Obj(jsonStr);
            Music_api api = new Music_api
            {
                method = "GET",
                url = "http://lyrics.kugou.com/download",
                body = Common.Dynamic2JObject(new
                {
                    charset = "utf8",
                    accesskey = data.candidates[0].accesskey,
                    id = data.candidates[0].id,
                    client = "mobi",
                    fmt = "lrc",
                    ver = 1
                })
            };
            data = Common.JsonStr2Obj(this.Exec(api));
            Music_lyric rtn = new Music_lyric
            {
                lyric = Common.DecodeBase64("utf-8", data.content.ToString()),
                tlyric = ""
            };

            return rtn;
        }
        #endregion

        #region 提取(解析)虾米音乐歌词
        protected Music_lyric Xiami_lyric(dynamic result)
        {
            string jsonStr = result.ToString();
            dynamic songData = Common.JsonStr2Obj(jsonStr);
            Music_lyric rtn = null;
            string contentData = string.Empty;
            if (songData["data"]["data"]["lyrics"] != null && songData["data"]["data"]["lyrics"].Count > 0)
            {
                // 有歌词
                contentData = songData["data"]["data"]["lyrics"][0]["content"].ToString();
                contentData = Regex.Replace(contentData, "<[^>]+>", "");

                MatchCollection matches = Regex.Matches(contentData, @"\[([\d:\.]+)\](.*)\s\[x-trans\](.*)", RegexOptions.IgnoreCase);
                // 目前仅适用于歌词与翻译歌词一样多
                if (matches.Count > 0 && matches[0].Groups.Count == 4)
                {
                    // 有翻译歌词
                    string[] lyricArr = new string[matches.Count];
                    string[] tlyricArr = new string[matches.Count];
                    for (int i = 0; i < matches.Count; i++)
                    {
                        // 存在BUG：可能某一句存在无翻译，从而导致.Groups[2\3] 超出索引外
                        lyricArr[i] = "[" + matches[i].Groups[1].Value + "]" + matches[i].Groups[2].Value;
                        tlyricArr[i] = "[" + matches[i].Groups[1].Value + "]" + matches[i].Groups[3].Value;
                    }
                    string lyricStr = contentData;
                    string tlyricStr = contentData;
                    // 循环将原歌词中的每句替换为歌词和翻译歌词
                    for (int i = 0; i < lyricArr.Length; i++)
                    {
                        lyricStr = lyricStr.Replace(matches[i].Groups[0].Value, lyricArr[i]);
                        tlyricStr = tlyricStr.Replace(matches[i].Groups[0].Value, tlyricArr[i]);
                    }

                    rtn = new Music_lyric
                    {
                        lyric = lyricStr,
                        tlyric = tlyricStr
                    };
                }
                else
                {
                    // 无翻译歌词
                    rtn = new Music_lyric
                    {
                        lyric = contentData,
                        tlyric = ""
                    };
                }
            }
            else
            {
                // 无歌词
                rtn = new Music_lyric
                {
                    lyric = "",
                    tlyric = ""
                };
            }

            return rtn;
        }
        #endregion

        #region 提取(解析)百度音乐歌词
        protected Music_lyric Baidu_lyric(dynamic result)
        {
            string jsonStr = result.ToString();
            dynamic data = Common.JsonStr2Obj(jsonStr);

            Music_lyric rtn = new Music_lyric
            {
                lyric = Common.IsPropertyExist(data, "lrcContent") ? data.lrcContent.ToString() : "",
                tlyric = ""
            };

            return rtn;
        }
        #endregion



        #region 即将废弃

        #region 初始化
        /// <summary>
        /// 初始化音乐API 服务提供者
        /// </summary>
        /// <param name="value"></param>
        [Obsolete("该方法即将废弃，请使用 Meting(ServerProvider value = ServerProvider.Netease) 代替")]
        public Meting(string value)
        {
            this.Site(value);
        }
        #endregion

        #region 设置音乐API 服务提供者 (初始化Server, Header)
        /// <summary>
        /// 设置音乐API 服务提供者 (初始化Server, Header)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Obsolete("该方法即将废弃，请使用 Site(ServerProvider value) 代替")]
        public Meting Site(string value)
        {
            ServerProvider provider = ServerProvider.Netease;
            switch (value)
            {
                case "netease":
                    provider = ServerProvider.Netease;
                    break;
                case "tencent":
                    provider = ServerProvider.Tencent;
                    break;
            }
            this.Server = provider;
            this.Header = this.CurlSet();

            return this;
        }
        #endregion

        /// <summary>
        /// 根据歌曲ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Obsolete("该方法即将废弃，请使用 Song(string id) 代替")]
        public string Song(long id)
        {
            return Song(id.ToString());
        }
        /// <summary>
        /// 根据专辑ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Obsolete("该方法即将废弃，请使用 Album(string id) 代替")]
        public string Album(long id)
        {
            return Album(id.ToString());
        }
        /// <summary>
        /// 根据作家ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [Obsolete("该方法即将废弃，请使用 Artist(string id, int limit = 50) 代替")]
        public string Artist(long id, int limit = 50)
        {
            return Artist(id.ToString(), limit);
        }
        /// <summary>
        /// 根据歌单ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Obsolete("该方法即将废弃，请使用 Playlist(string id) 代替")]
        public string Playlist(long id)
        {
            return Playlist(id.ToString());
        }
        /// <summary>
        /// 根据音乐ID获取音乐链接
        /// </summary>
        /// <param name="id"></param>
        /// <param name="br"></param>
        /// <returns></returns>
        [Obsolete("该方法即将废弃，请使用 Url(string id, int br = 320) 代替")]
        public string Url(long id, int br = 320)
        {
            return Url(id.ToString(), br);
        }
        /// <summary>
        /// 根据歌曲ID查歌词
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Obsolete("该方法即将废弃，请使用 Lyric(string id) 代替")]
        public string Lyric(long id)
        {
            return Lyric(id.ToString());
        }
        /// <summary>
        /// 歌曲图片(对指定歌曲编号，返回图片地址)
        /// </summary>
        /// <param name="id">eg.传递通过 api.Song("35847388") 获取到的 pic_id</param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Obsolete("该方法即将废弃，请使用 Pic(string id, int size = 300) 代替")]
        public string Pic(long id, int size = 300)
        {
            return Pic(id.ToString(), size);
        }

        #endregion
    }
}