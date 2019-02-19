﻿using System;
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
            Meting api = new Meting("tencent");
            string jsonStr = api.FormatMethod(true).Search("千里邀月");
            #endregion

            return Content(jsonStr, "application/json");
        }
    }
}