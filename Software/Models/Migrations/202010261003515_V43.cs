namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V43 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BranchUsers", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.BranchUsers", "UserId", "dbo.Users");
            DropIndex("dbo.BranchUsers", new[] { "UserId" });
            DropIndex("dbo.BranchUsers", new[] { "BranchId" });
            DropTable("dbo.BranchUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BranchUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        BranchId = c.Guid(nullable: false),
                        IsManager = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.BranchUsers", "BranchId");
            CreateIndex("dbo.BranchUsers", "UserId");
            AddForeignKey("dbo.BranchUsers", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BranchUsers", "BranchId", "dbo.Branches", "Id", cascadeDelete: true);
        }
    }
}
