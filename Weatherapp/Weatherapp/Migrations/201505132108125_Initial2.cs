namespace Weatherapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OutdoorTemperatureModels",
                c => new
                    {
                        OutdoorTemperatureModelId = c.Int(nullable: false, identity: true),
                        Temperature = c.Double(nullable: false),
                        TemperatureTrend = c.Double(nullable: false),
                        AvgTemperature = c.Double(nullable: false),
                        WindChill = c.Double(nullable: false),
                        HeatIndex = c.Double(nullable: false),
                        DewPoint = c.Double(nullable: false),
                        RelHumidity = c.Double(nullable: false),
                        ApparentTemperature = c.Double(nullable: false),
                        DateAndTime = c.DateTime(nullable: false),
                        Self = c.String(),
                    })
                .PrimaryKey(t => t.OutdoorTemperatureModelId);
            
            CreateTable(
                "dbo.RainfallModels",
                c => new
                    {
                        RainfallModelId = c.Int(nullable: false, identity: true),
                        RainfallRate = c.Double(nullable: false),
                        RainfallLastHour = c.Double(nullable: false),
                        RainfallToday = c.Double(nullable: false),
                        RainfallLast24Hours = c.Double(nullable: false),
                        RainfallYesterday = c.Double(nullable: false),
                        RainfallThisMonth = c.Double(nullable: false),
                        RainfallThisYear = c.Double(nullable: false),
                        DateAndTime = c.DateTime(nullable: false),
                        Self = c.String(),
                    })
                .PrimaryKey(t => t.RainfallModelId);
            
            CreateTable(
                "dbo.RecentExtremesModels",
                c => new
                    {
                        RecentExtremesModelId = c.Int(nullable: false, identity: true),
                        HighWind = c.Double(nullable: false),
                        HighGust = c.Double(nullable: false),
                        Bearing = c.Double(nullable: false),
                        MinTemp = c.Double(nullable: false),
                        MaxTemp = c.Double(nullable: false),
                        MinPressure = c.Double(nullable: false),
                        MaxPressure = c.Double(nullable: false),
                        RainRate = c.Double(nullable: false),
                        DateAndTime = c.DateTime(nullable: false),
                        Self = c.String(),
                    })
                .PrimaryKey(t => t.RecentExtremesModelId);
            
            CreateTable(
                "dbo.BarometerModels",
                c => new
                    {
                        BarometerModelId = c.Int(nullable: false, identity: true),
                        Pressure = c.Double(nullable: false),
                        PressureTrend = c.Double(nullable: false),
                        DateAndTime = c.DateTime(nullable: false),
                        Self = c.String(),
                    })
                .PrimaryKey(t => t.BarometerModelId);
            
            CreateTable(
                "dbo.WindModels",
                c => new
                    {
                        WindModelId = c.Int(nullable: false, identity: true),
                        LatestWind = c.Double(nullable: false),
                        Bearing = c.Double(nullable: false),
                        Gust = c.Double(nullable: false),
                        AverageWind = c.Double(nullable: false),
                        AvgDirection = c.Double(nullable: false),
                        WindRun = c.Double(nullable: false),
                        DateAndTime = c.DateTime(nullable: false),
                        Self = c.String(),
                    })
                .PrimaryKey(t => t.WindModelId);
            
            AddColumn("dbo.WeatherModels", "BarometerModel_BarometerModelId", c => c.Int());
            AddColumn("dbo.WeatherModels", "OutdoorTemperatureModel_OutdoorTemperatureModelId", c => c.Int());
            AddColumn("dbo.WeatherModels", "RainfallModel_RainfallModelId", c => c.Int());
            AddColumn("dbo.WeatherModels", "RecentExtremesModel_RecentExtremesModelId", c => c.Int());
            AddColumn("dbo.WeatherModels", "WindModel_WindModelId", c => c.Int());
            CreateIndex("dbo.WeatherModels", "BarometerModel_BarometerModelId");
            CreateIndex("dbo.WeatherModels", "OutdoorTemperatureModel_OutdoorTemperatureModelId");
            CreateIndex("dbo.WeatherModels", "RainfallModel_RainfallModelId");
            CreateIndex("dbo.WeatherModels", "RecentExtremesModel_RecentExtremesModelId");
            CreateIndex("dbo.WeatherModels", "WindModel_WindModelId");
            AddForeignKey("dbo.WeatherModels", "BarometerModel_BarometerModelId", "dbo.BarometerModels", "BarometerModelId");
            AddForeignKey("dbo.WeatherModels", "OutdoorTemperatureModel_OutdoorTemperatureModelId", "dbo.OutdoorTemperatureModels", "OutdoorTemperatureModelId");
            AddForeignKey("dbo.WeatherModels", "RainfallModel_RainfallModelId", "dbo.RainfallModels", "RainfallModelId");
            AddForeignKey("dbo.WeatherModels", "RecentExtremesModel_RecentExtremesModelId", "dbo.RecentExtremesModels", "RecentExtremesModelId");
            AddForeignKey("dbo.WeatherModels", "WindModel_WindModelId", "dbo.WindModels", "WindModelId");
            DropColumn("dbo.WeatherModels", "OutdoorTemperatureModel_Temperature");
            DropColumn("dbo.WeatherModels", "OutdoorTemperatureModel_TemperatureTrend");
            DropColumn("dbo.WeatherModels", "OutdoorTemperatureModel_AvgTemperature");
            DropColumn("dbo.WeatherModels", "OutdoorTemperatureModel_WindChill");
            DropColumn("dbo.WeatherModels", "OutdoorTemperatureModel_HeatIndex");
            DropColumn("dbo.WeatherModels", "OutdoorTemperatureModel_DewPoint");
            DropColumn("dbo.WeatherModels", "OutdoorTemperatureModel_RelHumidity");
            DropColumn("dbo.WeatherModels", "OutdoorTemperatureModel_ApparentTemperature");
            DropColumn("dbo.WeatherModels", "WindModel_LatestWind");
            DropColumn("dbo.WeatherModels", "WindModel_Bearing");
            DropColumn("dbo.WeatherModels", "WindModel_Gust");
            DropColumn("dbo.WeatherModels", "WindModel_AverageWind");
            DropColumn("dbo.WeatherModels", "WindModel_AvgDirection");
            DropColumn("dbo.WeatherModels", "WindModel_WindRun");
            DropColumn("dbo.WeatherModels", "BarometerModel_Pressure");
            DropColumn("dbo.WeatherModels", "BarometerModel_PressureTrend");
            DropColumn("dbo.WeatherModels", "RainfallModel_RainfallRate");
            DropColumn("dbo.WeatherModels", "RainfallModel_RainfallLastHour");
            DropColumn("dbo.WeatherModels", "RainfallModel_RainfallToday");
            DropColumn("dbo.WeatherModels", "RainfallModel_RainfallLast24Hours");
            DropColumn("dbo.WeatherModels", "RainfallModel_RainfallYesterday");
            DropColumn("dbo.WeatherModels", "RainfallModel_RainfallThisMonth");
            DropColumn("dbo.WeatherModels", "RainfallModel_RainfallThisYear");
            DropColumn("dbo.WeatherModels", "RecentExtremesModel_HighWind");
            DropColumn("dbo.WeatherModels", "RecentExtremesModel_HighGust");
            DropColumn("dbo.WeatherModels", "RecentExtremesModel_Bearing");
            DropColumn("dbo.WeatherModels", "RecentExtremesModel_MinTemp");
            DropColumn("dbo.WeatherModels", "RecentExtremesModel_MaxTemp");
            DropColumn("dbo.WeatherModels", "RecentExtremesModel_MinPressure");
            DropColumn("dbo.WeatherModels", "RecentExtremesModel_MaxPressure");
            DropColumn("dbo.WeatherModels", "RecentExtremesModel_RainRate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WeatherModels", "RecentExtremesModel_RainRate", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RecentExtremesModel_MaxPressure", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RecentExtremesModel_MinPressure", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RecentExtremesModel_MaxTemp", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RecentExtremesModel_MinTemp", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RecentExtremesModel_Bearing", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RecentExtremesModel_HighGust", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RecentExtremesModel_HighWind", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RainfallModel_RainfallThisYear", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RainfallModel_RainfallThisMonth", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RainfallModel_RainfallYesterday", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RainfallModel_RainfallLast24Hours", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RainfallModel_RainfallToday", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RainfallModel_RainfallLastHour", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RainfallModel_RainfallRate", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "BarometerModel_PressureTrend", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "BarometerModel_Pressure", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "WindModel_WindRun", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "WindModel_AvgDirection", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "WindModel_AverageWind", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "WindModel_Gust", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "WindModel_Bearing", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "WindModel_LatestWind", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "OutdoorTemperatureModel_ApparentTemperature", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "OutdoorTemperatureModel_RelHumidity", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "OutdoorTemperatureModel_DewPoint", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "OutdoorTemperatureModel_HeatIndex", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "OutdoorTemperatureModel_WindChill", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "OutdoorTemperatureModel_AvgTemperature", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "OutdoorTemperatureModel_TemperatureTrend", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "OutdoorTemperatureModel_Temperature", c => c.Double(nullable: false));
            DropForeignKey("dbo.WeatherModels", "WindModel_WindModelId", "dbo.WindModels");
            DropForeignKey("dbo.WeatherModels", "RecentExtremesModel_RecentExtremesModelId", "dbo.RecentExtremesModels");
            DropForeignKey("dbo.WeatherModels", "RainfallModel_RainfallModelId", "dbo.RainfallModels");
            DropForeignKey("dbo.WeatherModels", "OutdoorTemperatureModel_OutdoorTemperatureModelId", "dbo.OutdoorTemperatureModels");
            DropForeignKey("dbo.WeatherModels", "BarometerModel_BarometerModelId", "dbo.BarometerModels");
            DropIndex("dbo.WeatherModels", new[] { "WindModel_WindModelId" });
            DropIndex("dbo.WeatherModels", new[] { "RecentExtremesModel_RecentExtremesModelId" });
            DropIndex("dbo.WeatherModels", new[] { "RainfallModel_RainfallModelId" });
            DropIndex("dbo.WeatherModels", new[] { "OutdoorTemperatureModel_OutdoorTemperatureModelId" });
            DropIndex("dbo.WeatherModels", new[] { "BarometerModel_BarometerModelId" });
            DropColumn("dbo.WeatherModels", "WindModel_WindModelId");
            DropColumn("dbo.WeatherModels", "RecentExtremesModel_RecentExtremesModelId");
            DropColumn("dbo.WeatherModels", "RainfallModel_RainfallModelId");
            DropColumn("dbo.WeatherModels", "OutdoorTemperatureModel_OutdoorTemperatureModelId");
            DropColumn("dbo.WeatherModels", "BarometerModel_BarometerModelId");
            DropTable("dbo.WindModels");
            DropTable("dbo.BarometerModels");
            DropTable("dbo.RecentExtremesModels");
            DropTable("dbo.RainfallModels");
            DropTable("dbo.OutdoorTemperatureModels");
        }
    }
}
