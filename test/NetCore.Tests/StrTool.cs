using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Tests
{
    class StrTool
    {
        #region 指定的字符串是否包括全部的子串数组
        public static bool ContainsStrArr(string searchStr, string[] subStrArr)
        {
            bool isAllContain = true;
            foreach (string subStr in subStrArr)
            {
                isAllContain = searchStr.Contains(subStr);
                // 主要有一个不包含，则退出，标记为不包含false
                if (!isAllContain)
                {
                    break;
                }
            }

            return isAllContain;
        }
        #endregion
    }
}
