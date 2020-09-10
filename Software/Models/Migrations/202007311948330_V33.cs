namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V33 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "RefrenceId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "RefrenceId");
        }
    }
}
