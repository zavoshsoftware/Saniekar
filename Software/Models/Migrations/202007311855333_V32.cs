namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V32 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductRequestDetailSuppliers", "IsReceived", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductRequestDetailSuppliers", "IsReceived");
        }
    }
}
