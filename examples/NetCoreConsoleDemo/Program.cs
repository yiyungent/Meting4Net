using System;

using Meting4Net.Core;

namespace NetCoreConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Meting api = new Meting("tencent");
            string jsonStr = api.FormatMethod(true).Search("千里邀月");
            Console.WriteLine(jsonStr);

            Console.ReadLine();
        }
    }
}
