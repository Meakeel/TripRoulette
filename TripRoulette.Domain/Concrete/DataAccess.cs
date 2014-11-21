using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripRoulette.Domain.Entities;
using System.Data;
using System.Data.SqlClient;

namespace TripRoulette.Domain.Concrete
{
    public class DataAccess
    {
        public string SqlconnectionString = "Server=localhost\\SQL2012;Database=TravelRoulette;User Id=sa;Password=TravelRoulette;";

        public Event Get_Event(int id)
        {
            SqlConnection sqlConnection = new SqlConnection(SqlconnectionString);
            sqlConnection.Open();
            string strSqlQry = "select top 1 * from Event where eventID=" + id.ToString();
            SqlCommand cmd = new SqlCommand(strSqlQry, sqlConnection);

            Event eventrow = new Event();

            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                eventrow.EventID = Convert.ToInt32(r["eventID"]);
                eventrow.Name = Convert.ToString(r["name"]);
                eventrow.StartDate = Convert.ToDateTime(r["startDate"]);
                eventrow.EndDate = Convert.ToDateTime(r["endDate"]);
                eventrow.Description = Convert.ToString(r["description"]);
                eventrow.Price = Convert.ToDecimal(r["price"]);
                eventrow.LunchPrice = Convert.ToDecimal(r["lunchPrice"]);
                eventrow.DinnerPrice = Convert.ToDecimal(r["dinnerPrice"]);
                eventrow.Theme = Convert.ToString(r["theme"]);
                eventrow.Postcode = Convert.ToString(r["postcode"]);
                eventrow.Hint = Convert.ToString(r["hint"]);
                eventrow.FullDetails = Convert.ToString(r["fullDetails"]);
                eventrow.Duration = Convert.ToInt32(r["duration"]);
                eventrow.MinPeople = Convert.ToInt32(r["minPeople"]);
                eventrow.MaxPeople = Convert.ToInt32(r["maxPeople"]);
            }
            return eventrow;
        }

        public EventCollection Get_Events()
        {
            EventCollection eventCollection = new EventCollection();

            SqlConnection sqlConnection = new SqlConnection(SqlconnectionString); 
            sqlConnection.Open();
            string strSqlQry = "select * from Event";
            SqlCommand cmd = new SqlCommand(strSqlQry,sqlConnection);

            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Event eventrow = new Event();
                eventrow.EventID = Convert.ToInt32(r["eventID"]);
                eventrow.Name = Convert.ToString(r["name"]);
                eventrow.StartDate = Convert.ToDateTime(r["startDate"]);
                eventrow.EndDate = Convert.ToDateTime(r["endDate"]);
                eventrow.Description = Convert.ToString(r["description"]);
                eventrow.Price = Convert.ToDecimal(r["price"]);
                eventrow.LunchPrice = Convert.ToDecimal(r["lunchPrice"]);
                eventrow.DinnerPrice = Convert.ToDecimal(r["dinnerPrice"]);
                eventrow.Theme = Convert.ToString(r["theme"]);
                eventrow.Postcode = Convert.ToString(r["postcode"]);
                eventrow.Hint = Convert.ToString(r["hint"]);
                eventrow.FullDetails = Convert.ToString(r["fullDetails"]);
                eventrow.Duration = Convert.ToInt32(r["duration"]);
                eventrow.MinPeople = Convert.ToInt32(r["minPeople"]);
                eventrow.MaxPeople = Convert.ToInt32(r["maxPeople"]);
                eventCollection.Add(eventrow);
            }

            return eventCollection;
        }

        public void InsertEvent(Event row)
        { 
        
        }
    
    }
}