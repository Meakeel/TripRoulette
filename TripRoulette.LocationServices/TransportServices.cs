using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace TripRoulette.TransportServices
{

    public static class TransportServices
    {
        private const string _apiKey = "AIzaSyCBA2UGIPcrk7EGTsekh0cjNQkxf9mGa30";

        public static RouteInfo GetDirections(
            string origin,
            string destination,
            DateTime? depatureTime,
            DateTime? arrivalTime)
        {

            RouteInfo routeInfo = new RouteInfo();
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            StringBuilder requestUrl = new StringBuilder();
            requestUrl.Append("https://maps.googleapis.com/maps/api/directions/xml?");
            requestUrl.Append("origin=" + origin);
            requestUrl.Append("&destination=" + destination);
            requestUrl.Append("&units=metric");
            requestUrl.Append("&mode=transit");

            if(depatureTime.HasValue)
            {
                requestUrl.Append("&departure_time=" + ConvertToUnixTimestamp(depatureTime.Value).ToString());
            }

            if(arrivalTime.HasValue)
            {
                requestUrl.Append("&arrival_time=" + ConvertToUnixTimestamp(arrivalTime.Value).ToString());
            }
            
            requestUrl.Append("&key=" + _apiKey);

            WebRequest request = WebRequest.Create(requestUrl.ToString());
            
            using(WebResponse response = request.GetResponse())
            {
                StreamReader sr = new StreamReader(response.GetResponseStream());

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(sr.ReadToEnd());
                
                foreach(XmlElement drElement in xml.DocumentElement)
                {
                    switch (drElement.Name)
                    {
                        case "status":
                            routeInfo.Status = drElement.InnerText;
                            if (drElement.Value != "OK")
                            {
                                
                                //If the status is not equal to ok then the trip can not be met in time or for some other reason.
                            }
                            break;

                        case "route":

                            foreach(XmlElement routeElement in drElement)
                            {
                                switch(routeElement.Name)
                                {
                                    case "summary":
                                        
                                        break;

                                    case "leg":

                                        LegStep steps = new LegStep();
                                        ParseStep(routeElement, ref routeInfo, steps, steps);
                                        routeInfo.Steps = steps;
                                        break;

                                }

                            }

                            break;
                    }

                }

            }

            return routeInfo;

        }

        private static void ParseStep(XmlElement element, ref RouteInfo routeInfo, LegStep childStep, LegStep parentStep)
        {
            
            foreach (XmlElement child in element)
            {

                switch (child.Name)
                {
                    case "step":
                        
                        parentStep = childStep;
                        childStep = new LegStep();
                        ParseStep(child, ref routeInfo, childStep, parentStep);
                        parentStep.ChildrenSteps.Add(childStep);
                        childStep = parentStep;

                        break;

                    case "duration":
                        childStep.Duration.Text = child["text"].InnerText;
                        childStep.Duration.Value = Convert.ToInt64(child["value"].InnerText);
                        break;
                    case "distance":
                        childStep.Distance.Text = child["text"].InnerText;
                        childStep.Distance.Value = Convert.ToInt64(child["value"].InnerText);
                        break;
                    case "start_location":
                        childStep.StartLocation.Lat = Convert.ToDouble(child["lat"].InnerText);
                        childStep.StartLocation.Lng = Convert.ToDouble(child["lng"].InnerText);
                        break;
                    case "end_location":
                        childStep.EndLocation.Lat = Convert.ToDouble(child["lat"].InnerText);
                        childStep.EndLocation.Lng = Convert.ToDouble(child["lng"].InnerText);
                        break;
                    case "departure_time":
                        routeInfo.DepatureTime = child["text"].InnerText;
                        routeInfo.DepatureTimeUtc = Convert.ToInt64(child["value"].InnerText);
                        break;
                    case "arrival_time":
                        routeInfo.ArrivalTime = child["text"].InnerText;
                        routeInfo.ArrivalTimeUtc = Convert.ToInt64(child["value"].InnerText);
                        break;

                    case "travel_mode":
                        switch(child.InnerText)
                        {
                            case "WALKING":
                                childStep.TravelMode = LegStep.TravelModes.Walking;
                                break;
                            case "TRANSIT":
                                childStep.TravelMode = LegStep.TravelModes.Transit;
                                break;

                        }
                        break;

                    case "polyline":

                        break;

                    case "html_instructions":
                        childStep.Instructions = child.InnerText;
                        break;
                }

            }


        }

        public static double Distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {

            double theta = lon1 - lon2;

            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));

            dist = Math.Acos(dist);

            dist = rad2deg(dist);

            dist = dist * 60 * 1.1515;

            if (unit == 'K')
            {

                dist = dist * 1.609344;

            }
            else if (unit == 'N')
            {

                dist = dist * 0.8684;

            }

            return (dist);
        }

        public static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        public static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
        
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

    }

    public class TravelInfo
    {
               
        #region Fields
            private RouteInfo _routes;
        #endregion

        #region Properties
            public RouteInfo Routes {
                get
                {
                    return _routes;
                }
                set
                {
                    _routes = value;
                }
            }
        #endregion

        #region Constructors

            public TravelInfo()
            {
                DefaultConstructor();
            }

            private void DefaultConstructor()
            {
                _routes = new RouteInfo();
            }

        #endregion
        
    }

    public class RouteInfo
    {

        #region Fields
            private string _arrivalTime;
            private long _arrivalTimeUtc;
            private string _departureTime;
            private long _departureTimeUtc;
            private string _distanceFriendly;
            private long _distance;
            private string _durationFriendly;
            private long _duration;
            private string _endAddress;
            private Coordinates _endLocation;
            private string _startAddress;
            private Coordinates _startLocation;
            private LegStep _steps;
            private string _status;
        #endregion

        #region Properties
            public string ArrivalTime {
                get
                {
                    return _arrivalTime;
                }
                set
                {
                    _arrivalTime = value;
                }
            }
            public long ArrivalTimeUtc 
            {
                get
                {
                    return _arrivalTimeUtc;
                }
                set
                {
                    _arrivalTimeUtc = value;
                }
            }
            public string DepatureTime
            {
                get
                {
                    return _departureTime;
                }
                set
                {
                    _departureTime = value;
                }
            }
            public long DepatureTimeUtc
            {
                get
                {
                    return _departureTimeUtc;
                }
                set
                {
                    _departureTimeUtc = value;
                }
            }
            public string DistanceFriendly
            {
                get
                {
                    return _distanceFriendly;
                }
                set
                {
                    _distanceFriendly = value;
                }
            }
            public long Distance
            {
                get
                {
                    return _distance;
                }
                set
                {
                    _distance = value;
                }
            }
            public string DurationFriendly
            {
                get
                {
                    return _durationFriendly;
                }
                set
                {
                    _durationFriendly = value;
                }
            }
            public long Duration
            {
                get
                {
                    return _duration;
                }
                set
                {
                    _duration = value;
                }
            }
            public string EndAddress
            {
                get
                {
                    return _endAddress;
                }
                set
                {
                    _endAddress = value;
                }
            }
            public string StartAddress
            {
                get
                {
                    return _startAddress;
                }
                set
                {
                    _startAddress = value;
                }
            }
            public Coordinates StartLocation
            {
                get { return _startLocation; }
                set { _startLocation = value; }
            }
            public Coordinates EndLocation
            {
                get { return _endLocation; }
                set { _endLocation = value; }
            }
            public LegStep Steps
            {
                get
                {
                    return _steps;
                }
                set
                {
                    _steps = value;
                }

            }
            public string Status
            {
                get
                {
                    return _status;
                }

                set
                {
                    _status = value;
                }

            }

        #endregion

        #region Constructors

            public RouteInfo()
            {
                DefaultConstructor();
            }

            private void DefaultConstructor()
            {
                _arrivalTime = "";
                _arrivalTimeUtc = 0;
                _departureTime = "";
                _departureTimeUtc = 0;
                _distanceFriendly = "";
                _distance = 0;
                _durationFriendly = "";
                _duration = 0;
                _endAddress = "";
                _endLocation = new Coordinates();
                _startAddress = "";
                _startLocation = new Coordinates();
                _steps = new LegStep();
                _status = "";
            }

        #endregion

    }

    public class LegStep
    {

        #region Enumerators

            public enum TravelModes
            {
                None,
                Walking,
                Transit,
                Driving
            }

        #endregion

        #region Fields

            private TravelModes _travelMode;
            private Coordinates _startlocation;
            private Coordinates _endlocation;
            private string _polyline;
            private TextValue _duration;
            private TextValue _distance;
            private string _instructions;
            private List<LegStep> _childrenSteps;

        #endregion

        #region Properties
            
            public TravelModes TravelMode
            {
                get { return _travelMode; }
                set { _travelMode = value; }
            }

            public Coordinates StartLocation
            {
                get { return _startlocation; }
                set { _startlocation = value; }
            }

            public Coordinates EndLocation
            {
                get { return _endlocation; }
                set { _endlocation = value; }
            }
            
            public string Polyline
            {
                get { return _polyline; }
                set { _polyline = value; }
            }

            public TextValue Duration
            {

                get
                {
                    return _duration;
                }

                set
                {
                    _duration = value;
                }
                
            }

            public TextValue Distance
            {

                get
                {
                    return _distance;
                }

                set
                {
                    _distance = value;
                }

            }
            
            public string Instructions
            {
                get
                {
                    return _instructions;
                }
                set
                {
                    _instructions = value;
                }
            }

            public List<LegStep> ChildrenSteps
            {
                get
                {
                    return _childrenSteps;
                }

                set
                {
                    _childrenSteps = value;
                }
            }

        #endregion
        
        #region Constructors

            public LegStep()
            {
                DefaultConstructor();
            }

            private void DefaultConstructor()
            {
                _travelMode = TravelModes.None;
                _startlocation = new Coordinates();
                _endlocation = new Coordinates();
                _polyline = "";
                _duration = new TextValue();
                _instructions = "";
                ChildrenSteps = new List<LegStep>();
                _distance = new TextValue();
            }

        #endregion

    }

    public class Coordinates
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class TextValue
    {
        private string _text;
        private Int64 _value;
        
        public string Text { get { return _text; } set { _text = value; } }
        public Int64 Value { get { return _value; } set { _value = Value; } }

        public TextValue()
        {
            _text = "";
            _value = 0;
        }
    }
    
}



