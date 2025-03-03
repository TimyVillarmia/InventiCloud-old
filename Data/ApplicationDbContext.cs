using InventiCloud.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<AttributeSet> AttributeSets { get; set; }
    public DbSet<Entities.Attribute> Attributes { get; set; }
    public DbSet<AttributeValue> AttributeValues { get; set; }
    public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<BranchAccount> BranchAccounts { get; set; }
    public DbSet<Inventory> Inventories { get; set; }


    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    public DbSet<PurchaseOrderStatus> PurchaseOrderStatuses { get; set; }
    public DbSet<StockAdjustment> StockAdjustments { get; set; }
    public DbSet<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
    public DbSet<StockAdjustmentReason> StockAdjustmentReasons { get; set; }
    public DbSet<StockAdjustmentStatus> StockAdjustmentStatuses { get; set; }
    public DbSet<StockTransfer> StockTransfers { get; set; }
    public DbSet<StockTransferDetail> StockTransferDetails { get; set; }
    public DbSet<StockTransferStatus> StockTransferStatuses { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed PurchaseOrderStatus data
        modelBuilder.Entity<PurchaseOrderStatus>().HasData(
            new PurchaseOrderStatus { PurchaseOrderStatusId = 1, StatusName = "Draft" },
            new PurchaseOrderStatus { PurchaseOrderStatusId = 2, StatusName = "Ordered" },
            new PurchaseOrderStatus { PurchaseOrderStatusId = 3, StatusName = "Completed" },
            new PurchaseOrderStatus { PurchaseOrderStatusId = 4, StatusName = "Cancelled" }
        );

        // Seed Branch data
        modelBuilder.Entity<Branch>().HasData(
            new Branch
            {
                BranchId = 1,
                BranchName = "Main Warehouse",
                Country = "USA",
                Address = "123 Main St",
                PostalCode = "12345",
                City = "Anytown",
                Region = "State",
                PhoneNumber = "555-123-4567",
                Email = "warehouse@example.com"
            },
            new Branch
            {
                BranchId = 2,
                BranchName = "Retail Store A",
                Country = "Canada",
                Address = "456 Oak Ave",
                PostalCode = "A1B 2C3",
                City = "Springfield",
                Region = "Province",
                PhoneNumber = "123-456-7890",
                Email = "retailA@example.com"
            },
            new Branch
            {
                BranchId = 3,
                BranchName = "Distribution Center",
                Country = "UK",
                Address = "789 Pine Ln",
                PostalCode = "SW1A 1AA",
                City = "London",
                Region = "England",
                PhoneNumber = "+44 20 1234 5678",
                Email = "distribution@example.com"
            }
        );
    }
}
