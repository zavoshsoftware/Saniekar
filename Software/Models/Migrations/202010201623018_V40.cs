namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V40 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InventoryDetails", "EntityId", c => c.Guid());
            AlterColumn("dbo.InventoryDetailTypes", "Title", c => c.String(maxLength: 50));
            AlterColumn("dbo.InventoryDetailTypes", "Name", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InventoryDetailTypes", "Name", c => c.String());
            AlterColumn("dbo.InventoryDetailTypes", "Title", c => c.String());
            DropColumn("dbo.InventoryDetails", "EntityId");
        }
    }
}
