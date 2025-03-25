using InventiCloud.Entities;
using InventiCloud.Utils;
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
    public DbSet<StockTransferItem> StockTransferDetails { get; set; }
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

        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = "your-user-id-1", // Generate a unique ID (GUID)
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = PasswordHasherUtility.HashPassword("YourSecurePassword123!"),
                SecurityStamp = Guid.NewGuid().ToString(), // Generate a unique SecurityStamp
            }
        );

        modelBuilder.Entity<Category>().HasData(
             new Category { CategoryId = 1, CategoryName = "Eyeglasses" },
             new Category { CategoryId = 2, CategoryName = "Contact Lenses" },
             new Category { CategoryId = 3, CategoryName = "Reading Glasses" },
             new Category { CategoryId = 4, CategoryName = "Eye Care Products" },
             new Category { CategoryId = 5, CategoryName = "Sunglasses" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = 1, // Use negative ProductId
                CategoryId = 1,
                ProductName = "Premium Blue Light Blocking Glasses",
                ImageURL = "glasses_blue_light.jpg",
                Brand = "VisionGuard",
                Description = "High-quality glasses to protect your eyes from harmful blue light.",
                UnitCost = 50.00m,
                UnitPrice = 120.00m,
                SKU = "VG-BL-001"
            },
            new Product
            {
                ProductId = 2, // Use negative ProductId
                CategoryId = 2,
                ProductName = "Daily Disposable Contact Lenses",
                ImageURL = "contact_lenses_daily.jpg",
                Brand = "AquaView",
                Description = "Comfortable daily disposable contact lenses for clear vision.",
                UnitCost = 15.00m,
                UnitPrice = 35.00m,
                SKU = "AV-CD-002"
            },
            new Product
            {
                ProductId = 3, // Use negative ProductId
                CategoryId = 3,
                ProductName = "Anti-Glare Reading Glasses",
                ImageURL = "reading_glasses_anti_glare.jpg",
                Brand = "ReadWell",
                Description = "Stylish reading glasses with anti-glare coating for reduced eye strain.",
                UnitCost = 25.00m,
                UnitPrice = 60.00m,
                SKU = "RW-RG-003"
            },
            new Product
            {
                ProductId = 4, // Use negative ProductId
                CategoryId = 4,
                ProductName = "Eye Drops for Dry Eyes",
                ImageURL = "eye_drops_dry_eyes.jpg",
                Brand = "MoisturePlus",
                Description = "Relief from dry, irritated eyes with these lubricating eye drops.",
                UnitCost = 8.00m,
                UnitPrice = 20.00m,
                SKU = "MP-ED-004"
            },
            new Product
            {
                ProductId = 5, // Use negative ProductId
                CategoryId = 1,
                ProductName = "Designer Sunglasses",
                ImageURL = "designer_sunglasses.jpg",
                Brand = "SunStyle",
                Description = "Fashionable sunglasses with UV protection for sunny days.",
                UnitCost = 80.00m,
                UnitPrice = 200.00m,
                SKU = "SS-SG-005"
            }
        );


        modelBuilder.Entity<Supplier>().HasData(
           new Supplier
           {
               SupplierCode = "SUP001",
               SupplierName = "Global Electronics",
               Company = "Global Tech Inc.",
               ContactPerson = "John Doe",
               Email = "john.doe@globaltech.com",
               PhoneNumber = "+15551234567",
               Country = "USA",
               Address = "123 Main St",
               PostalCode = "12345",
               City = "New York",
           }
         );

        // Seed Branch data
        modelBuilder.Entity<Branch>().HasData(
            new Branch
            {
                BranchId = 1,
                BranchName = "Branch A",
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
                BranchName = "Branch B",
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
                BranchName = "Branch C",
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
