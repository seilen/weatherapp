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
                w => w.DateAndTime,
                new WeatherModel {
                    DateAndTime = DateTime.Now,
                    OutdoorTemperatureModel = new OutdoorTemperatureModel(),
                    IndoorTemperatureModel = new IndoorTemperatureModel(),
                    BarometerModel = new BarometerModel(),
                    WindModel = new WindModel(),
                    RainfallModel = new RainfallModel(),
                    RecentExtremesModel = new RecentExtremesModel()
                }
                );
            context.IndoorTemperatureModels.AddOrUpdate(
                i => i.DateAndTime,
                new IndoorTemperatureModel
                {
                    DateAndTime = DateTime.Now,
                    Temperature = 20.0,
                    Humidity = 10
                }
                );
            context.OutdoorTemperatureModels.AddOrUpdate(
                i => i.DateAndTime,
                new OutdoorTemperatureModel
                {
                    Temperature = 20.0,
                    TemperatureTrend = 10.0,
                    AvgTemperature = 15.0,
                    WindChill = 10.0,
                    HeatIndex = 20.0,
                    DewPoint = 10.0,
                    RelHumidity = 20.0,
                    ApparentTemperature = 10.0,
                    DateAndTime = DateTime.Now
                }
                );
            context.RainfallModels.AddOrUpdate(
                i => i.DateAndTime,
                new RainfallModel
                {
                    RainfallRate = 0.0,
                    RainfallLastHour = 0.0,
                    RainfallToday = 0.0,
                    RainfallLast24Hours = 0.0,
                    RainfallYesterday = 0.0,
                    RainfallThisMonth = 0.0,
                    RainfallThisYear = 0.0,
                    DateAndTime = DateTime.Now
                }
                );
            context.RecentExtremesModels.AddOrUpdate(
                i => i.DateAndTime,
                new RecentExtremesModel
                {
                    HighWind = 0.0,
                    HighGust = 0.0,
                    Bearing = 0.0,
                    MinTemp = 0.0,
                    MaxTemp = 0.0,
                    MinPressure = 0.0,
                    MaxPressure = 0.0,
                    RainRate = 0.0,
                    DateAndTime = DateTime.Now
                }
                );
            context.WindModels.AddOrUpdate(
                i => i.DateAndTime,
                new WindModel
                {
                    LatestWind = 0.0,
                    Bearing = 0.0,
                    Gust = 0.0,
                    AverageWind = 0.0,
                    AvgDirection = 0.0,
                    WindRun = 0.0,
                    DateAndTime = DateTime.Now,
                }
                );
        }
    }
}
