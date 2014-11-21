using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripRoulette.Domain.Entities;
using TripRoulette.Web.Models;

namespace TripRoulette.Web.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {

            EventCollection events = new EventCollection();
            events = Event.GetEvents();

            //events = Event.

    //        Product product = repository.Products
    //.FirstOrDefault(p => p.ProductID == productId);
            return View(events);
    }

        public ActionResult Events()
        {

            EventCollection events = new EventCollection();
            events = Event.GetEvents();

            //events = Event.

            //        Product product = repository.Products
            //.FirstOrDefault(p => p.ProductID == productId);
            return View("Event",events);
        }
    }
}