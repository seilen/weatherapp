using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;


namespace Weatherapp.Models
{
    public class IndoorTemperatureModel
    {
        public int IndoorTemperatureModelId { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Self
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
               "api/IndoorTemperatureModels/{0}", this.IndoorTemperatureModelId);
            }
            set { }
        }

        public IndoorTemperatureModel(double temperature, double humidity, DateTime dateAndTime)
        {
            Temperature = temperature;
            Humidity = humidity;
            DateAndTime = dateAndTime;
        }

        public IndoorTemperatureModel()
        {
            Temperature = 0.0;
            Humidity = 0.0;
            DateAndTime = DateTime.Now;
        }
    }
}