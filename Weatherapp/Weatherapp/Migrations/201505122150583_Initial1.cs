namespace Weatherapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IndoorTemperatureModels",
                c => new
                    {
                        IndoorTemperatureModelId = c.Int(nullable: false, identity: true),
                        Temperature = c.Double(nullable: false),
                        Humidity = c.Double(nullable: false),
                        DateAndTime = c.DateTime(nullable: false),
                        Self = c.String(),
                    })
                .PrimaryKey(t => t.IndoorTemperatureModelId);
            
            AddColumn("dbo.WeatherModels", "IndoorTemperatureModel_IndoorTemperatureModelId", c => c.Int());
            CreateIndex("dbo.WeatherModels", "IndoorTemperatureModel_IndoorTemperatureModelId");
            AddForeignKey("dbo.WeatherModels", "IndoorTemperatureModel_IndoorTemperatureModelId", "dbo.IndoorTemperatureModels", "IndoorTemperatureModelId");
            DropColumn("dbo.WeatherModels", "IndoorTemperatureModel_Temperature");
            DropColumn("dbo.WeatherModels", "IndoorTemperatureModel_Humidity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WeatherModels", "IndoorTemperatureModel_Humidity", c => c.Double(nullable: false));
            AddColumn("dbo.WeatherModels", "IndoorTemperatureModel_Temperature", c => c.Double(nullable: false));
            DropForeignKey("dbo.WeatherModels", "IndoorTemperatureModel_IndoorTemperatureModelId", "dbo.IndoorTemperatureModels");
            DropIndex("dbo.WeatherModels", new[] { "IndoorTemperatureModel_IndoorTemperatureModelId" });
            DropColumn("dbo.WeatherModels", "IndoorTemperatureModel_IndoorTemperatureModelId");
            DropTable("dbo.IndoorTemperatureModels");
        }
    }
}
