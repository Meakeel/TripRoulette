using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripRoulette.Domain.Concrete;

namespace TripRoulette.Domain.Entities
{
    public class Event
    {
        private int _eventID;
        private string _name;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _description;
        private decimal _price;
        private decimal _lunchPrice;
        private decimal _dinnerPrice;
        private string _theme;
        private string _postcode;
        private string _hint;
        private string _fullDetails;
        private int _duration;
        private int _minPeople;
        private int _maxPeople;
        private EventDetailCollection _eventDetails; 


        public int EventID
        {
            get { return _eventID; }
            set { _eventID = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public decimal LunchPrice
        {
            get { return _lunchPrice; }
            set { _lunchPrice = value; }
        }
        public decimal DinnerPrice
        {
            get { return _dinnerPrice; }
            set { _dinnerPrice = value; }
        }
        public string Theme
        {
            get { return _theme; }
            set { _theme = value; }
        }

        public string Postcode
        {
            get { return _postcode; }
            set { _postcode = value; }
        }

        public string Hint
        {
            get { return _hint; }
            set { _hint = value; }
        }

        public string FullDetails
        {
            get { return _fullDetails; }
            set { _fullDetails = value; }
        }

        public int Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
        public int MinPeople
        {
            get { return _minPeople; }
            set { _minPeople = value; }
        }
        public int MaxPeople
        {
            get { return _maxPeople ; }
            set { _maxPeople = value; }
        }

        public EventDetailCollection EventDetails
        {
            get { return _eventDetails; }
            set { _eventDetails = value; }
        }

        //Calls

        public static Event GetEvent(int Id)
        {
            DataAccess dbAccess = new DataAccess();
            return dbAccess.Get_Event(Id);
        }

        public static EventCollection GetEvents()
        {
            DataAccess dbAccess = new DataAccess();
            return dbAccess.Get_Events();
        }

        public Event()
            {
                DefaultConstructor();
            }

        private void DefaultConstructor()
        {
            _name = "";
            _startDate = DateTime.UtcNow;
            _endDate = DateTime.UtcNow.AddMonths(1);
            _description = "";
            _price = 0;
            _lunchPrice = 0;
            _dinnerPrice = 0;
            _theme = "";
            _postcode = "";
            _hint =  "";
            _fullDetails = "";
            _duration = 0;
            _minPeople = 0;
            _maxPeople = 0;
        }

    }
}