using System;
using System.Collections.Generic;
using System.Text;

using Meting4Net.Core.Models.Standard;
using Newtonsoft.Json;

namespace Meting4Net.Core
{
    public class MetingTool
    {
        public static Music_search_item[] MusicJson2Obj(string jsonStr)
        {
            Music_search_item[] rtn = JsonConvert.DeserializeObject<Music_search_item[]>(jsonStr);

            return rtn;
        }

        public static Music_search_item MusicItemJson2Obj(string jsonStr)
        {
            Music_search_item rtn = JsonConvert.DeserializeObject<Music_search_item>(jsonStr);

            return rtn;
        }

        public static Music_lyric LyricJson2Obj(string jsonStr)
        {
            Music_lyric rtn = JsonConvert.DeserializeObject<Music_lyric>(jsonStr);

            return rtn;
        }

        public static Music_url UrlJson2Obj(string jsonStr)
        {
            Music_url rtn = JsonConvert.DeserializeObject<Music_url>(jsonStr);

            return rtn;
        }

        public static Music_pic PicJson2Obj(string jsonStr)
        {
            Music_pic rtn = JsonConvert.DeserializeObject<Music_pic>(jsonStr);

            return rtn;
        }
    }
}
