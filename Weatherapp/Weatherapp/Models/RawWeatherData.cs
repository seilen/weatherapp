using System;

namespace Weatherapp.Models
{
    public class RawWeatherData
    {
        public int No { get; set; }
        public DateTime Time { get; set; }
        public double Interval { get; set; }
        public double IndoorTemp { get; set; }
        public double IndoorHumidity { get; set; }
        public double OutdoorTemp { get; set; }
        public double OutdoorHumidity { get; set; }
        public double RelativePressure { get; set; }
        public double AbsolutePressure { get; set; }
        public double WindSpeed { get; set; }
        public double Gust { get; set; }
        public string WindDirection { get; set; }
        public double DewPoint { get; set; }
        public double WindChill { get; set; }
        public double HourRainfall { get; set; }
        public double DayRainfall { get; set; }
        public double WeekRainfall { get; set; }
        public double TotalRainfall { get; set; }


    }
}