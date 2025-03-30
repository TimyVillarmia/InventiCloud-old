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
    public DbSet<StockTransferItem> StockTransferItems { get; set; }
    public DbSet<StockTransferStatus> StockTransferStatuses { get; set; }
    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<SalesPerson> SalesPersons { get; set; }

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

        // Seed StockTransferStatus data
        modelBuilder.Entity<StockTransferStatus>().HasData(
            new StockTransferStatus { StockTransferStatusId = 1, StatusName = "Allocated" },
            new StockTransferStatus { StockTransferStatusId = 2, StatusName = "In Transit" },
            new StockTransferStatus { StockTransferStatusId = 3, StatusName = "Cancelled" },
            new StockTransferStatus { StockTransferStatusId = 4, StatusName = "Completed" }
        );

        // Seed StockAdjustmentStatus data
        modelBuilder.Entity<StockAdjustmentStatus>().HasData(
            new StockAdjustmentStatus { StockAdjustmentStatusId = 1, StatusName = "Draft" },
            new StockAdjustmentStatus { StockAdjustmentStatusId = 2, StatusName = "Completed" }
        );

        // Seed StockAdjustmentReason data
        modelBuilder.Entity<StockAdjustmentReason>().HasData(
            new StockAdjustmentReason { StockAdjustmentReasonId = 1, Reason = "Damaged/Defective" },
            new StockAdjustmentReason { StockAdjustmentReasonId = 2, Reason = "Loss/Shrinkage" },
            new StockAdjustmentReason { StockAdjustmentReasonId = 3, Reason = "Unexpected Receipt/Found" },
            new StockAdjustmentReason { StockAdjustmentReasonId = 4, Reason = "Physical Count Variance" },
            new StockAdjustmentReason { StockAdjustmentReasonId = 5, Reason = "Expired/Obsolete" },
            new StockAdjustmentReason { StockAdjustmentReasonId = 6, Reason = "Initial Inventory Adjustment:" }
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

        // Seed Inventory data for each product and branch
        modelBuilder.Entity<Inventory>().HasData(
            // Product 1 Inventory
            new Inventory { InventoryId = 1, ProductId = 1, BranchId = 1, OnHandquantity = 100, AvailableQuantity = 100, IncomingQuantity = 0, Allocated = 0 },
            new Inventory { InventoryId = 2, ProductId = 1, BranchId = 2, OnHandquantity = 50, AvailableQuantity = 50, IncomingQuantity = 0, Allocated = 0 },
            new Inventory {InventoryId = 3, ProductId = 1, BranchId = 3, OnHandquantity = 75, AvailableQuantity = 75, IncomingQuantity = 0, Allocated = 0 },

            // Product 2 Inventory
            new Inventory { InventoryId = 4, ProductId = 2, BranchId = 1, OnHandquantity = 200, AvailableQuantity = 200, IncomingQuantity = 0, Allocated = 0 },
            new Inventory { InventoryId = 5, ProductId = 2, BranchId = 2, OnHandquantity = 150, AvailableQuantity = 150, IncomingQuantity = 0, Allocated = 0 },
            new Inventory { InventoryId = 6, ProductId = 2, BranchId = 3, OnHandquantity = 150, AvailableQuantity = 150, IncomingQuantity = 0, Allocated = 0 },

            new Inventory { InventoryId = 7, ProductId = 3, BranchId = 1, OnHandquantity = 80, AvailableQuantity = 80, IncomingQuantity = 0, Allocated = 0 },
            new Inventory { InventoryId = 8, ProductId = 3, BranchId = 2, OnHandquantity = 120, AvailableQuantity = 120, IncomingQuantity = 0, Allocated = 0 },
            new Inventory { InventoryId = 9, ProductId = 3, BranchId = 3, OnHandquantity = 90 },

            // Product 4 Inventory
            new Inventory { InventoryId = 10, ProductId = 4, BranchId = 1, OnHandquantity = 300, AvailableQuantity = 300, IncomingQuantity = 0, Allocated = 0 },
            new Inventory { InventoryId = 11, ProductId = 4, BranchId = 2, OnHandquantity = 250, AvailableQuantity = 250, IncomingQuantity = 0, Allocated = 0 },
            new Inventory { InventoryId = 12, ProductId = 4, BranchId = 3, OnHandquantity = 280, AvailableQuantity = 280, IncomingQuantity = 0, Allocated = 0 },

            // Product 5 Inventory
            new Inventory { InventoryId = 13, ProductId = 5, BranchId = 1, OnHandquantity = 60, AvailableQuantity = 60, IncomingQuantity = 0, Allocated = 0 },
            new Inventory {InventoryId = 14, ProductId = 5, BranchId = 2, OnHandquantity = 40, AvailableQuantity = 40, IncomingQuantity = 0, Allocated = 0 },
            new Inventory {InventoryId = 15, ProductId = 5, BranchId = 3, OnHandquantity = 70, AvailableQuantity = 70, IncomingQuantity = 0, Allocated = 0 }
        );





    }
}
