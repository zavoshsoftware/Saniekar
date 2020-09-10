namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShipmentTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "ShipmentTypeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Orders", "ShipmentTypeId");
            AddForeignKey("dbo.Orders", "ShipmentTypeId", "dbo.ShipmentTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ShipmentTypeId", "dbo.ShipmentTypes");
            DropIndex("dbo.Orders", new[] { "ShipmentTypeId" });
            DropColumn("dbo.Orders", "ShipmentTypeId");
            DropTable("dbo.ShipmentTypes");
        }
    }
}
