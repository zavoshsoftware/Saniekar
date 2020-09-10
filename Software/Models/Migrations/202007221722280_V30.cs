namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V30 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Funds",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BranchId = c.Guid(nullable: false),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        ReceiveDate = c.DateTime(nullable: false),
                        FinishDate = c.DateTime(nullable: false),
                        RemainAmount = c.Decimal(nullable: false, storeType: "money"),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                        Fund_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("dbo.Funds", t => t.Fund_Id)
                .Index(t => t.BranchId)
                .Index(t => t.Fund_Id);
            
            CreateTable(
                "dbo.FundDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FundId = c.Guid(nullable: false),
                        Title = c.String(),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        PaymentDate = c.DateTime(nullable: false),
                        RemainAmount = c.Decimal(nullable: false, storeType: "money"),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Funds", t => t.FundId, cascadeDelete: true)
                .Index(t => t.FundId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FundDetails", "FundId", "dbo.Funds");
            DropForeignKey("dbo.Funds", "Fund_Id", "dbo.Funds");
            DropForeignKey("dbo.Funds", "BranchId", "dbo.Branches");
            DropIndex("dbo.FundDetails", new[] { "FundId" });
            DropIndex("dbo.Funds", new[] { "Fund_Id" });
            DropIndex("dbo.Funds", new[] { "BranchId" });
            DropTable("dbo.FundDetails");
            DropTable("dbo.Funds");
        }
    }
}
