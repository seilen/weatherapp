using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Weatherapp.Models
{
    public class RainfallModel
    {
        public double HourRainfall;
        public double DayRainfall;
        public double WeekRainfall;
        public double TotalRainfall;
        public DateTime DateAndTime;

        //Rainfall
        /*public double RainfallRate { get; set; }
        public double RainfallLastHour { get; set; }
        public double RainfallToday { get; set; }
        public double RainfallLast24Hours { get; set; }
        public double RainfallYesterday { get; set; }
        public double RainfallThisMonth { get; set; }
        public double RainfallThisYear { get; set; }
        */
        public int RainfallModelId { get; set; }
        public string Self
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
               "api/RainfallModels/{0}", this.RainfallModelId);
            }
            set { }
        }

        /*public RainfallModel(double rainRate, double rainLastHour, double rainToday, double rainLast24Hours, double rainYesterday, double rainThisMonth, double rainThisYear, DateTime dateAndTime)
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
        */
        public RainfallModel()
        {
            HourRainfall = 0.0;
            DayRainfall = 0.0;
            WeekRainfall = 0.0;
            TotalRainfall = 0.0;
            DateAndTime = DateTime.Now;
        }

        public RainfallModel(double hourRainfall, double dayRainfall, double weekRainfall, double totalRainfall, DateTime time)
        {
            HourRainfall = hourRainfall;
            DayRainfall = dayRainfall;
            WeekRainfall = weekRainfall;
            TotalRainfall = totalRainfall;
            DateAndTime = time;
        }
    }
}