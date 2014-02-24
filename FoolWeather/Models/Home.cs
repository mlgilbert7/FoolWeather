using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoolWeather.Models
{
    public class Home
    {
        public Home()
        {
            Latitude = 0f;
            Longitude = 0f;
        }

        [DisplayName("\"City, State\" or \"Zip\"")]
        public string Address { get; set; }
        [UIHint("Reading")]
        public float Latitude { get; set; }
        [UIHint("Reading")]
        public float Longitude { get; set; }
    }
}