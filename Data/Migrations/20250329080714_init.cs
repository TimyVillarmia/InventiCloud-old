using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttributeSets",
                columns: table => new
                {
                    AttributeSetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeSetName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeSets", x => x.AttributeSetId);
                });

            migrationBuilder.CreateTable(
                name: "AttributeValues",
                columns: table => new
                {
                    AttributeValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeValues", x => x.AttributeValueId);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderStatuses",
                columns: table => new
                {
                    PurchaseOrderStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderStatuses", x => x.PurchaseOrderStatusId);
                });

            migrationBuilder.CreateTable(
                name: "StockAdjustmentReasons",
                columns: table => new
                {
                    StockAdjustmentReasonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAdjustmentReasons", x => x.StockAdjustmentReasonId);
                });

            migrationBuilder.CreateTable(
                name: "StockAdjustmentStatuses",
                columns: table => new
                {
                    StockAdjustmentStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAdjustmentStatuses", x => x.StockAdjustmentStatusId);
                });

            migrationBuilder.CreateTable(
                name: "StockTransferStatuses",
                columns: table => new
                {
                    StockTransferStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransferStatuses", x => x.StockTransferStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierCode);
                });

            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    AttributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttributeSetId = table.Column<int>(type: "int", nullable: false),
                    isRequired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.AttributeId);
                    table.ForeignKey(
                        name: "FK_Attributes_AttributeSets_AttributeSetId",
                        column: x => x.AttributeSetId,
                        principalTable: "AttributeSets",
                        principalColumn: "AttributeSetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchAccounts",
                columns: table => new
                {
                    BranchAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchAccounts", x => x.BranchAccountId);
                    table.ForeignKey(
                        name: "FK_BranchAccounts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchAccounts_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    AttributeSetId = table.Column<int>(type: "int", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_AttributeSets_AttributeSetId",
                        column: x => x.AttributeSetId,
                        principalTable: "AttributeSets",
                        principalColumn: "AttributeSetId");
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockAdjustments",
                columns: table => new
                {
                    StockAdjustmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasonId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    AdjustedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdjustedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAdjustments", x => x.StockAdjustmentId);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_AspNetUsers_AdjustedBy",
                        column: x => x.AdjustedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_StockAdjustmentReasons_ReasonId",
                        column: x => x.ReasonId,
                        principalTable: "StockAdjustmentReasons",
                        principalColumn: "StockAdjustmentReasonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_StockAdjustmentStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StockAdjustmentStatuses",
                        principalColumn: "StockAdjustmentStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransfers",
                columns: table => new
                {
                    StockTransferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceBranchId = table.Column<int>(type: "int", nullable: false),
                    DestinationBranchId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransfers", x => x.StockTransferId);
                    table.ForeignKey(
                        name: "FK_StockTransfers_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Branches_DestinationBranchId",
                        column: x => x.DestinationBranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Branches_SourceBranchId",
                        column: x => x.SourceBranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_StockTransfers_StockTransferStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StockTransferStatuses",
                        principalColumn: "StockTransferStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DestinationBranchId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedArrival = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchasedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.PurchaseOrderId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Branches_DestinationBranchId",
                        column: x => x.DestinationBranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PurchaseOrderStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "PurchaseOrderStatuses",
                        principalColumn: "PurchaseOrderStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Suppliers_SupplierCode",
                        column: x => x.SupplierCode,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    InventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    OnHandquantity = table.Column<int>(type: "int", nullable: false),
                    IncomingQuantity = table.Column<int>(type: "int", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    Allocated = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.InventoryId);
                    table.ForeignKey(
                        name: "FK_Inventories_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeValues",
                columns: table => new
                {
                    ProductAttributeValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    AttributeValueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeValues", x => x.ProductAttributeValueId);
                    table.ForeignKey(
                        name: "FK_ProductAttributeValues_AttributeValues_AttributeValueId",
                        column: x => x.AttributeValueId,
                        principalTable: "AttributeValues",
                        principalColumn: "AttributeValueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAttributeValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAttributeValues_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransferItems",
                columns: table => new
                {
                    StockTransferItemlId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockTransferId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    TransferQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransferItems", x => x.StockTransferItemlId);
                    table.ForeignKey(
                        name: "FK_StockTransferItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransferItems_StockTransfers_StockTransferId",
                        column: x => x.StockTransferId,
                        principalTable: "StockTransfers",
                        principalColumn: "StockTransferId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderItems",
                columns: table => new
                {
                    PurchaseOrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderItems", x => x.PurchaseOrderItemId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderID",
                        column: x => x.PurchaseOrderID,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockAdjustmentDetails",
                columns: table => new
                {
                    StockAdjustmentDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockAdjustmentId = table.Column<int>(type: "int", nullable: false),
                    InventoryId = table.Column<int>(type: "int", nullable: false),
                    PreviousQuantity = table.Column<int>(type: "int", nullable: false),
                    NewQuantity = table.Column<int>(type: "int", nullable: false),
                    AdjustedQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAdjustmentDetails", x => x.StockAdjustmentDetailId);
                    table.ForeignKey(
                        name: "FK_StockAdjustmentDetails_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "InventoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockAdjustmentDetails_StockAdjustments_StockAdjustmentId",
                        column: x => x.StockAdjustmentId,
                        principalTable: "StockAdjustments",
                        principalColumn: "StockAdjustmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "your-user-id-1", 0, "7da4ec69-d502-46a6-9430-dfd9a7f4271d", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEO8jA0GpcRSNnPR/fOKm9QSnBcqgtRtPDIpeZiG3N3ITaLSot0vKM+dIeRv/kwfqzw==", null, false, "797f5db8-d836-41ee-b9fb-28b6695e8542", false, "admin" });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "BranchId", "Address", "BranchName", "City", "Country", "Email", "PhoneNumber", "PostalCode", "Region" },
                values: new object[,]
                {
                    { 1, "123 Main St", "Branch A", "Anytown", "USA", "warehouse@example.com", "555-123-4567", "12345", "State" },
                    { 2, "456 Oak Ave", "Branch B", "Springfield", "Canada", "retailA@example.com", "123-456-7890", "A1B 2C3", "Province" },
                    { 3, "789 Pine Ln", "Branch C", "London", "UK", "distribution@example.com", "+44 20 1234 5678", "SW1A 1AA", "England" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Eyeglasses" },
                    { 2, "Contact Lenses" },
                    { 3, "Reading Glasses" },
                    { 4, "Eye Care Products" },
                    { 5, "Sunglasses" }
                });

            migrationBuilder.InsertData(
                table: "PurchaseOrderStatuses",
                columns: new[] { "PurchaseOrderStatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Draft" },
                    { 2, "Ordered" },
                    { 3, "Completed" },
                    { 4, "Cancelled" }
                });

            migrationBuilder.InsertData(
                table: "StockTransferStatuses",
                columns: new[] { "StockTransferStatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Allocated" },
                    { 2, "In Transit" },
                    { 3, "Cancelled" },
                    { 4, "Completed" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierCode", "Address", "City", "Company", "ContactPerson", "Country", "Email", "PhoneNumber", "PostalCode", "SupplierName" },
                values: new object[] { "SUP001", "123 Main St", "New York", "Global Tech Inc.", "John Doe", "USA", "john.doe@globaltech.com", "+15551234567", "12345", "Global Electronics" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "AttributeSetId", "Brand", "CategoryId", "Description", "ImageURL", "ProductName", "SKU", "UnitCost", "UnitPrice", "isActive" },
                values: new object[,]
                {
                    { 1, null, "VisionGuard", 1, "High-quality glasses to protect your eyes from harmful blue light.", "glasses_blue_light.jpg", "Premium Blue Light Blocking Glasses", "VG-BL-001", 50.00m, 120.00m, true },
                    { 2, null, "AquaView", 2, "Comfortable daily disposable contact lenses for clear vision.", "contact_lenses_daily.jpg", "Daily Disposable Contact Lenses", "AV-CD-002", 15.00m, 35.00m, true },
                    { 3, null, "ReadWell", 3, "Stylish reading glasses with anti-glare coating for reduced eye strain.", "reading_glasses_anti_glare.jpg", "Anti-Glare Reading Glasses", "RW-RG-003", 25.00m, 60.00m, true },
                    { 4, null, "MoisturePlus", 4, "Relief from dry, irritated eyes with these lubricating eye drops.", "eye_drops_dry_eyes.jpg", "Eye Drops for Dry Eyes", "MP-ED-004", 8.00m, 20.00m, true },
                    { 5, null, "SunStyle", 1, "Fashionable sunglasses with UV protection for sunny days.", "designer_sunglasses.jpg", "Designer Sunglasses", "SS-SG-005", 80.00m, 200.00m, true }
                });

            migrationBuilder.InsertData(
                table: "Inventories",
                columns: new[] { "InventoryId", "Allocated", "AvailableQuantity", "BranchId", "IncomingQuantity", "OnHandquantity", "ProductId" },
                values: new object[,]
                {
                    { 1, 0, 0, 1, 0, 100, 1 },
                    { 2, 0, 0, 2, 0, 50, 1 },
                    { 3, 0, 0, 3, 0, 75, 1 },
                    { 4, 0, 0, 1, 0, 200, 2 },
                    { 5, 0, 0, 2, 0, 150, 2 },
                    { 6, 0, 0, 3, 0, 150, 2 },
                    { 7, 0, 0, 1, 0, 80, 3 },
                    { 8, 0, 0, 2, 0, 120, 3 },
                    { 9, 0, 0, 3, 0, 90, 3 },
                    { 10, 0, 0, 1, 0, 300, 4 },
                    { 11, 0, 0, 2, 0, 250, 4 },
                    { 12, 0, 0, 3, 0, 280, 4 },
                    { 13, 0, 0, 1, 0, 60, 5 },
                    { 14, 0, 0, 2, 0, 40, 5 },
                    { 15, 0, 0, 3, 0, 70, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_AttributeSetId",
                table: "Attributes",
                column: "AttributeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchAccounts_ApplicationUserId",
                table: "BranchAccounts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchAccounts_BranchId",
                table: "BranchAccounts",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName",
                table: "Categories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_BranchId",
                table: "Inventories",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId",
                table: "Inventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_AttributeId",
                table: "ProductAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_AttributeValueId",
                table: "ProductAttributeValues",
                column: "AttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_ProductId",
                table: "ProductAttributeValues",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AttributeSetId",
                table: "Products",
                column: "AttributeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductName",
                table: "Products",
                column: "ProductName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                table: "Products",
                column: "SKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_ProductID",
                table: "PurchaseOrderItems",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_PurchaseOrderID",
                table: "PurchaseOrderItems",
                column: "PurchaseOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CreatedById",
                table: "PurchaseOrders",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_DestinationBranchId",
                table: "PurchaseOrders",
                column: "DestinationBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_StatusId",
                table: "PurchaseOrders",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierCode",
                table: "PurchaseOrders",
                column: "SupplierCode");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentDetails_InventoryId",
                table: "StockAdjustmentDetails",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentDetails_StockAdjustmentId",
                table: "StockAdjustmentDetails",
                column: "StockAdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_AdjustedBy",
                table: "StockAdjustments",
                column: "AdjustedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_ReasonId",
                table: "StockAdjustments",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_StatusId",
                table: "StockAdjustments",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferItems_ProductId",
                table: "StockTransferItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferItems_StockTransferId",
                table: "StockTransferItems",
                column: "StockTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_CreatedById",
                table: "StockTransfers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_DestinationBranchId",
                table: "StockTransfers",
                column: "DestinationBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_SourceBranchId",
                table: "StockTransfers",
                column: "SourceBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_StatusId",
                table: "StockTransfers",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierCode",
                table: "Suppliers",
                column: "SupplierCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchAccounts");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "ProductAttributeValues");

            migrationBuilder.DropTable(
                name: "PurchaseOrderItems");

            migrationBuilder.DropTable(
                name: "StockAdjustmentDetails");

            migrationBuilder.DropTable(
                name: "StockTransferItems");

            migrationBuilder.DropTable(
                name: "AttributeValues");

            migrationBuilder.DropTable(
                name: "Attributes");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "StockAdjustments");

            migrationBuilder.DropTable(
                name: "StockTransfers");

            migrationBuilder.DropTable(
                name: "PurchaseOrderStatuses");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "StockAdjustmentReasons");

            migrationBuilder.DropTable(
                name: "StockAdjustmentStatuses");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "StockTransferStatuses");

            migrationBuilder.DropTable(
                name: "AttributeSets");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1");
        }
    }
}
