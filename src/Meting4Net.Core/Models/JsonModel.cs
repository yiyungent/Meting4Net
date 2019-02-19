using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meting4Net.Core.Models
{
    public class JsonModel
    {
        /// <summary>
        /// 转换当前对象为 json字符串
        /// 注意：标记为虚方法：调用 JsonModel 对象的此方法，若此对象里装了子类，则调用子类的此方法，此时则为子类对象 的 jsonStr
        /// </summary>
        /// <returns></returns>
        public virtual string ToJsonStr()
        {
            return Common.Obj2JsonStr(this);
        }
    }
}
