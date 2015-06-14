namespace Weatherapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WeatherModels", "RecentExtremesModel_RecentExtremesModelId", "dbo.RecentExtremesModels");
            DropIndex("dbo.WeatherModels", new[] { "RecentExtremesModel_RecentExtremesModelId" });
            DropColumn("dbo.OutdoorTemperatureModels", "Temperature");
            DropColumn("dbo.OutdoorTemperatureModels", "TemperatureTrend");
            DropColumn("dbo.OutdoorTemperatureModels", "AvgTemperature");
            DropColumn("dbo.OutdoorTemperatureModels", "WindChill");
            DropColumn("dbo.OutdoorTemperatureModels", "HeatIndex");
            DropColumn("dbo.OutdoorTemperatureModels", "DewPoint");
            DropColumn("dbo.OutdoorTemperatureModels", "RelHumidity");
            DropColumn("dbo.OutdoorTemperatureModels", "ApparentTemperature");
            DropColumn("dbo.RainfallModels", "RainfallRate");
            DropColumn("dbo.RainfallModels", "RainfallLastHour");
            DropColumn("dbo.RainfallModels", "RainfallToday");
            DropColumn("dbo.RainfallModels", "RainfallLast24Hours");
            DropColumn("dbo.RainfallModels", "RainfallYesterday");
            DropColumn("dbo.RainfallModels", "RainfallThisMonth");
            DropColumn("dbo.RainfallModels", "RainfallThisYear");
            DropColumn("dbo.WeatherModels", "RecentExtremesModel_RecentExtremesModelId");
            DropColumn("dbo.WindModels", "LatestWind");
            DropColumn("dbo.WindModels", "Bearing");
            DropColumn("dbo.WindModels", "AverageWind");
            DropColumn("dbo.WindModels", "AvgDirection");
            DropColumn("dbo.WindModels", "WindRun");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WindModels", "WindRun", c => c.Double(nullable: false));
            AddColumn("dbo.WindModels", "AvgDirection", c => c.Double(nullable: false));
            AddColumn("dbo.WindModels", "AverageWind", c => c.Double(nullable: false));
            AddColumn("dbo.WindModels", "Bearing", c => c.Double(nullable: false));
            AddColumn("dbo.WindModels", "LatestWind", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "RecentExtremesModel_RecentExtremesModelId", c => c.Int());
            AddColumn("dbo.RainfallModels", "RainfallThisYear", c => c.Double(nullable: false));
            AddColumn("dbo.RainfallModels", "RainfallThisMonth", c => c.Double(nullable: false));
            AddColumn("dbo.RainfallModels", "RainfallYesterday", c => c.Double(nullable: false));
            AddColumn("dbo.RainfallModels", "RainfallLast24Hours", c => c.Double(nullable: false));
            AddColumn("dbo.RainfallModels", "RainfallToday", c => c.Double(nullable: false));
            AddColumn("dbo.RainfallModels", "RainfallLastHour", c => c.Double(nullable: false));
            AddColumn("dbo.RainfallModels", "RainfallRate", c => c.Double(nullable: false));
            AddColumn("dbo.OutdoorTemperatureModels", "ApparentTemperature", c => c.Double(nullable: false));
            AddColumn("dbo.OutdoorTemperatureModels", "RelHumidity", c => c.Double(nullable: false));
            AddColumn("dbo.OutdoorTemperatureModels", "DewPoint", c => c.Double(nullable: false));
            AddColumn("dbo.OutdoorTemperatureModels", "HeatIndex", c => c.Double(nullable: false));
            AddColumn("dbo.OutdoorTemperatureModels", "WindChill", c => c.Double(nullable: false));
            AddColumn("dbo.OutdoorTemperatureModels", "AvgTemperature", c => c.Double(nullable: false));
            AddColumn("dbo.OutdoorTemperatureModels", "TemperatureTrend", c => c.Double(nullable: false));
            AddColumn("dbo.OutdoorTemperatureModels", "Temperature", c => c.Double(nullable: false));
            CreateIndex("dbo.WeatherModels", "RecentExtremesModel_RecentExtremesModelId");
            AddForeignKey("dbo.WeatherModels", "RecentExtremesModel_RecentExtremesModelId", "dbo.RecentExtremesModels", "RecentExtremesModelId");
        }
    }
}
