namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderStatus", "Code", c => c.Int(nullable: false));
            DropColumn("dbo.OrderStatus", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderStatus", "Name", c => c.String(nullable: false));
            DropColumn("dbo.OrderStatus", "Code");
        }
    }
}
