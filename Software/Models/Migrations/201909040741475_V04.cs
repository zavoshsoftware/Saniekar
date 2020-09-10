namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V04 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Code", c => c.String(nullable: false));
            DropColumn("dbo.Products", "Body");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Body", c => c.String());
            AlterColumn("dbo.Products", "Code", c => c.Int(nullable: false));
        }
    }
}
