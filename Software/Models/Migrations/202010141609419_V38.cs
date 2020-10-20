namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V38 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductColors", "StoreAmount", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.ProductColors", "FactoryAmount", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductColors", "FactoryAmount");
            DropColumn("dbo.ProductColors", "StoreAmount");
        }
    }
}
