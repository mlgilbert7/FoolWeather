using System;
using System.Net;
using System.Xml.Linq;

namespace FoolWeather.Models
{
    public class Geocoder
    {
        // Code from http://stackoverflow.com/questions/16274508/how-to-call-google-geocode-service-from-c-sharp-code

        public float Latitude;
        public float Longitude;
        public string Address;

        public void GetLocation(string address)
        {
            Address = address;
            string requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));

            WebRequest request = WebRequest.Create(requestUri);
            WebResponse response = request.GetResponse();
            XDocument xdoc = XDocument.Load(response.GetResponseStream());

            XElement result = xdoc.Element("GeocodeResponse").Element("result");
            XElement locationElement = result.Element("geometry").Element("location");
            Latitude = float.Parse(locationElement.Element("lat").Value);
            Longitude = float.Parse(locationElement.Element("lng").Value);
        }
    }
}