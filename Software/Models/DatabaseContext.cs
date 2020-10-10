using System.Data.Entity;
namespace Models
{
   public class DatabaseContext:DbContext
    {
        static DatabaseContext()
        {
           System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<BranchUser> BranchUsers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<ProductRequest> ProductRequests { get; set; }
        public DbSet<ProductRequestDetail> ProductRequestDetails { get; set; }
        public DbSet<BranchProduct> BranchProducts { get; set; }
        public DbSet<InputDocument> InputDocuments { get; set; }
        public DbSet<InputDocumentDetail> InputDocumentDetails { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }

        public System.Data.Entity.DbSet<Models.Inventory> Inventories { get; set; }
        public System.Data.Entity.DbSet<Models.ProductRequestDetailSupplier> ProductRequestDetailSuppliers { get; set; }

        public System.Data.Entity.DbSet<Models.ShipmentType> ShipmentTypes { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<Mattress> Mattresses { get; set; }
        public DbSet<Fund> Funds { get; set; }
        public DbSet<FundDetail> FundDetails { get; set; }
        public DbSet<ProductRequestStatus> ProductRequestStatuses { get; set; }
    }
}
