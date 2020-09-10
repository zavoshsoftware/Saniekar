namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V05 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InputDocuments", "SubTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.InputDocuments", "AddedAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.InputDocuments", "DecreaseAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.InputDocuments", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InputDocuments", "Total");
            DropColumn("dbo.InputDocuments", "DecreaseAmount");
            DropColumn("dbo.InputDocuments", "AddedAmount");
            DropColumn("dbo.InputDocuments", "SubTotal");
        }
    }
}
