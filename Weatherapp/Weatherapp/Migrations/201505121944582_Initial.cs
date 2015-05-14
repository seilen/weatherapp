namespace Weatherapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WeatherModels",
                c => new
                    {
                        WeatherModelId = c.Int(nullable: false, identity: true),
                        DateAndTime = c.DateTime(nullable: false),
                        OutdoorTemperatureModel_Temperature = c.Double(nullable: false),
                        OutdoorTemperatureModel_TemperatureTrend = c.Double(nullable: false),
                        OutdoorTemperatureModel_AvgTemperature = c.Double(nullable: false),
                        OutdoorTemperatureModel_WindChill = c.Double(nullable: false),
                        OutdoorTemperatureModel_HeatIndex = c.Double(nullable: false),
                        OutdoorTemperatureModel_DewPoint = c.Double(nullable: false),
                        OutdoorTemperatureModel_RelHumidity = c.Double(nullable: false),
                        OutdoorTemperatureModel_ApparentTemperature = c.Double(nullable: false),
                        IndoorTemperatureModel_Temperature = c.Double(nullable: false),
                        IndoorTemperatureModel_Humidity = c.Double(nullable: false),
                        WindModel_LatestWind = c.Double(nullable: false),
                        WindModel_Bearing = c.Double(nullable: false),
                        WindModel_Gust = c.Double(nullable: false),
                        WindModel_AverageWind = c.Double(nullable: false),
                        WindModel_AvgDirection = c.Double(nullable: false),
                        WindModel_WindRun = c.Double(nullable: false),
                        BarometerModel_Pressure = c.Double(nullable: false),
                        BarometerModel_PressureTrend = c.Double(nullable: false),
                        RainfallModel_RainfallRate = c.Double(nullable: false),
                        RainfallModel_RainfallLastHour = c.Double(nullable: false),
                        RainfallModel_RainfallToday = c.Double(nullable: false),
                        RainfallModel_RainfallLast24Hours = c.Double(nullable: false),
                        RainfallModel_RainfallYesterday = c.Double(nullable: false),
                        RainfallModel_RainfallThisMonth = c.Double(nullable: false),
                        RainfallModel_RainfallThisYear = c.Double(nullable: false),
                        RecentExtremesModel_HighWind = c.Double(nullable: false),
                        RecentExtremesModel_HighGust = c.Double(nullable: false),
                        RecentExtremesModel_Bearing = c.Double(nullable: false),
                        RecentExtremesModel_MinTemp = c.Double(nullable: false),
                        RecentExtremesModel_MaxTemp = c.Double(nullable: false),
                        RecentExtremesModel_MinPressure = c.Double(nullable: false),
                        RecentExtremesModel_MaxPressure = c.Double(nullable: false),
                        RecentExtremesModel_RainRate = c.Double(nullable: false),
                        Self = c.String(),
                    })
                .PrimaryKey(t => t.WeatherModelId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WeatherModels");
        }
    }
}
