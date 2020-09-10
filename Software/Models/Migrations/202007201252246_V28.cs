namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V28 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InputDocumentDetails", "ProductColorId", c => c.Guid());
            AddColumn("dbo.InputDocumentDetails", "MattressId", c => c.Guid());
            CreateIndex("dbo.InputDocumentDetails", "ProductColorId");
            CreateIndex("dbo.InputDocumentDetails", "MattressId");
            AddForeignKey("dbo.InputDocumentDetails", "MattressId", "dbo.Mattresses", "Id");
            AddForeignKey("dbo.InputDocumentDetails", "ProductColorId", "dbo.ProductColors", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InputDocumentDetails", "ProductColorId", "dbo.ProductColors");
            DropForeignKey("dbo.InputDocumentDetails", "MattressId", "dbo.Mattresses");
            DropIndex("dbo.InputDocumentDetails", new[] { "MattressId" });
            DropIndex("dbo.InputDocumentDetails", new[] { "ProductColorId" });
            DropColumn("dbo.InputDocumentDetails", "MattressId");
            DropColumn("dbo.InputDocumentDetails", "ProductColorId");
        }
    }
}
