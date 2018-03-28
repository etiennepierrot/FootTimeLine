using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FootTimeLine.Front.ModelBinder;
using FootTimeLine.Front.Models;

namespace FootTimeLine.Front.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        

        public ActionResult GetTimeline([FromJson]GameModelPost gameModelPost)
        {
            return View("Index");
        }
    }
}
