namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V27 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductRequestDetails", "ProductColorId", c => c.Guid());
            AddColumn("dbo.ProductRequestDetails", "MattressId", c => c.Guid());
            CreateIndex("dbo.ProductRequestDetails", "ProductColorId");
            CreateIndex("dbo.ProductRequestDetails", "MattressId");
            AddForeignKey("dbo.ProductRequestDetails", "ProductColorId", "dbo.ProductColors", "Id");
            AddForeignKey("dbo.ProductRequestDetails", "MattressId", "dbo.Mattresses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductRequestDetails", "MattressId", "dbo.Mattresses");
            DropForeignKey("dbo.ProductRequestDetails", "ProductColorId", "dbo.ProductColors");
            DropIndex("dbo.ProductRequestDetails", new[] { "MattressId" });
            DropIndex("dbo.ProductRequestDetails", new[] { "ProductColorId" });
            DropColumn("dbo.ProductRequestDetails", "MattressId");
            DropColumn("dbo.ProductRequestDetails", "ProductColorId");
        }
    }
}
