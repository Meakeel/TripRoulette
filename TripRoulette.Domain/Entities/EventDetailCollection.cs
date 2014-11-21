using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripRoulette.Domain.Concrete;

namespace TripRoulette.Domain.Entities
{
    public class EventDetailCollection : List<EventDetail>
    {
        public void Save()
        {
            DataAccess dbconnection = new DataAccess();
            //Loop through the collection and save each row.
            foreach (EventDetail row in this)
            {
                dbconnection.InsertEventDetail(row);
            }
        }
    }

}