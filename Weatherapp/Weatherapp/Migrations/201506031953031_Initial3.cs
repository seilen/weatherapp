namespace Weatherapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WeatherModels", "Interval", c => c.Double(nullable: false));
            AddColumn("dbo.BarometerModels", "RelativePressure", c => c.Double(nullable: false));
            AddColumn("dbo.BarometerModels", "AbsolutePressure", c => c.Double(nullable: false));
            DropColumn("dbo.RainfallModels", "DateAndTime");
            DropColumn("dbo.BarometerModels", "Pressure");
            DropColumn("dbo.BarometerModels", "PressureTrend");
            DropColumn("dbo.WindModels", "DateAndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WindModels", "DateAndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.BarometerModels", "PressureTrend", c => c.Double(nullable: false));
            AddColumn("dbo.BarometerModels", "Pressure", c => c.Double(nullable: false));
            AddColumn("dbo.RainfallModels", "DateAndTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.BarometerModels", "AbsolutePressure");
            DropColumn("dbo.BarometerModels", "RelativePressure");
            DropColumn("dbo.WeatherModels", "Interval");
        }
    }
}
