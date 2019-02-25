using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Meting4Net.Core;

namespace SimpleDemo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ContentResult Index()
        {
            #region 网易云音乐 API
            //Meting api = new Meting("netease");
            //string jsonStr = api.FormatMethod(true).Url("35847388");
            //string jsonStr = api.FormatMethod(true).Song("35847388");
            //string jsonStr = api.FormatMethod(true).Album("73927024");
            //string jsonStr = api.Album("73927024");
            //string jsonStr = api.FormatMethod(true).Artist("1049179");
            //string jsonStr = api.FormatMethod(true).Playlist("2487120533");
            //string jsonStr = api.FormatMethod(true).Lyric("35847388");
            //string jsonStr = api.FormatMethod(false).Lyric("35847388");

            // 错误：传递歌曲ID
            //string jsonStr = api.Pic("527190053");
            // 错误：传递专辑ID
            //string jsonStr = api.Pic("73927024");
            // 正确：传递通过 api.Song("35847388") 获取到的 pic_id
            //string jsonStr = api.Pic("1407374890649284");

            // 需要设置 page 时，必须同时设置 limit 才有效
            //string jsonStr = api.FormatMethod(true).Search("Soldier", new Meting4Net.Core.Models.Standard.Options
            //{
            //    page = 1,
            //    limit = 50
            //});
            //string jsonStr = api.FormatMethod(true).Search("Soldier");
            // 只返回 3 条 原始网易音乐格式
            //string jsonStr = api.FormatMethod(false).Search("Soldier", new Meting4Net.Core.Models.Standard.Options
            //{
            //    limit = 3
            //});
            #endregion

            #region 腾讯QQ音乐 API
            //Meting api = new Meting("tencent");
            //string jsonStr = api.FormatMethod(true).Search("千里邀月");
            // 腾讯传递 通过Search() 获取到的 歌曲 "id"
            //string jsonStr = api.FormatMethod(true).Song("001Nal2N2f0Qr8");
            // 例如专辑 https://y.qq.com/n/yqq/album/001AMQBG3GzakR.html#stat=y_new.album.otheralbum.click
            // 则其中 001AMQBG3GzakR 为专辑ID
            //string jsonStr = api.FormatMethod(true).Album("001AMQBG3GzakR");
            // https://y.qq.com/n/yqq/singer/001fNHEf1SFEFN.html#stat=y_new.singerlist.singerpic
            // 其中 001fNHEf1SFEFN 为 歌手ID
            //string jsonStr = api.FormatMethod(true).Artist("001fNHEf1SFEFN");
            // https://y.qq.com/n/yqq/playsquare/1721973967.html#stat=y_new.playlist.pic_click
            // 其中 1721973967 为歌单ID
            //string jsonStr = api.FormatMethod(true).Playlist("1721973967");
            // https://y.qq.com/n/yqq/song/001Nal2N2f0Qr8.html
            //string jsonStr = api.FormatMethod(true).Url("001Nal2N2f0Qr8");
            //
            //string jsonStr = api.FormatMethod(true).Lyric("001Nal2N2f0Qr8");
            #endregion

            #region 酷狗音乐 API
            Meting api = new Meting(ServerProvider.Kugou);
            //string jsonStr = api.FormatMethod(true).Search("千里邀月");
            // 传递通过 Search("千里邀月") 获得的 歌曲id
            //string jsonStr = api.FormatMethod(true).Song("e64025c53de70ba1d91aec1f8c38f1ae");
            // http://www.kugou.com/share/9vXiM9bt7V2.html#hash=8B264F3E5F587DDB2631660B34CD43FB&album_id=1746593
            // 其中 album_id= 后的 1746593 即为专辑ID
            //string jsonStr = api.FormatMethod(true).Album("1746593");
            // https://www.kugou.com/singer/19671.html 其中 19671 即为作家ID
            //string jsonStr = api.FormatMethod(true).Artist("19671");
            // https://www.kugou.com/yy/special/single/602964.html 其中 602964 即为歌单ID
            //string jsonStr = api.FormatMethod(true).Playlist("602964");
            // 传递 歌曲ID
            string jsonStr = api.FormatMethod(true).Url("e64025c53de70ba1d91aec1f8c38f1ae");
            #endregion

            return Content(jsonStr, "application/json");
        }
    }
}