using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Weatherapp.Models
{
    public class WeatherModel
    {
        public int WeatherModelId { get; set; }
        public DateTime DateAndTime { get; set; }
        public double Interval {get;set;}

        public OutdoorTemperatureModel OutdoorTemperatureModel { get; set; }
        public IndoorTemperatureModel IndoorTemperatureModel { get; set; }
        public WindModel WindModel { get; set; }
        public BarometerModel BarometerModel { get; set; }
        public RainfallModel RainfallModel { get; set; }
        //public RecentExtremesModel RecentExtremesModel { get; set; }

        public string Self
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
               "api/weather/{0}", this.WeatherModelId);
            }
            set { }
        }

        public WeatherModel(DateTime dateAndTime, OutdoorTemperatureModel outdoorTempModel, IndoorTemperatureModel indoorTempModel, BarometerModel barometerModel, WindModel windModel, RainfallModel rainfallModel, RecentExtremesModel recentExtremesModel)
        {
            DateAndTime = dateAndTime;
            OutdoorTemperatureModel = outdoorTempModel;
            IndoorTemperatureModel = indoorTempModel;
            BarometerModel = barometerModel;
            WindModel = windModel;
            RainfallModel = rainfallModel;
        }

        public WeatherModel()
        {
            DateAndTime = DateTime.Now;
            OutdoorTemperatureModel = new OutdoorTemperatureModel();
            IndoorTemperatureModel = new IndoorTemperatureModel();
            BarometerModel = new BarometerModel();
            WindModel = new WindModel();
            RainfallModel = new RainfallModel();
            
        }

        public WeatherModel(RawWeatherData data)
        {
            WeatherModelFromRawData(data.No, data.Time, data.Interval, data.IndoorTemp, data.IndoorHumidity, data.OutdoorTemp, data.OutdoorHumidity, data.RelativePressure, data.AbsolutePressure, data.WindSpeed, data.Gust, data.WindDirection, data.DewPoint, data.WindChill, data.HourRainfall, data.DayRainfall, data.WeekRainfall, data.TotalRainfall);
        }

        public void WeatherModelFromRawData(int no, DateTime time, double interval, double indoorTemp, double indoorHumidity, double outdoorTemp, double outdoorHumidity, double relativePressure, double absolutePressure, double windSpeed, double gust, string windDirection, double dewPoint, double windChill, double hourRainfall, double dayRainfall, double weekRainfall, double totalRainfall)
        {
            WeatherModelId = no;
            DateAndTime = time;
            Interval = interval;
            IndoorTemperatureModel = new IndoorTemperatureModel(indoorTemp, indoorHumidity, time);
            OutdoorTemperatureModel = new OutdoorTemperatureModel(outdoorTemp, outdoorHumidity, time);
            BarometerModel = new BarometerModel(relativePressure, absolutePressure, time);
            WindModel = new WindModel(windSpeed, gust, windDirection, dewPoint, windChill, time);
            RainfallModel = new RainfallModel(hourRainfall, dayRainfall, weekRainfall, totalRainfall, time);
            
        }
    }
}