namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V35 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductRequestStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Code = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ProductRequests", "ProductRequestStatusId", c => c.Guid());
            CreateIndex("dbo.ProductRequests", "ProductRequestStatusId");
            AddForeignKey("dbo.ProductRequests", "ProductRequestStatusId", "dbo.ProductRequestStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductRequests", "ProductRequestStatusId", "dbo.ProductRequestStatus");
            DropIndex("dbo.ProductRequests", new[] { "ProductRequestStatusId" });
            DropColumn("dbo.ProductRequests", "ProductRequestStatusId");
            DropTable("dbo.ProductRequestStatus");
        }
    }
}
