using Microsoft.EntityFrameworkCore.Migrations;

namespace GadgetStore.Migrations
{
    public partial class qwr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gadgets_Orders_OrderId",
                table: "Gadgets");

            migrationBuilder.DropIndex(
                name: "IX_Gadgets_OrderId",
                table: "Gadgets");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Gadgets");

            migrationBuilder.AddColumn<string>(
                name: "Cart",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cart",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Gadgets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gadgets_OrderId",
                table: "Gadgets",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gadgets_Orders_OrderId",
                table: "Gadgets",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
