using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meting4Net.Core.Models.Standard
{
    public class Music_api
    {
        public string method { get; set; }
        public string url { get; set; }
        public dynamic body { get; set; }
        public Del_music_api_encode encode { get; set; }
        public Del_music_api_decode decode { get; set; }
        public string format { get; set; }
    }

    public delegate Music_api Del_music_api_encode(Music_api api);

    public delegate Music_decode Del_music_api_decode(dynamic data);

    public class Music_decode
    {
        /// <summary>
        /// 转换当前对象为 json字符串
        /// 注意：标记为虚方法：调用 Music_decode 对象的此方法，若此对象里装了子类，则调用子类的此方法，此时则为子类对象 的 jsonStr
        /// </summary>
        /// <returns></returns>
        public virtual string ToJsonStr()
        {
            return Common.Obj2JsonStr(this);
        }
    }
}
