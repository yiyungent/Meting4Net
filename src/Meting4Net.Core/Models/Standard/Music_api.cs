using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Meting4Net.Core.Models.Standard
{
    public class Music_api
    {
        public string method { get; set; }
        public string url { get; set; }
        public JObject body { get; set; }
        public Del_music_api_encode encode { get; set; }
        public Del_music_api_decode decode { get; set; }
        public string format { get; set; }
    }

    public delegate Music_api Del_music_api_encode(Music_api api);

    public delegate Music_decode Del_music_api_decode(dynamic data);
}
