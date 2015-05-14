using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Weatherapp.Models
{
    public class RecentExtremesModel
    {
        //Rainfall
        public double HighWind { get; set; }
        public double HighGust { get; set; }
        public double Bearing { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public double MinPressure { get; set; }
        public double MaxPressure { get; set; }
        public double RainRate { get; set; }

        public int RecentExtremesModelId { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Self
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
               "api/RecentExtremesModels/{0}", this.RecentExtremesModelId);
            }
            set { }
        }

        public RecentExtremesModel(double highWind, double highGust, double bearing, double minTemp, double maxTemp, double minPressure, double maxPressure, double rainRate, DateTime dateAndTime)
        {
            HighWind = highWind;
            HighGust = highGust;
            Bearing = bearing;
            MinTemp = minTemp;
            MaxTemp = maxTemp;
            MinPressure = minPressure;
            MaxPressure = maxPressure;
            RainRate = rainRate;
            DateAndTime = dateAndTime;
        }

        public RecentExtremesModel()
        {
            HighWind = 0.0;
            HighGust = 0.0;
            Bearing = 0.0;
            MinTemp = 0.0;
            MaxTemp = 0.0;
            MinPressure = 0.0;
            MaxPressure = 0.0;
            RainRate = 0.0;
            DateAndTime = DateTime.Now;
        }
    }
}