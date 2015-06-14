using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Weatherapp.Models
{
    public class WeatherappContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public WeatherappContext() : base("name=WeatherappContext")
        {
        }

        public System.Data.Entity.DbSet<Weatherapp.Models.WeatherModel> WeatherModels { get; set; }

        public System.Data.Entity.DbSet<Weatherapp.Models.IndoorTemperatureModel> IndoorTemperatureModels { get; set; }

        public System.Data.Entity.DbSet<Weatherapp.Models.OutdoorTemperatureModel> OutdoorTemperatureModels { get; set; }

        public System.Data.Entity.DbSet<Weatherapp.Models.RainfallModel> RainfallModels { get; set; }

        public System.Data.Entity.DbSet<Weatherapp.Models.WindModel> WindModels { get; set; }

        public System.Data.Entity.DbSet<Weatherapp.Models.BarometerModel> BarometerModels { get; set; }
    }
}
