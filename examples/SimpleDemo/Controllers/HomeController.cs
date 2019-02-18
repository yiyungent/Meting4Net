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
            Meting api = new Meting("netease");
            //string jsonStr = api.FormatMethod(true).Url(35847388);
            //string jsonStr = api.FormatMethod(true).Song(35847388);
            //string jsonStr = api.FormatMethod(true).Album(73927024);
            //string jsonStr = api.Album(73927024);
            //string jsonStr = api.FormatMethod(true).Artist(1049179);
            //string jsonStr = api.FormatMethod(true).Playlist(2487120533);
            //string jsonStr = api.FormatMethod(true).Lyric(35847388);
            string jsonStr = api.FormatMethod(false).Lyric(35847388);

            return Content(jsonStr, "application/json");
        }
    }
}