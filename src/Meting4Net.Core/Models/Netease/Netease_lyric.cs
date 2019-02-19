using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meting4Net.Core.Models.Netease
{
    public class Netease_lyric : JsonModel
    {
        public bool sgc { get; set; }
        public bool sfy { get; set; }
        public bool qfy { get; set; }
        public Netease_lyric_transuser transUser { get; set; }
        public Netease_lyric_lyricuser lyricUser { get; set; }
        public Netease_lyric_lrc lrc { get; set; }
        public Netease_lyric_klyric klyric { get; set; }
        public Netease_lyric_tlyric tlyric { get; set; }
        public int code { get; set; }
    }

    public class Netease_lyric_transuser : JsonModel
    {
        public int id { get; set; }
        public int status { get; set; }
        public int demand { get; set; }
        public int userid { get; set; }
        public string nickname { get; set; }
        public long uptime { get; set; }
    }

    public class Netease_lyric_lyricuser : JsonModel
    {
        public int id { get; set; }
        public int status { get; set; }
        public int demand { get; set; }
        public int userid { get; set; }
        public string nickname { get; set; }
        public long uptime { get; set; }
    }

    public class Netease_lyric_lrc : JsonModel
    {
        public int version { get; set; }
        public string lyric { get; set; }
    }

    public class Netease_lyric_klyric : JsonModel
    {
        public int version { get; set; }
        public object lyric { get; set; }
    }

    public class Netease_lyric_tlyric : JsonModel
    {
        public int version { get; set; }
        public string lyric { get; set; }
    }
}
