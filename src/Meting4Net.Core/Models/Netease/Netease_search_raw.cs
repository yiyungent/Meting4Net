using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meting4Net.Core.Models.Netease
{
    public class Netease_search_raw : JsonModel
    {
        public Result result { get; set; }
        public int code { get; set; }
    }

    public class Result : JsonModel
    {
        public Song[] songs { get; set; }
        public int songCount { get; set; }
    }

    public class Song : JsonModel
    {
        public string name { get; set; }
        public int id { get; set; }
        public int pst { get; set; }
        public int t { get; set; }
        public Ar[] ar { get; set; }
        public object[] alia { get; set; }
        public int pop { get; set; }
        public int st { get; set; }
        public string rt { get; set; }
        public int fee { get; set; }
        public int v { get; set; }
        public object crbt { get; set; }
        public string cf { get; set; }
        public Al al { get; set; }
        public int dt { get; set; }
        public H h { get; set; }
        public M m { get; set; }
        public L l { get; set; }
        public object a { get; set; }
        public string cd { get; set; }
        public int no { get; set; }
        public object rtUrl { get; set; }
        public int ftype { get; set; }
        public object[] rtUrls { get; set; }
        public int djId { get; set; }
        public int copyright { get; set; }
        public int s_id { get; set; }
        public int mv { get; set; }
        public int rtype { get; set; }
        public object rurl { get; set; }
        public int mst { get; set; }
        public int cp { get; set; }
        public long publishTime { get; set; }
        public Privilege privilege { get; set; }
    }

    public class Al : JsonModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string picUrl { get; set; }
        public object[] tns { get; set; }
        public string pic_str { get; set; }
        public long pic { get; set; }
    }

    public class H : JsonModel
    {
        public int br { get; set; }
        public int fid { get; set; }
        public int size { get; set; }
        public int vd { get; set; }
    }

    public class M : JsonModel
    {
        public int br { get; set; }
        public int fid { get; set; }
        public int size { get; set; }
        public int vd { get; set; }
    }

    public class L : JsonModel
    {
        public int br { get; set; }
        public int fid { get; set; }
        public int size { get; set; }
        public int vd { get; set; }
    }

    public class Privilege : JsonModel
    {
        public int id { get; set; }
        public int fee { get; set; }
        public int payed { get; set; }
        public int st { get; set; }
        public int pl { get; set; }
        public int dl { get; set; }
        public int sp { get; set; }
        public int cp { get; set; }
        public int subp { get; set; }
        public bool cs { get; set; }
        public int maxbr { get; set; }
        public int fl { get; set; }
        public bool toast { get; set; }
        public int flag { get; set; }
    }

    public class Ar : JsonModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public object[] tns { get; set; }
        public string[] alias { get; set; }
        public string[] alia { get; set; }
    }
}
