using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sashiel_CLDV6211_Part2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Nee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Firstname",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lastname",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Product_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Product_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Product_Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Product_ID);
                });

            migrationBuilder.CreateTable(
                name: "CartOrders",
                columns: table => new
                {
                    Cart_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product_ID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartOrders", x => x.Cart_ID);
                    table.ForeignKey(
                        name: "FK_CartOrders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartOrders_Products_Product_ID",
                        column: x => x.Product_ID,
                        principalTable: "Products",
                        principalColumn: "Product_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseHistory",
                columns: table => new
                {
                    History_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product_ID = table.Column<int>(type: "int", nullable: false),
                    PDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseHistory", x => x.History_ID);
                    table.ForeignKey(
                        name: "FK_PurchaseHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseHistory_Products_Product_ID",
                        column: x => x.Product_ID,
                        principalTable: "Products",
                        principalColumn: "Product_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesStatement",
                columns: table => new
                {
                    Statement_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product_ID = table.Column<int>(type: "int", nullable: false),
                    Purchase_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesStatement", x => x.Statement_ID);
                    table.ForeignKey(
                        name: "FK_SalesStatement_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesStatement_Products_Product_ID",
                        column: x => x.Product_ID,
                        principalTable: "Products",
                        principalColumn: "Product_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartOrders_Product_ID",
                table: "CartOrders",
                column: "Product_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CartOrders_UserId",
                table: "CartOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistory_Product_ID",
                table: "PurchaseHistory",
                column: "Product_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistory_UserId",
                table: "PurchaseHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesStatement_Product_ID",
                table: "SalesStatement",
                column: "Product_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesStatement_UserId",
                table: "SalesStatement",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartOrders");

            migrationBuilder.DropTable(
                name: "PurchaseHistory");

            migrationBuilder.DropTable(
                name: "SalesStatement");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Firstname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Lastname",
                table: "AspNetUsers");
        }
    }
}
