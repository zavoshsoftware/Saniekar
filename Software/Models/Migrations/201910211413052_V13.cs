namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderStatus", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.OrderStatus", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderStatus", "Title", c => c.String());
            DropColumn("dbo.OrderStatus", "Name");
        }
    }
}
