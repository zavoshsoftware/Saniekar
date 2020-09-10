namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Phone", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Phone");
        }
    }
}
