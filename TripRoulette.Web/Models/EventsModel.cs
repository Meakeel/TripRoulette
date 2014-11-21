using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripRoulette.Domain.Entities;

namespace TripRoulette.Web.Models
{
    public class EventsModel

    {

        public IEnumerable<Event> Events {  get; set; }
        public EventsPagingInfo PagingInfo { get; set; }



    }
}