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

            //Meting api = new Meting("tencent");
            //string jsonStr = api.FormatMethod(true).Lyric("001Nal2N2f0Qr8");
            // 传递通过 .Song("001Nal2N2f0Qr8") 获取的 pic_id 002szRig2zZxtj
            //string jsonStr = api.Pic("002szRig2zZxtj");

            //Meting api = new Meting(ServerProvider.Kugou);
            //string jsonStr = api.FormatMethod(true).Search("千里邀月");
            //string jsonStr = api.FormatMethod(true).Lyric("e64025c53de70ba1d91aec1f8c38f1ae");
            //string jsonStr = api.FormatMethod(true).Pic("e64025c53de70ba1d91aec1f8c38f1ae");

            Meting api = new Meting(ServerProvider.Xiami);
            //string jsonStr = api.FormatMethod(true).Search("千里邀月");
            // 传递通过Search获得的歌曲ID
            //string jsonStr = api.FormatMethod(true).Song("1808486366");
            // https://www.xiami.com/album/2103947271  其中专辑ID为2103947271
            //string jsonStr = api.FormatMethod(true).Album("2103947271");
            // https://www.xiami.com/artist/2110230610 作家ID 2110230610 
            //string jsonStr = api.FormatMethod(true).Artist("2110230610");
            // https://www.xiami.com/collect/632580584 歌单ID 632580584
            //string jsonStr = api.FormatMethod(true).Playlist("632580584");
            //string jsonStr = api.FormatMethod(true).Url("1808486366");
            // https://www.xiami.com/song/1774129963 歌曲ID 1774129963
            // 有翻译歌词
            //string jsonStr = api.FormatMethod(true).Lyric("1774129963");
            // 无翻译歌词
            //string jsonStr = api.FormatMethod(true).Lyric("1768955559");
            // 无歌词
            string jsonStr = api.FormatMethod(true).Lyric("1772326454");
            Console.WriteLine(jsonStr);

            Console.ReadLine();
        }
    }
}
