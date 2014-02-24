using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoolWeather.Models;

namespace FoolWeather.Tests.Models
{
    [TestClass]
    public class WeatherTest
    {
        private Weather _weather;

        [TestMethod]
        public void GetXmlFromWebSiteTest()
        {
            _weather = new Weather();
            _weather.LoadDocument(42.026705f, -87.686f);

            VerifyEvanstonLocation();
        }

        [TestMethod]
        public void GetWeatherFromGeocoderResultsTest()
        {
            Geocoder geo = new Geocoder();
            _weather = new Weather();

            geo.GetLocation("60202");
            _weather.LoadDocument(geo.Latitude, geo.Longitude);
            VerifyEvanstonLocation();

            geo.GetLocation("60201");
            _weather.LoadDocument(geo.Latitude, geo.Longitude);
            Assert.AreEqual("Evanston, IL", _weather.ForecastLocationDescription);
        }

        [TestMethod]
        public void GetWeatherFromGeocoderResultsTestGreenBay()
        {
            Geocoder geo = new Geocoder();
            _weather = new Weather();

            geo.GetLocation("54311");
            _weather.LoadDocument(geo.Latitude, geo.Longitude);
            Assert.AreEqual("4 Miles ENE Bellevue Town WI", _weather.ForecastLocationDescription);
        }

        [TestMethod]
        public void ReadTestFileTest()
        {
            _weather = new Weather();
            _weather.LoadDocument(@"..\..\TestFiles\MapClick.php.xml");

            VerifyEvanstonLocation();
            Assert.AreEqual("42 Fahrenheit", _weather.Temperature);
            Assert.AreEqual("55%", _weather.RelativeHumidity);
            Assert.AreEqual("Mostly Cloudy", _weather.Conditions);
            Assert.AreEqual(210f, _weather.WindDegrees);
            Assert.AreEqual("SSW", _weather.WindDirection);
            Assert.AreEqual("22 knots", _weather.WindGust);
            Assert.AreEqual("15 knots", _weather.WindSpeed);
            Assert.AreEqual("Winds from the SSW at 15 knots with gusts to 22 knots", _weather.WindDescription);
        }

        private void VerifyEvanstonLocation()
        {
            Assert.AreEqual("Evanston, IL", _weather.ForecastLocationDescription);
            Assert.AreEqual("Chicago/O'Hare, IL", _weather.ObservationLocationDescription);
        }
    }
}
