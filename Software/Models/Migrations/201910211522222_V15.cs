namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "SubAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Orders", "Amount");
            DropColumn("dbo.Orders", "FinalPaymentDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "FinalPaymentDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Orders", "SubAmount");
        }
    }
}
