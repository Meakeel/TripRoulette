using Engine.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TripRoulette.TransportServices;
using System.Web;

namespace TripRoulette.RandomGenerator
{
    public class RandomGenerator
    {

        public static StringBuilder CreateEmail(DataRow eventRow, RouteInfo routeInfo, string emailAddress, string eventName, string eventDate)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("It's now time to get up and go...<br/><br/>");
            sb.AppendLine("Here are the directions to your spontaneous adventure!<br/><br/>");
            GetStep(ref sb, routeInfo.Steps);

            sb.AppendLine(eventName + "<br/><br/>");
            sb.AppendLine(eventDate + "<br/>");

            sb.AppendLine("<br/>Have a great time!<br/><br/>");
            sb.AppendLine("The Trip Roulette Team");

            /*
             * It's now time to get up and go...
             * 
             * Here are the directions to your spontaneous adventure!
             * 
             * Directions
             * 
             * Have a great time!
             * 
             * 
             * The Trip Roulette Team
             * */
            HttpContext context = HttpContext.Current;

            context.Trace.Warn("I got this far before sending the email.");

            try
            {

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("travelroulettenorwich@gmail.com", "R17at71!")
                };
                using (var message = new MailMessage("travelroulettenorwich@gmail.com", emailAddress)
                {
                    Subject = "We're ready are you?",
                    Body = sb.ToString(),
                    IsBodyHtml = true
                })
                {
                    context.Trace.Warn("Im sending the email");
                    smtp.Send(message);
                }

            }
            catch (Exception ex)
            {
                context.Trace.Warn(ex.Message);

            }


            return sb;
        }

        private static string lastDirection = "";

        public static void GetStep(ref StringBuilder sb, LegStep step)
        {

            foreach (LegStep s in step.ChildrenSteps)
            {
                if (lastDirection != (s.Distance.Text + " " + s.Instructions + "<br/>"))
                {
                    lastDirection = (s.Distance.Text + " " + s.Instructions + "<br/>");
                    sb.AppendLine(lastDirection);
                }

                if (s.ChildrenSteps.Count > 0)
                {
                    GetStep(ref sb, s);
                }
            }
        }

        public static bool GenerateRandomTrip(
            int numberOfPeople,
            DateTime arriveDate,
            decimal maxBudget,
            DateTime? departDate,
            string postcode,
            string emailAddress
        )
        {
            try
            {
                RouteInfo info = TransportServices.TransportServices.GetDirections(postcode, "NR294SY", null, DateTime.Now.AddDays(2));
                double lat = info.Steps.StartLocation.Lat;
                double lng = info.Steps.StartLocation.Lng;

                postcode = postcode.Trim();

                SQLHelper SQL = new SQLHelper("Server=tcp:bp90bdr9uj.database.windows.net,1433;Database=TripRoulette;User ID=triproulette@bp90bdr9uj;Password= Sp1nTheWh33ls;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;");

                DataTable dt = SQL.Execute<DataTable>
                (
                    SQLHelper.CommandTypes.StoredProcedure,
                    "SELECTRandomEvent",
                    SQLHelper.ExecuteTypes.Reader,
                    new List<SqlParameter>(){
                    new SqlParameter("NumberOfPeople",numberOfPeople),
                    new SqlParameter("MaxBudget",maxBudget)
                }
                );

                int index = 0;
                List<int> matches = new List<int>();
                bool isMatch = true;
                RouteInfo routeInfo = null;

                foreach (DataRow row in dt.Rows)
                {
                    double eventLat = Convert.ToDouble(row["lat"].ToString());
                    double eventLng = Convert.ToDouble(row["lng"].ToString());

                    double miles = TransportServices.TransportServices.Distance(eventLat, eventLng, lat, lng, 'M');

                    isMatch = true;

                    if (miles >= 20)
                    {
                        isMatch = false;
                    }

                    if (isMatch)
                    {
                        matches.Add(index);
                    }
                    index++;

                }

                Random rnd = new Random(DateTime.Now.Ticks.GetHashCode());
                int rndIndex = rnd.Next(0, matches.Count - 1);
                bool canLoop = true;
                List<int> tried = new List<int>();
                bool foundEvent = false;
                string eventName = "";
                string eventDate = "";

                while (canLoop)
                {
                    DataRow row = dt.Rows[matches[rndIndex]];

                    DataTable dtTimes = SQL.Execute<DataTable>(
                        SQLHelper.CommandTypes.Text,
                        "SELECT * FROM EventDetail WHERE eventId = " + row["eventId"].ToString() + " ORDER BY startTime DESC ",
                        SQLHelper.ExecuteTypes.Reader,
                        null
                    );

                    if (dtTimes.Rows.Count > 0)
                    {

                        DateTime eventTime = DateTime.Parse(dtTimes.Rows[0]["startTime"].ToString());
                        eventTime = DateTime.Parse(arriveDate.ToString("yyyy-MM-dd") + " " + eventTime.ToString("HH:mm:ss"));

                        routeInfo = TransportServices.TransportServices.GetDirections(postcode, row["postcode"].ToString(), null, eventTime);
                        if (routeInfo.Status == "OK")
                        {
                            eventName = dt.Rows[rndIndex]["name"].ToString();
                            eventDate = eventTime.ToString();
                            //found event that matches the criteria
                            canLoop = false;
                            foundEvent = true;
                        }
                        else
                        {
                            tried.Add(rndIndex);
                            bool search = true;
                            while (search)
                            {
                                rndIndex = rnd.Next(0, matches.Count - 1);

                                if (!tried.Contains(rndIndex))
                                {
                                    search = false;
                                }

                            }

                            //This event can not be forefilled, try another.
                        }
                    }
                }

                if (foundEvent)
                {
                    if (routeInfo != null)
                    {
                        CreateEmail(dt.Rows[rndIndex], routeInfo, emailAddress, eventName, eventDate);
                    }
                }

                

            }

            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);

            }
            return true;
        }

    }

}
