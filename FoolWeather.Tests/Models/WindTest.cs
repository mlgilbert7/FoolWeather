using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoolWeather.Models;

namespace FoolWeather.Tests.Models
{
    [TestClass]
    public class WindTest
    {
        [TestMethod]
        public void TestWindCollectionInOrderAndNoHoles()
        {
            Wind prev = null;
            foreach (Wind w in Wind.WindList)
            {
                if (prev != null)
                {
                    Assert.AreEqual(prev.Range2, w.Range1, "Failed " + w.Direction);
                    Assert.IsTrue(w.Range2 > w.Range1, "Failed " + w.Direction);
                    Assert.IsTrue(w.Range1 > prev.Range1, "Failed " + w.Direction);
                    Assert.IsTrue(w.Range2 > prev.Range2, "Failed " + w.Direction);
                }
                prev = w;
            }
        }

        [TestMethod]
        public void TestAllValues()
        {
            for (float degrees=0; degrees <= 360f; degrees += .05f)
            {
                Wind w = Wind.WindFromDegrees(degrees);
                Assert.IsTrue(degrees >= w.Range1, "Failed " + w.Direction + " Degrees " + degrees);
                Assert.IsTrue(degrees <= w.Range2, "Failed " + w.Direction + " Degrees " + degrees);
            }
        }

        [TestMethod]
        public void TestNorthWinds()
        {
            for (float degrees = 0; degrees <= 11.25f; degrees += .25f)
            {
                Wind w = Wind.WindFromDegrees(degrees);
                Assert.AreEqual("N", w.Direction);
                Assert.IsTrue(degrees >= w.Range1, "Failed " + w.Direction + " Degrees " + degrees);
                Assert.IsTrue(degrees <= w.Range2, "Failed " + w.Direction + " Degrees " + degrees);
            }

            for (float degrees = 349.0f; degrees <= 360.0f; degrees += .25f)
            {
                Wind w = Wind.WindFromDegrees(degrees);
                Assert.AreEqual("N", w.Direction);
                Assert.IsTrue(degrees >= w.Range1, "Failed " + w.Direction + " Degrees " + degrees);
                Assert.IsTrue(degrees <= w.Range2, "Failed " + w.Direction + " Degrees " + degrees);
            }
        }

        [TestMethod]
        public void TestVariableDirectionWinds()
        {
            Assert.AreEqual("variable directions", Wind.WindDirection(999));
        }

        [TestMethod]
        public void TestSomeRandomValues()
        {
            Assert.AreEqual("N", Wind.WindDirection(0f));
            Assert.AreEqual("NNE", Wind.WindDirection(11.26f));
            Assert.AreEqual("ENE", Wind.WindDirection(58f));
            Assert.AreEqual("S", Wind.WindDirection(180f));
            Assert.AreEqual("NNW", Wind.WindDirection(348.75f));
            Assert.AreEqual("N", Wind.WindDirection(348.76f));
        }

        [TestMethod]
        public void TestArgumentOutOfRangeException()
        {
            TestWindFromDegreesException(-.05f);
            TestWindFromDegreesException(360.05f);
        }

        private void TestWindFromDegreesException(float degrees)
        {
            try
            {
                Wind.WindFromDegrees(degrees);
                Assert.Fail("There should have been an ArgumentOutOfRangeException.");
            }
            catch(ArgumentOutOfRangeException)
            {}
        }
    }
}
