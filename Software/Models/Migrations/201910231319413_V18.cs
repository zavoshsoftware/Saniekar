namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "FactoryAmount", c => c.Decimal(storeType: "money"));
            AddColumn("dbo.Products", "StoreAmount", c => c.Decimal(storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "StoreAmount");
            DropColumn("dbo.Products", "FactoryAmount");
        }
    }
}
