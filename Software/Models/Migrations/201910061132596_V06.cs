namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V06 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductRequests", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProductRequestDetails", "ProductRequestId", c => c.Guid(nullable: false));
            CreateIndex("dbo.ProductRequestDetails", "ProductRequestId");
            AddForeignKey("dbo.ProductRequestDetails", "ProductRequestId", "dbo.ProductRequests", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductRequestDetails", "ProductRequestId", "dbo.ProductRequests");
            DropIndex("dbo.ProductRequestDetails", new[] { "ProductRequestId" });
            DropColumn("dbo.ProductRequestDetails", "ProductRequestId");
            DropColumn("dbo.ProductRequests", "Total");
        }
    }
}
