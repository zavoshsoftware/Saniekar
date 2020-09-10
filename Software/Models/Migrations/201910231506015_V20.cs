namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V20 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Code", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Code", c => c.String(nullable: false));
        }
    }
}
