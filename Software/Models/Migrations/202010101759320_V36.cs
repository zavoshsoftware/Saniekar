namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V36 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Attachment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Attachment");
        }
    }
}
