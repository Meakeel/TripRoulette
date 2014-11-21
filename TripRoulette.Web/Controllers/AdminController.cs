using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripRoulette.Domain.Entities;
using TripRoulette.Web.Models;
using TripRoulette.Domain.Concrete;

namespace TripRoulette.Web.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View("Index");
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

        public ActionResult EventEdit()
        {

            Event events = new Event();
            events = Event.GetEvent(1);

            //events = Event.

            //        Product product = repository.Products
            //.FirstOrDefault(p => p.ProductID == productId);
            return View("EventEdit", events);
        }

        [HttpPost]
        public ActionResult EventDelete(int eventid)
        {
            DataAccess dbAccess = new DataAccess();

            dbAccess.deleteEvent(eventid);
            return Redirect("Events");

        }

        [HttpGet]
        public ActionResult EventCreate()
        {

            Event events = new Event();

            //events = Event.

            //        Product product = repository.Products
            //.FirstOrDefault(p => p.ProductID == productId);
            return View("EventCreate", events);
        }

        [HttpPost]
        public ActionResult EventCreate(Event eventrow)
        {
            Event newEvent = new Event();
            DataAccess dbAccess = new DataAccess();
            newEvent.Name = DataClean.CleanString(eventrow.Name);
            newEvent.StartDate = DataClean.CleanDate(eventrow.StartDate);
            newEvent.EndDate = DataClean.CleanDate(eventrow.EndDate);
            newEvent.Description = DataClean.CleanString(eventrow.Description);
            newEvent.Price = DataClean.CleanDecimal(eventrow.Price);
            newEvent.LunchPrice = DataClean.CleanDecimal(eventrow.LunchPrice);
            newEvent.DinnerPrice = DataClean.CleanDecimal(eventrow.DinnerPrice);
            newEvent.Theme = DataClean.CleanString(eventrow.Theme);
            newEvent.Postcode = DataClean.CleanString(eventrow.Postcode);
            newEvent.Hint = DataClean.CleanString(eventrow.Hint);
            newEvent.FullDetails = DataClean.CleanString(eventrow.FullDetails);
            newEvent.Duration = DataClean.CleanInt(eventrow.Duration);
            newEvent.MinPeople = DataClean.CleanInt(eventrow.MinPeople);
            newEvent.MaxPeople = DataClean.CleanInt(eventrow.MaxPeople);

            dbAccess.InsertEvent(newEvent);

            return RedirectToAction("Events");
        }

    }
}