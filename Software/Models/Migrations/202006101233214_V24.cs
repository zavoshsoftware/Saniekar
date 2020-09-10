namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V24 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductColors", "Amount", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductColors", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
