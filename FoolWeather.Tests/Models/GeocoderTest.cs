using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoolWeather.Models;

namespace FoolWeather.Tests.Models
{
    [TestClass]
    public class GeocoderTest
    {
        [TestMethod]
        public void FindLocationTest()
        {
            Geocoder geo = new Geocoder();
            geo.GetLocation("60202");
            Assert.AreEqual(42.03013f, geo.Latitude);
            Assert.AreEqual(-87.68277f, geo.Longitude);
            Assert.AreEqual("60202", geo.Address);
        }
    }
}
