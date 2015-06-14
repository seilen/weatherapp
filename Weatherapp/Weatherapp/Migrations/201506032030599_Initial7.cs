namespace Weatherapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial7 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.RecentExtremesModels");
        }
        
        public override void Down()
        {
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
            
        }
    }
}
