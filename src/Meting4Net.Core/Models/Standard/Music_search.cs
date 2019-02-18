﻿using System;
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

    public class Music_search_item
    {
        public int id { get; set; }
        public string name { get; set; }
        public string[] artist { get; set; }
        public string album { get; set; }
        public string pic_id { get; set; }
        public int url_id { get; set; }
        public int lyric_id { get; set; }
        public string source { get; set; }
    }
}