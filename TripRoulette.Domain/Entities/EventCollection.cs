using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripRoulette.Domain.Concrete;

namespace TripRoulette.Domain.Entities
{
    public class EventCollection : List<Event>
    {
        public void Save()
        {
            DataAccess dbconnection = new DataAccess();
            //Loop through the collection and save each row.
            foreach (Event row in this)
            {
                dbconnection.InsertEvent(row);
            }
        }



    }
}
