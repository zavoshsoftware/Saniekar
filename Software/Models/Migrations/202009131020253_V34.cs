namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V34 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "ShipmentTypeId", "dbo.ShipmentTypes");
            DropIndex("dbo.Orders", new[] { "ShipmentTypeId" });
            AlterColumn("dbo.Orders", "ShipmentTypeId", c => c.Guid());
            CreateIndex("dbo.Orders", "ShipmentTypeId");
            AddForeignKey("dbo.Orders", "ShipmentTypeId", "dbo.ShipmentTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ShipmentTypeId", "dbo.ShipmentTypes");
            DropIndex("dbo.Orders", new[] { "ShipmentTypeId" });
            AlterColumn("dbo.Orders", "ShipmentTypeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Orders", "ShipmentTypeId");
            AddForeignKey("dbo.Orders", "ShipmentTypeId", "dbo.ShipmentTypes", "Id", cascadeDelete: true);
        }
    }
}
