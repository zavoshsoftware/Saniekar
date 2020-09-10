namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V17 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "Phone", c => c.Guid(nullable: false));
        }
    }
}
