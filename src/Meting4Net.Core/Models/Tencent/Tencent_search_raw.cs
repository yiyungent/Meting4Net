using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meting4Net.Core.Models.Tencent
{
    public class Tencent_search_raw : JsonModel
    {
        public int code { get; set; }
        public Data data { get; set; }
        public string message { get; set; }
        public string notice { get; set; }
        public int subcode { get; set; }
        public int time { get; set; }
        public string tips { get; set; }
    }

    public class Data : JsonModel
    {
        public string keyword { get; set; }
        public int priority { get; set; }
        public object[] qc { get; set; }
        public Semantic semantic { get; set; }
        public Song song { get; set; }
        public int tab { get; set; }
        public object[] taglist { get; set; }
        public int totaltime { get; set; }
        public Zhida zhida { get; set; }
    }

    public class Semantic : JsonModel
    {
        public int curnum { get; set; }
        public int curpage { get; set; }
        public object[] list { get; set; }
        public int totalnum { get; set; }
    }

    public class Song : JsonModel
    {
        public int curnum { get; set; }
        public int curpage { get; set; }
        public List[] list { get; set; }
        public int totalnum { get; set; }
    }

    public class List : JsonModel
    {
        public Action action { get; set; }
        public Album album { get; set; }
        public int chinesesinger { get; set; }
        public string desc { get; set; }
        public string desc_hilight { get; set; }
        public string docid { get; set; }
        public File file { get; set; }
        public int fnote { get; set; }
        public int genre { get; set; }
        public object[] grp { get; set; }
        public int id { get; set; }
        public int index_album { get; set; }
        public int index_cd { get; set; }
        public int interval { get; set; }
        public int isonly { get; set; }
        public Ksong ksong { get; set; }
        public int language { get; set; }
        public string lyric { get; set; }
        public string lyric_hilight { get; set; }
        public string mid { get; set; }
        public Mv mv { get; set; }
        public string name { get; set; }
        public int newStatus { get; set; }
        public long nt { get; set; }
        public Pay pay { get; set; }
        public int pure { get; set; }
        public Singer[] singer { get; set; }
        public int status { get; set; }
        public string subtitle { get; set; }
        public int t { get; set; }
        public int tag { get; set; }
        public string time_public { get; set; }
        public string title { get; set; }
        public string title_hilight { get; set; }
        public int type { get; set; }
        public string url { get; set; }
        public int ver { get; set; }
        public Volume volume { get; set; }
    }

    public class Action : JsonModel
    {
        public int alert { get; set; }
        public int icons { get; set; }
        public int msg { get; set; }
        public int _switch { get; set; }
    }

    public class Album : JsonModel
    {
        public int id { get; set; }
        public string mid { get; set; }
        public string name { get; set; }
        public string subtitle { get; set; }
        public string title { get; set; }
        public string title_hilight { get; set; }
    }

    public class File : JsonModel
    {
        public string media_mid { get; set; }
        public int size_128 { get; set; }
        public int size_320 { get; set; }
        public int size_aac { get; set; }
        public int size_ape { get; set; }
        public int size_dts { get; set; }
        public int size_flac { get; set; }
        public int size_ogg { get; set; }
        public int size_try { get; set; }
        public string strMediaMid { get; set; }
        public int try_begin { get; set; }
        public int try_end { get; set; }
    }

    public class Ksong : JsonModel
    {
        public int id { get; set; }
        public string mid { get; set; }
    }

    public class Mv : JsonModel
    {
        public int id { get; set; }
        public string vid { get; set; }
    }

    public class Pay : JsonModel
    {
        public int pay_down { get; set; }
        public int pay_month { get; set; }
        public int pay_play { get; set; }
        public int pay_status { get; set; }
        public int price_album { get; set; }
        public int price_track { get; set; }
        public int time_free { get; set; }
    }

    public class Volume : JsonModel
    {
        public float gain { get; set; }
        public float lra { get; set; }
        public float peak { get; set; }
    }

    public class Singer : JsonModel
    {
        public int id { get; set; }
        public string mid { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string title_hilight { get; set; }
        public int type { get; set; }
        public int uin { get; set; }
    }

    public class Zhida : JsonModel
    {
        public int chinesesinger { get; set; }
        public int type { get; set; }
    }
}
