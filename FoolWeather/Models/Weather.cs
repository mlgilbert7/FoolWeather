using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace FoolWeather.Models
{
    public class Weather
    {
        public string Address { get; set; }

        [DisplayName("Address (City, State or Zip Code)")]
        public string NewAddress { get; set; }
        
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public XDocument XDoc;
        public const string UrlFmt = @"http://forecast.weather.gov/MapClick.php?lat={0}&lon={1}&FcstType=dwml";
        public const string MapUrlFmt = @"http://maps.googleapis.com/maps/api/staticmap?center={0},{1}&zoom=14&size=360x300&sensor=false";
        public const string NotFound = "Location not found.";

        public void LoadDocument(float latitude, float longitude)
        {
            try
            {
                string url = string.Format(UrlFmt, latitude, longitude);
                Latitude = latitude;
                Longitude = longitude;
                LoadDocument(url);
            }
            catch 
            {
                XDoc = null;
            }
        }

        public void LoadDocument(string uri)
        {
            XDoc = XDocument.Load(uri);
        }

        [DisplayName("Location")]
        public string ForecastLocationDescription
        {
            get 
            {
                if (XDoc == null) return NotFound;
                try
                {
                    return DataElementWithType("forecast").Element("location").Element("description").Value;
                }
                catch (NullReferenceException exc)
                {
                    return DataElementWithType("forecast").Element("location").Element("area-description").Value;
                }
            }
        }

        [DisplayName("Observed At")]
        public string ObservationLocationDescription
        {
            get 
            {
                if (XDoc == null) return null;
                try
                {
                    return DataElementWithType("current observations").Element("location").Element("description").Value; 
                }
                catch (NullReferenceException exc)
                {
                    return DataElementWithType("current observations").Element("location").Element("area-description").Value; 
                }
            }
        }

        public int DegreesTemp
        {
            get
            {
                if (XDoc == null) return -460;
                XElement element = DataElementWithType("current observations").Element("parameters").
                    Element("temperature");
                return int.Parse(element.Value);
            }
        }

        public string Temperature
        {
            get
            {
                if (XDoc == null) return null;
                XElement element = DataElementWithType("current observations").Element("parameters").
                    Element("temperature");
                return element.Value + " " + element.Attribute("units").Value;
            }
        }

        [DisplayName("Relative Humidity")]
        public string RelativeHumidity
        {
            get
            {
                if (XDoc == null) return null;
                XElement element = DataElementWithType("current observations").Element("parameters").
                    Element("humidity");
                return element.Value + "%";
            }
        }

        public string Conditions
        {
            get
            {
                if (XDoc == null) return null;
                XElement weatherNode = DataElementWithType("current observations").Element("parameters").
                    Element("weather");
                return NameAndAttributeTypeFind(weatherNode, "weather-conditions", "weather-summary");
            }
        }

        [DisplayName("Wind Degrees")]
        public float WindDegrees
        {
            get
            {
                if (XDoc == null) return 0;
                XElement windNode = DataElementWithType("current observations").Element("parameters").
                    Element("direction");
                return float.Parse(windNode.Value);
            }
        }

        [DisplayName("Wind Direction")]
        public string WindDirection
        {
            get { return Wind.WindDirection(WindDegrees); }
        }


        [DisplayName("Wind Gusts")]
        public string WindGust 
        {
            get 
            {
                if (XDoc == null) return null;
                XElement parmNode = DataElementWithType("current observations").Element("parameters");
                XElement gustNode = NameAndAttributeValueFind(parmNode, "wind-speed", "type", "gust");
                return gustNode.Value + " " + gustNode.Attribute("units").Value;
            } 
        }

        [DisplayName("Wind Speed")]
        public string WindSpeed
        {
            get
            {
                if (XDoc == null) return null;
                XElement parmNode = DataElementWithType("current observations").Element("parameters");
                XElement speedNode = NameAndAttributeValueFind(parmNode, "wind-speed", "type", "sustained");
                return speedNode.Value + " " + speedNode.Attribute("units").Value;
            }
        }

        [DisplayName("Wind Description")]
        public string WindDescription
        {
            get 
            {
                if (XDoc == null) return null;
                return string.Format("Winds from the {0} at {1} with gusts to {2}", 
                    WindDirection, WindSpeed, WindGust); 
            }
        }

        public string ImageSourceByTemperature
        {
            get
            {
                if (ForecastLocationDescription == NotFound)
                    return "/files/notfound.jpg";

                int degrees = DegreesTemp;
                if (DegreesTemp > 72)
                    return "/files/hot.jpg";
                if (DegreesTemp > 60)
                    return "/files/warm.jpg";
                if (DegreesTemp > 50)
                    return "/files/comfortable.jpg";
                if (DegreesTemp > 40)
                    return "/files/cool.jpg";
                if (DegreesTemp > 30)
                    return "/files/chilly.jpg";

                return "/files/cold.jpg";
            }
        }

        public string MapUrl
        {
            get { return string.Format(MapUrlFmt, Latitude, Longitude); }
        }

        private XElement DataElementWithType(string type)
        {
            return (XDoc == null) ? null : NameAndAttributeValueFind(XDoc.Root, "data", "type", type);
        }

        private XElement NameAndAttributeValueFind(XElement node, string name, string attibuteType, string value)
        {
            foreach (XElement element in node.Elements(name))
                if (element.Attribute(attibuteType).Value == value)
                    return element;

            throw new ApplicationException("Element \"" + name + "\" with attribute \"" + attibuteType +  
                "\" and value = \"" + value + "\" was not found.");
        }

        private string NameAndAttributeTypeFind(XElement node, string name, string attibuteType)
        {
            foreach (XElement element in node.Elements(name))
                if (element.Attribute(attibuteType) != null)
                    return element.Attribute(attibuteType).Value;

            throw new ApplicationException("Element \"" + name + "\" with attribute \"" + attibuteType +
                "\" was not found.");
        }
    }
}