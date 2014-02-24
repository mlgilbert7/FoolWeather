using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoolWeather.Models;

namespace FoolWeather.Controllers
{
    public class WeatherController : Controller
    {
        //
        // GET: /Weather/
        public string Index()
        {
            //return View();
            return "This is my <b>default</b> action..."; 
        } 
 
        // 
        // GET: /HelloWorld/Welcome/ 
 
        public string Weather(string latitude, string longitude) 
        {
            Weather w = new Weather();
            w.LoadDocument(float.Parse(latitude), float.Parse(longitude));

            return "This is the Welcome action method:  " + w.Conditions; 
        }
 
        public ActionResult Details(string latitude, string longitude, string address = null)
        {
            Weather w = new Weather { Address = address };
            w.LoadDocument(float.Parse(latitude), float.Parse(longitude));

            return View(w);
        }

        public ActionResult Edit(string latitude, string longitude, string address = null)
        {
            Weather w = new Weather { Address = address };
            w.LoadDocument(float.Parse(latitude), float.Parse(longitude));

            return View(w);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NewAddress")]Weather weather)
        {
            if (string.IsNullOrWhiteSpace(weather.NewAddress))
                return RedirectToAction("Create", "Home");

            Geocoder geo = new Geocoder();
            geo.GetLocation(weather.NewAddress);

            weather.Latitude = geo.Latitude;
            weather.Longitude = geo.Longitude;
            weather.Address = weather.NewAddress;
            weather.NewAddress = null;

            return RedirectToAction("Edit", "Weather", weather);
        }
    }
}