using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Weatherapp.Models
{
    public class BarometerModel
    {
        //Barometer - Steady
        public double Pressure { get; set; }
        public double PressureTrend { get; set; }

        public int BarometerModelId { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Self
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
               "api/BarometerModels/{0}", this.BarometerModelId);
            }
            set { }
        }

        public BarometerModel(double pressure, double pressureTrend, DateTime dateAndTime)
        {
            Pressure = pressure;
            PressureTrend = pressureTrend;
            DateAndTime = dateAndTime;
        }

        public BarometerModel()
        {
            Pressure = 0.0;
            PressureTrend = 0.0;
            DateAndTime = DateTime.Now;
        }
    }
}