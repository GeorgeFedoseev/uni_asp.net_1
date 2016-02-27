using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace uni_aspnet_1.Controllers
{
    public class SurveyController : Controller
    {
        //
        // GET: /Survey/

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ProcessForm()
        {
            ViewBag.Name = Request.Form["name"]; 
            ViewBag.Surname = Request.Form["surname"];
            ViewBag.Type = Request.Form["answer"];
  
            return View("FormProcessed");
        }

    }
}
