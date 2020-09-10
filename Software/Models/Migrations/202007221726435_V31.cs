namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V31 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Funds", "FinishDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Funds", "FinishDate", c => c.DateTime(nullable: false));
        }
    }
}
