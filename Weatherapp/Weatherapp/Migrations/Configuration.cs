namespace Weatherapp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Weatherapp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Weatherapp.Models.WeatherappContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Weatherapp.Models.WeatherappContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.WeatherModels.AddOrUpdate(
                w => w.WeatherModelId,
                new WeatherModel {
                    DateAndTime = DateTime.Now,
                    Interval = 30,
                    OutdoorTemperatureModel = new OutdoorTemperatureModel(),
                    IndoorTemperatureModel = new IndoorTemperatureModel(),
                    BarometerModel = new BarometerModel(),
                    WindModel = new WindModel(),
                    RainfallModel = new RainfallModel(),
                }
                );
            context.IndoorTemperatureModels.AddOrUpdate(
                i => i.IndoorTemperatureModelId,
                new IndoorTemperatureModel
                {
                    DateAndTime = DateTime.Now,
                    Temperature = 20.0,
                    Humidity = 10
                }
                );
            context.OutdoorTemperatureModels.AddOrUpdate(
                i => i.OutdoorTemperatureModelId,
                new OutdoorTemperatureModel
                {
                    OutdoorTemp = 20.0,
                    OutdoorHumidity = 50
                }
                );
            context.RainfallModels.AddOrUpdate(
                i => i.RainfallModelId,
                new RainfallModel
                {
                    HourRainfall = 0.0,
                    DayRainfall = 0.0,
                    WeekRainfall = 0.0,
                    TotalRainfall = 0.0,
                    DateAndTime = DateTime.Now
                }
                );
            
            context.WindModels.AddOrUpdate(
                i => i.WindModelId,
                new WindModel
                {
                    WindSpeed = 2.0,
                    Gust = 3.0,
                    WindDirection = "N",
                    DewPoint = 10.0,
                    WindChill = 3.0,
                    DateAndTime = DateTime.Now,
                }
                );
        }
    }
}
