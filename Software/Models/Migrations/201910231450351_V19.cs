namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductGroups", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductGroups", "Code");
        }
    }
}
