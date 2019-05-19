using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JINRO_WebApp.Models.Entity;

namespace JINRO_WebApp.Controllers
{
    public class JINROController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Jinro(GameInfo gameInfo)
        {
            JINROService _service = new JINROService();
            _service.Night(gameInfo);
            return View();
        }
    }
}