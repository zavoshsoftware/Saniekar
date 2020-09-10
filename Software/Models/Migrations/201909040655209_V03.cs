namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V03 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InputDocuments", "Code", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InputDocuments", "Code", c => c.Int(nullable: false));
        }
    }
}
