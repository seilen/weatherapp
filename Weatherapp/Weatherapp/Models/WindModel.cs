using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Weatherapp.Models
{
    public class WindModel
    {
        public double WindSpeed;
        public double Gust { get; set; }
        public string WindDirection;
        public double DewPoint;
        public double WindChill;
        public DateTime DateAndTime;

        //Wind
        /*public double LatestWind { get; set; }
        public double Bearing { get; set; }
        public double AverageWind { get; set; }
        public double AvgDirection { get; set; }
        public double WindRun { get; set; }
        */
        public int WindModelId { get; set; }
        public string Self
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
               "api/RecentExtremesModels/{0}", this.WindModelId);
            }
            set { }
        }

       /* public WindModel(double latestWind, double bearing, double gust, double averageWind, double avgDirection, double windRun, DateTime dateAndTime)
        {
            LatestWind = latestWind;
            Bearing = bearing;
            Gust = gust;
            AverageWind = averageWind;
            AvgDirection = avgDirection;
            WindRun = windRun;
            DateAndTime = dateAndTime;
        }*/

        public WindModel()
        {
            WindSpeed = 0.0;
            Gust = 0.0;
            WindDirection = "N";
            DewPoint = 0.0;
            WindChill = 0.0;
            DateAndTime = DateTime.Now;
        }

        public WindModel(double windSpeed, double gust, string windDirection, double dewPoint, double windChill, DateTime time)
        {
            WindSpeed = windSpeed;
            Gust = gust;
            WindDirection = windDirection;
            DewPoint = dewPoint;
            WindChill = windChill;
            DateAndTime = time;
        }
    }
}