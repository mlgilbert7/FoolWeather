using FoolWeather.Models;
using System.Web.Mvc;

namespace FoolWeather.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "A small application that allows a user to input location data and displays the current weather information based on that location.";
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Address,Longitude,Latitude")]Home home, FormCollection formCollection)
        {
            if (home.Latitude != 0f && home.Longitude != 0)
            {
                home.Address = string.Format("Latitude {0:F2}, Longitude {1:F2}", home.Latitude, home.Longitude);
                return RedirectToAction("Edit", "Weather", home);
            }

            if (string.IsNullOrWhiteSpace(home.Address))
                return RedirectToAction("Create");

            Geocoder geo = new Geocoder();
            geo.GetLocation(home.Address);
            home.Latitude = geo.Latitude;
            home.Longitude = geo.Longitude;

            return RedirectToAction("Edit", "Weather", home);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "The man behind the curtain...";
            return View();
        }
    }
}