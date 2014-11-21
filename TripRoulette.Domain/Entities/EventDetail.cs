using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TripRoulette.Domain.Concrete;

namespace TripRoulette.Domain.Entities
{
    public class EventDetail
    {

        private int _eventID;
        private int _eventDetailID;
        private int _dayOfWeek;
        public DateTime _startTime;
        public DateTime _endTime;
        public bool _allDay;

        public int EventID
        {
            get { return _eventID; }
            set { _eventID = value; }
        }
        public int EventDetailID
        {
            get { return _eventDetailID; }
            set { _eventDetailID = value; }
        }

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
        public bool allDay
        {
            get { return _allDay; }
            set { _allDay = value; }
        }

    }
}