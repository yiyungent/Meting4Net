using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;

namespace NetCore.Tests
{
    class TestTool
    {
        public static int TryCount { get; set; } = 5;

        public static int SleepMsec { get; set; } = 20000;

        public static bool TargetStrContain(Func<string> getTargetStr, string[] subStrArr)
        {
            bool isAllContain = false;
            for (int i = 0; i < TryCount; i++)
            {
                if (StrTool.ContainsStrArr(getTargetStr(), subStrArr))
                {
                    isAllContain = true;
                    break;
                }
                Thread.Sleep(SleepMsec * (i + 1));
            }

            return isAllContain;
        }
    }
}
