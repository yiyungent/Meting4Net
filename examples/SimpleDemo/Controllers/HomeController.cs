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
            string jsonStr = api.FormatMethod(true).Url("35847388");

            return Content(jsonStr, "application/json");
        }
    }
}