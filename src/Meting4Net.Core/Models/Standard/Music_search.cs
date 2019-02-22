using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meting4Net.Core.Models.Standard
{
    /// <summary>
    /// 通过 Meting.FormatMethod(true).Search() 获得的统一格式化后 json数据
    /// </summary>
    public class Music_search
    {
        public Music_search_item[] items { get; set; }
    }

    public class Music_search_item : Music_model
    {
        public string id { get; set; }
        public string name { get; set; }
        public string[] artist { get; set; }
        public string album { get; set; }
        public string pic_id { get; set; }
        public string url_id { get; set; }
        public string lyric_id { get; set; }
        public string source { get; set; }
    }

    public delegate Music_search_item Del_music_item_format(dynamic songItem);
}
