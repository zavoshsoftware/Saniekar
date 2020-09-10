namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        ShipmentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CityId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .Index(t => t.CityId);
            
            AddColumn("dbo.Orders", "RegionId", c => c.Guid());
            CreateIndex("dbo.Orders", "RegionId");
            AddForeignKey("dbo.Orders", "RegionId", "dbo.Regions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "RegionId", "dbo.Regions");
            DropForeignKey("dbo.Regions", "CityId", "dbo.Cities");
            DropIndex("dbo.Regions", new[] { "CityId" });
            DropIndex("dbo.Orders", new[] { "RegionId" });
            DropColumn("dbo.Orders", "RegionId");
            DropTable("dbo.Regions");
        }
    }
}
