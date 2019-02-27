using System;

using Meting4Net.Core;
using Meting4Net.Core.Models.Standard;

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
            //string jsonStr = api.FormatMethod(true).Song("1332662925");

            //Meting api = new Meting("tencent");
            //string jsonStr = api.FormatMethod(true).Lyric("001Nal2N2f0Qr8");
            // 传递通过 .Song("001Nal2N2f0Qr8") 获取的 pic_id 002szRig2zZxtj
            //string jsonStr = api.Pic("002szRig2zZxtj");

            //Meting api = new Meting(ServerProvider.Kugou);
            //string jsonStr = api.FormatMethod(true).Search("千里邀月");
            //string jsonStr = api.FormatMethod(true).Lyric("e64025c53de70ba1d91aec1f8c38f1ae");
            //string jsonStr = api.FormatMethod(true).Pic("e64025c53de70ba1d91aec1f8c38f1ae");

            //Meting api = new Meting(ServerProvider.Xiami);
            //string jsonStr = api.FormatMethod(true).Search("千里邀月");
            // 传递通过Search获得的歌曲ID
            //string jsonStr = api.FormatMethod(true).Song("1808486366");
            // https://www.xiami.com/album/2104586331  其中专辑ID为2104586331
            //string jsonStr = api.FormatMethod(true).Album("2104586331");
            // https://www.xiami.com/artist/593402077?spm=a1z1s.3057853.6862625.41.phWDbn 作家ID 593402077 
            //string jsonStr = api.FormatMethod(true).Artist("593402077");
            // https://www.xiami.com/collect/632580584 歌单ID 632580584
            //string jsonStr = api.FormatMethod(true).Playlist("632580584");
            //string jsonStr = api.FormatMethod(true).Url("1808486366");
            // https://www.xiami.com/song/1774129963 歌曲ID 1774129963
            // 有翻译歌词
            //string jsonStr = api.FormatMethod(true).Lyric("1774129963");
            // 无翻译歌词
            //string jsonStr = api.FormatMethod(true).Lyric("1768955559");
            // 无歌词
            //string jsonStr = api.FormatMethod(true).Lyric("1772326454");
            //string jsonStr = api.Pic("1772326454");
            //string jsonStr = api.ProxyMethod(new MetingProxy("47.97.169.111", 3128)).FormatMethod(true).Lyric("1774129963");


            //Console.WriteLine(jsonStr);

            //Music_search_item[] music_Search_Items = api.SearchObj("千里邀月");
            //Music_search_item music_Search_Item = api.SongObj("1808486366");
            //Music_search_item[] music_Search_Items = api.AlbumObj("2104586331");
            //Music_search_item[] music_Search_Items = api.ArtistObj("593402077");
            //Music_search_item[] music_Search_Items = api.PlaylistObj("632580584");
            //Music_url music_Url = api.UrlObj("1808486366");
            //Music_lyric music_Lyric = api.LyricObj("1774129963");
            //Music_pic music_Pic = api.PicObj("1772326454");

            Meting api = new Meting(ServerProvider.Baidu);
            //Music_search_item[] items = api.SearchObj("洛少爷");
            //Music_search_item song = api.SongObj("73992640");
            // http://music.taihe.com/album/533414191  专辑ID 533414191
            //Music_search_item[] musics = api.AlbumObj("533414191");
            // http://music.taihe.com/artist/1052 作家ID 1052
            //Music_search_item[] musics = api.ArtistObj("1052");
            // http://music.taihe.com/songlist/566104573 歌单ID 566104573
            //Music_search_item[] musics = api.PlaylistObj("566104573");
            // 目前百度音乐获取到的音乐链接当复制到浏览器地址栏时只可用于下载，不可在线播放
            //Music_url url = api.UrlObj("73992640");
            // http://music.taihe.com/song/976984 歌曲ID 976984
            //Music_lyric lyric = api.LyricObj("976984");
            Music_pic pic = api.PicObj("976984");

            Console.ReadLine();
        }
    }
}
