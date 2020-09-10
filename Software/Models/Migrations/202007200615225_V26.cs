namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V26 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "FileAttched", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "FileAttched");
        }
    }
}
