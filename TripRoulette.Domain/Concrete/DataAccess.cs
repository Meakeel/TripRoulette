using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripRoulette.Domain.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TripRoulette.Domain.Concrete
{
    public class DataAccess
    {
        public String SqlconnectionString = ConfigurationManager.ConnectionStrings["TripRouletteDB"].ConnectionString;

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
            SqlCommand cmd = new SqlCommand(strSqlQry, sqlConnection);

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

        public EventCollection Get_EventAndDetails(int eventID)
        {
            EventCollection eventCollection = new EventCollection();

            using (SqlConnection sqlConnection = new SqlConnection(SqlconnectionString))
            {
                sqlConnection.Open();
                string strSqlQry = "select * from Event where eventID=" + eventID.ToString();
                
                SqlCommand cmd = new SqlCommand(strSqlQry, sqlConnection);

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

            }

            using (SqlConnection sqlConnection = new SqlConnection(SqlconnectionString))
            {
                sqlConnection.Open();
                foreach (Event row in eventCollection)
                {

                    EventDetailCollection eventDetails = new EventDetailCollection();

                    string strSqlQryDetails = "select * from EventDetail where eventID =" + row.EventID.ToString();
                    SqlCommand cmd = new SqlCommand(strSqlQryDetails, sqlConnection);
                    
                    SqlDataReader t = cmd.ExecuteReader();
                    while (t.Read())
                    {
                        EventDetail eventDetailRow = new EventDetail();
                        eventDetailRow.EventDetailID = Convert.ToInt32(t["eventDetailID"]);
                        eventDetailRow.EventID = Convert.ToInt32(t["eventID"]);
                        eventDetailRow.DayOfWeek = Convert.ToInt32(t["dayOfWeek"]);
                        eventDetailRow.StartTime = Convert.ToDateTime(t["startTime"]);
                        eventDetailRow.EndTime = Convert.ToDateTime(t["endTime"]);
                        eventDetailRow.allDay = Convert.ToBoolean(t["allDay"]);
                        eventDetails.Add(eventDetailRow);
                    }

                    t.Close();

                    row.EventDetails = eventDetails;

                }
            }

            return eventCollection;
        }

        public EventCollection Get_EventsAndDetails()
        {
            EventCollection eventCollection = new EventCollection();

            using (SqlConnection sqlConnection = new SqlConnection(SqlconnectionString))
            {
                string strSqlQry = "select * from Event";

                SqlCommand cmd = new SqlCommand(strSqlQry, sqlConnection);

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

            }

            using (SqlConnection sqlConnection = new SqlConnection(SqlconnectionString))
            {
                sqlConnection.Open();
                foreach (Event row in eventCollection)
                {

                    EventDetailCollection eventDetails = new EventDetailCollection();

                    string strSqlQryDetails = "select * from EventDetail where eventID =" + row.EventID.ToString();
                    SqlCommand cmd = new SqlCommand(strSqlQryDetails, sqlConnection);

                    SqlDataReader t = cmd.ExecuteReader();
                    while (t.Read())
                    {
                        EventDetail eventDetailRow = new EventDetail();
                        eventDetailRow.EventDetailID = Convert.ToInt32(t["eventDetailID"]);
                        eventDetailRow.EventID = Convert.ToInt32(t["eventID"]);
                        eventDetailRow.DayOfWeek = Convert.ToInt32(t["dayOfWeek"]);
                        eventDetailRow.StartTime = Convert.ToDateTime(t["startTime"]);
                        eventDetailRow.EndTime = Convert.ToDateTime(t["endTime"]);
                        eventDetailRow.allDay = Convert.ToBoolean(t["allDay"]);
                        eventDetails.Add(eventDetailRow);
                    }

                    t.Close();

                    row.EventDetails = eventDetails;

                }
            }

            return eventCollection;
        }

        public void InsertEvent(Event row)
        {
            SqlConnection sqlConnection = new SqlConnection(SqlconnectionString);
            sqlConnection.Open();

            SqlCommand cmd = new SqlCommand("insertEvent");
            cmd.Connection = sqlConnection;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@name", row.Name);
            cmd.Parameters.AddWithValue("@startDate", row.StartDate);
            cmd.Parameters.AddWithValue("@endDate", row.EndDate);
            cmd.Parameters.AddWithValue("@description", row.Description);
            cmd.Parameters.AddWithValue("@price", row.Price);
            cmd.Parameters.AddWithValue("@lunchPrice", row.LunchPrice);
            cmd.Parameters.AddWithValue("@dinnerPrice", row.DinnerPrice);
            cmd.Parameters.AddWithValue("@theme", row.Theme);
            cmd.Parameters.AddWithValue("@postcode", row.Postcode);
            cmd.Parameters.AddWithValue("@hint", row.Hint);
            cmd.Parameters.AddWithValue("@fulldetails", row.FullDetails);
            cmd.Parameters.AddWithValue("@duration", row.Duration);
            cmd.Parameters.AddWithValue("@minPeople", row.MinPeople);
            cmd.Parameters.AddWithValue("@maxPeople", row.MaxPeople);

            cmd.ExecuteNonQuery();

        }

        public void InsertEventDetail(EventDetail row)
        {
            SqlConnection sqlConnection = new SqlConnection(SqlconnectionString);
            sqlConnection.Open();

            SqlCommand cmd = new SqlCommand("insertEventDetail");
            cmd.Connection = sqlConnection;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@eventID", row.EventID);
            cmd.Parameters.AddWithValue("@dayOfWeek", row.DayOfWeek);
            cmd.Parameters.AddWithValue("@startTime", row.StartTime);
            cmd.Parameters.AddWithValue("@endTime", row.EndTime);
            cmd.Parameters.AddWithValue("@allDay", row.allDay);
            cmd.ExecuteNonQuery();

        }

        public void deleteEvent(int eventId)
        {
            SqlConnection sqlConnection = new SqlConnection(SqlconnectionString);
            sqlConnection.Open();
            string strSqlQry = "delete from Event where eventID=" + eventId.ToString();
            SqlCommand cmd = new SqlCommand(strSqlQry, sqlConnection);
            cmd.ExecuteNonQuery();

        }

    }
}