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

        public OutdoorTemperatureModel OutdoorTemperatureModel { get; set; }
        public IndoorTemperatureModel IndoorTemperatureModel { get; set; }
        public WindModel WindModel { get; set; }
        public BarometerModel BarometerModel { get; set; }
        public RainfallModel RainfallModel { get; set; }
        public RecentExtremesModel RecentExtremesModel { get; set; }

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
            RecentExtremesModel = recentExtremesModel;
        }

        public WeatherModel()
        {
            DateAndTime = DateTime.Now;
            OutdoorTemperatureModel = new OutdoorTemperatureModel();
            IndoorTemperatureModel = new IndoorTemperatureModel();
            BarometerModel = new BarometerModel();
            WindModel = new WindModel();
            RainfallModel = new RainfallModel();
            RecentExtremesModel = new RecentExtremesModel();
        }
    }
}