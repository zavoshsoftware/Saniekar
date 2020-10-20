namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V39 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InventoryDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InventoryId = c.Guid(nullable: false),
                        InventoryDetailTypeId = c.Guid(nullable: false),
                        Title = c.String(),
                        Quantity = c.Int(nullable: false),
                        Remail = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inventories", t => t.InventoryId, cascadeDelete: true)
                .ForeignKey("dbo.InventoryDetailTypes", t => t.InventoryDetailTypeId, cascadeDelete: true)
                .Index(t => t.InventoryId)
                .Index(t => t.InventoryDetailTypeId);
            
            CreateTable(
                "dbo.InventoryDetailTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InventoryDetails", "InventoryDetailTypeId", "dbo.InventoryDetailTypes");
            DropForeignKey("dbo.InventoryDetails", "InventoryId", "dbo.Inventories");
            DropIndex("dbo.InventoryDetails", new[] { "InventoryDetailTypeId" });
            DropIndex("dbo.InventoryDetails", new[] { "InventoryId" });
            DropTable("dbo.InventoryDetailTypes");
            DropTable("dbo.InventoryDetails");
        }
    }
}
