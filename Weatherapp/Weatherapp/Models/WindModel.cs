using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Weatherapp.Models
{
    public class WindModel
    {
        //Wind
        public double LatestWind { get; set; }
        public double Bearing { get; set; }
        public double Gust { get; set; }
        public double AverageWind { get; set; }
        public double AvgDirection { get; set; }
        public double WindRun { get; set; }

        public int WindModelId { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Self
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
               "api/RecentExtremesModels/{0}", this.WindModelId);
            }
            set { }
        }

        public WindModel(double latestWind, double bearing, double gust, double averageWind, double avgDirection, double windRun, DateTime dateAndTime)
        {
            LatestWind = latestWind;
            Bearing = bearing;
            Gust = gust;
            AverageWind = averageWind;
            AvgDirection = avgDirection;
            WindRun = windRun;
            DateAndTime = dateAndTime;
        }

        public WindModel()
        {
            LatestWind = 0.0;
            Bearing = 0.0;
            Gust = 0.0;
            AverageWind = 0.0;
            AvgDirection = 0.0;
            WindRun = 0.0;
            DateAndTime = DateTime.Now;
        }
    }
}