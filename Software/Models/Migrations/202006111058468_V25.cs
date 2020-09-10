namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V25 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mattresses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Products", "HasMattress", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderDetails", "MattressId", c => c.Guid());
            CreateIndex("dbo.OrderDetails", "MattressId");
            AddForeignKey("dbo.OrderDetails", "MattressId", "dbo.Mattresses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "MattressId", "dbo.Mattresses");
            DropIndex("dbo.OrderDetails", new[] { "MattressId" });
            DropColumn("dbo.OrderDetails", "MattressId");
            DropColumn("dbo.Products", "HasMattress");
            DropTable("dbo.Mattresses");
        }
    }
}
