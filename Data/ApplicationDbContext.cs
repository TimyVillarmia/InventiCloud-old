using InventiCloud.Entities;
using InventiCloud.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Inventory> Inventories { get; set; }


    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    public DbSet<PurchaseOrderStatus> PurchaseOrderStatuses { get; set; }
    public DbSet<StockAdjustment> StockAdjustments { get; set; }
    public DbSet<StockAdjustmentItem> StockAdjustmentItems { get; set; }
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
            new StockTransferStatus { StockTransferStatusId = 1, StatusName = "Requested" },
            new StockTransferStatus { StockTransferStatusId = 2, StatusName = "Approved" },
            new StockTransferStatus { StockTransferStatusId = 3, StatusName = "Completed" },
            new StockTransferStatus { StockTransferStatusId = 4, StatusName = "Rejected" }
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


    }
}
