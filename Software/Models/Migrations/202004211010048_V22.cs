namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ShipmentFromFactory", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "FactoryShipmentDesc", c => c.String());
            DropColumn("dbo.Orders", "SendFromFactory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "SendFromFactory", c => c.Boolean(nullable: false));
            DropColumn("dbo.Orders", "FactoryShipmentDesc");
            DropColumn("dbo.Orders", "ShipmentFromFactory");
        }
    }
}
