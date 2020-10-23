namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InventoryDetails", "Remain", c => c.Int(nullable: false));
            DropColumn("dbo.InventoryDetails", "Remail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InventoryDetails", "Remail", c => c.Int(nullable: false));
            DropColumn("dbo.InventoryDetails", "Remain");
        }
    }
}
