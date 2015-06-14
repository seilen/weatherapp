using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Weatherapp.Models
{
    public class OutdoorTemperatureModel
    {
        public double OutdoorTemp;
        public double OutdoorHumidity;

        //Outdoor
        /*public double Temperature { get; set; }
        public double TemperatureTrend { get; set; }
        public double AvgTemperature { get; set; }
        public double WindChill { get; set; }
        public double HeatIndex { get; set; }
        public double DewPoint { get; set; }
        public double RelHumidity { get; set; }
        public double ApparentTemperature { get; set; }
        */
        public int OutdoorTemperatureModelId { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Self
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
               "api/OutdoorTemperatureModels/{0}", this.OutdoorTemperatureModelId);
            }
            set { }
        }

        /*public OutdoorTemperatureModel(double temp, double tempTrend, double avgTemp, double windChill, double heatIndex, double dewPoint, double relHumidity, double apparentTemperature, DateTime dateAndTime)
        {
            Temperature = temp;
            TemperatureTrend = tempTrend;
            AvgTemperature = avgTemp;
            WindChill = windChill;
            HeatIndex = heatIndex;
            DewPoint = dewPoint;
            RelHumidity = relHumidity;
            ApparentTemperature = apparentTemperature;

            DateAndTime = dateAndTime;
        }*/

        public OutdoorTemperatureModel()
        {
            OutdoorTemp = 0.0;
            OutdoorHumidity = 0.0;
            DateAndTime = DateTime.Now;
        }

        public OutdoorTemperatureModel(double outdoorTemp, double outdoorHumidity, DateTime dateAndTime)
        {
            OutdoorTemp = outdoorTemp;
            OutdoorHumidity = outdoorHumidity;
            DateAndTime = dateAndTime;
        }
    }
}