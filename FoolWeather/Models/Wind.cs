using System;
using System.Collections.Generic;

namespace FoolWeather.Models
{
    public class Wind
    {
        public string Direction;
        public float Range1;
        public float Range2;

        // Coversion data from http://climate.umn.edu/snow_fence/components/winddirectionanddegreeswithouttable3.htm
        public static List<Wind> WindList = new List<Wind>
        {
            new Wind { Direction = "N", Range1 = 0f, Range2 = 11.25f },
            new Wind { Direction = "NNE", Range1 = 11.25f, Range2 = 33.75f },
            new Wind { Direction = "NE", Range1 = 33.75f, Range2 = 56.25f },
            new Wind { Direction = "ENE", Range1 = 56.25f, Range2 = 78.75f },
            new Wind { Direction = "E", Range1 = 78.75f, Range2 = 101.25f },
            new Wind { Direction = "ESE", Range1 = 101.25f, Range2 = 123.75f },
            new Wind { Direction = "SE", Range1 = 123.75f, Range2 = 146.25f },
            new Wind { Direction = "SSE", Range1 = 146.25f, Range2 = 168.75f },
            new Wind { Direction = "S", Range1 = 168.75f, Range2 = 191.25f },
            new Wind { Direction = "SSW", Range1 = 191.25f, Range2 = 213.75f },
            new Wind { Direction = "SW", Range1 = 213.75f, Range2 = 236.25f },
            new Wind { Direction = "WSW", Range1 = 236.25f, Range2 = 258.75f },
            new Wind { Direction = "W", Range1 = 258.75f, Range2 = 281.25f },
            new Wind { Direction = "WNW", Range1 = 281.25f, Range2 = 303.75f },
            new Wind { Direction = "NW", Range1 = 303.75f, Range2 = 326.25f },
            new Wind { Direction = "NNW", Range1 = 326.25f, Range2 = 348.75f },
            new Wind { Direction = "N", Range1 = 348.75f, Range2 = 360f },
        };

        public static Wind WindFromDegrees(float degrees)
        {
            if (degrees < 0f || degrees > 360f)
                throw new ArgumentOutOfRangeException(
                    string.Format("Wind direction in degrees must be between 0 and 360.  {0} was supplied.",
                    degrees));

            foreach (Wind wind in WindList)
            {
                if (degrees >= wind.Range1 && degrees <= wind.Range2)
                    return wind;
            }

            throw new ApplicationException("Wind Direction not found.");
        }

        public static string WindDirection(float degrees)
        {
            if (degrees < 0f || degrees > 360f)
                return "variable directions";

            return WindFromDegrees(degrees).Direction;
        }
    }
}