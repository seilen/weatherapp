using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Weatherapp.Models
{
    public class BarometerModel
    {
        //Barometer - Steady
        public double RelativePressure { get; set; }
        public double AbsolutePressure { get; set; }

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

        public BarometerModel(double relativePressure, double absolutePressure, DateTime dateAndTime)
        {
            RelativePressure = relativePressure;
            AbsolutePressure = absolutePressure;
            DateAndTime = dateAndTime;
        }

        public BarometerModel()
        {
            RelativePressure = 0.0;
            AbsolutePressure = 0.0;
            DateAndTime = DateTime.Now;
        }
    }
}