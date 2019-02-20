using System;

using Meting4Net.Core;

namespace NetCoreConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Meting api = new Meting("tencent");
            //string jsonStr = api.FormatMethod(true).Search("千里邀月");

            //Meting api = new Meting("netease");
            //string jsonStr = api.FormatMethod(true).Lyric("35847388");

            Meting api = new Meting("tencent");
            string jsonStr = api.FormatMethod(true).Lyric("001Nal2N2f0Qr8");

            Console.WriteLine(jsonStr);

            Console.ReadLine();
        }
    }
}
