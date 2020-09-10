namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V23 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "ParentId", "dbo.Products");
            DropIndex("dbo.Products", new[] { "ParentId" });
            CreateTable(
                "dbo.ProductColors",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            AddColumn("dbo.OrderDetails", "ProductColorId", c => c.Guid());
            CreateIndex("dbo.OrderDetails", "ProductColorId");
            AddForeignKey("dbo.OrderDetails", "ProductColorId", "dbo.ProductColors", "Id");
            DropColumn("dbo.Products", "ParentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ParentId", c => c.Guid());
            DropForeignKey("dbo.ProductColors", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "ProductColorId", "dbo.ProductColors");
            DropIndex("dbo.ProductColors", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductColorId" });
            DropColumn("dbo.OrderDetails", "ProductColorId");
            DropTable("dbo.ProductColors");
            CreateIndex("dbo.Products", "ParentId");
            AddForeignKey("dbo.Products", "ParentId", "dbo.Products", "Id");
        }
    }
}
