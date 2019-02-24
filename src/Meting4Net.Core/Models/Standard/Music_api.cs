using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Meting4Net.Core.Models.Standard
{
    public enum SendDataType
    {
        /// <summary>
        /// 键值对参数: key1=val1&key2=val2&key3=val3
        /// </summary>
        KeyValueParm,
        /// <summary>
        /// json数据: 直接发送 json字符串
        /// </summary>
        Json
    }

    public class Music_api
    {
        public string method { get; set; }
        public string url { get; set; }
        public SendDataType sendDataType { get; set; } = SendDataType.KeyValueParm;
        /// <summary>
        /// 目前发送的数据只支持 两种类型
        /// 1.键值对参数: 使用 JObject 来构建 键值对
        /// 2.json数据: 使用 JOjbect 或 JArray 来构建 json
        /// </summary>
        public JToken body { get; set; }
        public Del_music_api_encode encode { get; set; }
        public Del_music_api_decode decode { get; set; }
        public string format { get; set; }
    }

    public class Options
    {
        public int? page { get; set; } = null;
        public int? limit { get; set; } = null;
        public int? type { get; set; } = null;
    }

    public delegate Music_api Del_music_api_encode(Music_api api);

    public delegate Music_model Del_music_api_decode(dynamic data);
}
