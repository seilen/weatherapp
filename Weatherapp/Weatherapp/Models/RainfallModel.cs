using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Weatherapp.Models
{
    public class RainfallModel
    {
        //Rainfall
        public double RainfallRate { get; set; }
        public double RainfallLastHour { get; set; }
        public double RainfallToday { get; set; }
        public double RainfallLast24Hours { get; set; }
        public double RainfallYesterday { get; set; }
        public double RainfallThisMonth { get; set; }
        public double RainfallThisYear { get; set; }

        public int RainfallModelId { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Self
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
               "api/RainfallModels/{0}", this.RainfallModelId);
            }
            set { }
        }

        public RainfallModel(double rainRate, double rainLastHour, double rainToday, double rainLast24Hours, double rainYesterday, double rainThisMonth, double rainThisYear, DateTime dateAndTime)
        {
            RainfallRate = rainRate;
            RainfallLastHour = rainLastHour;
            RainfallToday = rainToday;
            RainfallLast24Hours = rainLast24Hours;
            RainfallYesterday = rainYesterday;
            RainfallThisMonth = rainThisMonth;
            RainfallThisYear = rainThisYear;
            DateAndTime = dateAndTime;
        }

        public RainfallModel()
        {
            RainfallRate = 0.0;
            RainfallLastHour = 0.0;
            RainfallToday = 0.0;
            RainfallLast24Hours = 0.0;
            RainfallYesterday = 0.0;
            RainfallThisMonth = 0.0;
            RainfallThisYear = 0.0;
            DateAndTime = DateTime.Now;
        }
    }
}