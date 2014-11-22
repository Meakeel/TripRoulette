using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TripRoulette.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SendEmail(string numPeople, string leave, string budget, string leavingfrom)
        {
            /// Code here!!!!.....
            /// 
            HttpContext.Trace.Warn("Sending email");
            try
            {
                //TripRoulette.RandomGenerator.RandomGenerator.GenerateRandomTrip(
                //    Convert.ToInt32(numPeople),
                //    DateTime.Parse(leave),
                //    Convert.ToDecimal(budget),
                //    null,
                //    leavingfrom,
                //    "crowesoft@gmail.com"

                //);
            }
            catch(Exception ex)
            {
                HttpContext.Trace.Warn(ex.Message + " ---- " + ex.StackTrace);
            }

            ViewBag.Completed = "Completed";

            return View();
        }


        public ActionResult Results()
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

        public ActionResult Form()
        {
            return View();
        }
    }
}