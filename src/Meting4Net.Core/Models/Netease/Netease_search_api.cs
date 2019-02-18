using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meting4Net.Core.Models.Netease
{
    public class Netease_search_api
    {
        public string method { get; set; }
        public string url { get; set; }
        public Netease_search_api_body body { get; set; }
        public string encode { get; set; }
        public string format { get; set; }
    }

    public class Netease_search_api_body
    {
        public string s { get; set; }
        public int type { get; set; }
        public int limit { get; set; }
        public string total { get; set; }
        public int offset { get; set; }

        public string _params { get; set; }
        public string encSecKey { get; set; }
    }
}
