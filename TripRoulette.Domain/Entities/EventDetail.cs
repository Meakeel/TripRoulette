using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripRoulette.Domain.Concrete;

namespace TripRoulette.Domain.Entities
{
    public class EventDetail
    {

        private int _dayOfWeek;
        public DateTime _startTime;
        public DateTime _endTime;


        public int DayOfWeek
        {
            get { return _dayOfWeek; }
            set { _dayOfWeek = value; }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

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