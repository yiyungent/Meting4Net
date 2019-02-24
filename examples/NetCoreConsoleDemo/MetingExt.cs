using System;
using System.Collections.Generic;
using System.Text;

using Meting4Net.Core;
using Meting4Net.Core.Models.Standard;

namespace NetCoreConsoleDemo
{
    /// <summary>
    /// 可以通过继承 Meting 来扩展 Meting
    /// </summary>
    public class MetingExt : Meting
    {
        public MetingExt()
        {
        }

        public new Music_search_item Format_netease(dynamic songItem)
        {
            return new Music_search_item();
        }
    }
}
