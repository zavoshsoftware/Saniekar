namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V29 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inventories", "ProductColorId", c => c.Guid());
            AddColumn("dbo.Inventories", "MattressId", c => c.Guid());
            CreateIndex("dbo.Inventories", "ProductColorId");
            CreateIndex("dbo.Inventories", "MattressId");
            AddForeignKey("dbo.Inventories", "MattressId", "dbo.Mattresses", "Id");
            AddForeignKey("dbo.Inventories", "ProductColorId", "dbo.ProductColors", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inventories", "ProductColorId", "dbo.ProductColors");
            DropForeignKey("dbo.Inventories", "MattressId", "dbo.Mattresses");
            DropIndex("dbo.Inventories", new[] { "MattressId" });
            DropIndex("dbo.Inventories", new[] { "ProductColorId" });
            DropColumn("dbo.Inventories", "MattressId");
            DropColumn("dbo.Inventories", "ProductColorId");
        }
    }
}
