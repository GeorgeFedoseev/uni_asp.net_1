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
            var name = Request.Form["name"];
            var surname = Request.Form["surname"];
            var hobby = parseHobby(Request.Form["answer"]);

            var errors = new List<string>();
            // some validation
            if (name != "" && surname != "")
            {
                var surveyResult = new Models.SurveyResult(name, surname, hobby);

                if (canVote(surveyResult)) {
                    MvcApplication.surveyResults.Add(surveyResult);
                    ViewBag.surveyResult = surveyResult;
                }else{
                    errors.Add("Вы уже голосовали");
                }
            }
            else {
                errors.Add("Имя и Фамилия должны быть заполнены");
            }

            ViewBag.errors = errors;

            return View("FormProcessed");
        }

        private Models.HobbyType parseHobby(string hobby) {
            switch (hobby) { 
                case "student":
                    return Models.HobbyType.Student;
                case "worker":
                    return Models.HobbyType.Worker;
                default:
                    return Models.HobbyType.Undefined;
            }
        }

        private bool canVote(Models.SurveyResult surveyResult) {
            foreach (var sr in MvcApplication.surveyResults) {
                if (sr.name == surveyResult.name && sr.surname == surveyResult.surname) {
                    return false;
                }
            }

            return true;
        }


        public ActionResult Results() {
            return View();
        }

    }
}
