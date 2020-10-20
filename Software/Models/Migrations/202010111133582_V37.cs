namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V37 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsEdit", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "CheckByStore", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "CheckByFactory", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "CheckByFactory");
            DropColumn("dbo.Orders", "CheckByStore");
            DropColumn("dbo.Orders", "IsEdit");
        }
    }
}
