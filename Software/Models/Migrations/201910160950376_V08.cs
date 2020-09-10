namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V08 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductRequestDetailSuppliers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProductRequestDetailId = c.Guid(nullable: false),
                        BranchId = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("dbo.ProductRequestDetails", t => t.ProductRequestDetailId, cascadeDelete: true)
                .Index(t => t.ProductRequestDetailId)
                .Index(t => t.BranchId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductRequestDetailSuppliers", "ProductRequestDetailId", "dbo.ProductRequestDetails");
            DropForeignKey("dbo.ProductRequestDetailSuppliers", "BranchId", "dbo.Branches");
            DropIndex("dbo.ProductRequestDetailSuppliers", new[] { "BranchId" });
            DropIndex("dbo.ProductRequestDetailSuppliers", new[] { "ProductRequestDetailId" });
            DropTable("dbo.ProductRequestDetailSuppliers");
        }
    }
}
