using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeafBidAPI.Migrations
{
    /// <inheritdoc />
    public partial class RefactorForeignAuctionToProductsUsingNewAuctionProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionSalesProducts_AuctionSales_AuctionSaleId",
                table: "AuctionSalesProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_AuctionSalesProducts_Products_ProductId",
                table: "AuctionSalesProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Auctions_AuctionId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AuctionId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AuctionId",
                table: "Products");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "AuctionSalesProducts",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsLive",
                table: "Auctions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AuctionProducts",
                columns: table => new
                {
                    AuctionId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ServeOrder = table.Column<int>(type: "int", nullable: false),
                    AuctionStock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionProducts", x => new { x.AuctionId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_AuctionProducts_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuctionProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuctionProducts_ProductId",
                table: "AuctionProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionSalesProducts_AuctionSales_AuctionSaleId",
                table: "AuctionSalesProducts",
                column: "AuctionSaleId",
                principalTable: "AuctionSales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionSalesProducts_Products_ProductId",
                table: "AuctionSalesProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionSalesProducts_AuctionSales_AuctionSaleId",
                table: "AuctionSalesProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_AuctionSalesProducts_Products_ProductId",
                table: "AuctionSalesProducts");

            migrationBuilder.DropTable(
                name: "AuctionProducts");

            migrationBuilder.DropColumn(
                name: "IsLive",
                table: "Auctions");

            migrationBuilder.AddColumn<int>(
                name: "AuctionId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "AuctionSalesProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AuctionId",
                table: "Products",
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionSalesProducts_AuctionSales_AuctionSaleId",
                table: "AuctionSalesProducts",
                column: "AuctionSaleId",
                principalTable: "AuctionSales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionSalesProducts_Products_ProductId",
                table: "AuctionSalesProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Auctions_AuctionId",
                table: "Products",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id");
        }
    }
}
