using Microsoft.EntityFrameworkCore.Migrations;

namespace GadgetStore.Migrations
{
    public partial class @in : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Carts_CartId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Gadgets_Carts_CartId",
                table: "Gadgets");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Gadgets_CartId",
                table: "Gadgets");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CartId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Gadgets");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Gadgets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalCost = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gadgets_CartId",
                table: "Gadgets",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CartId",
                table: "AspNetUsers",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Carts_CartId",
                table: "AspNetUsers",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gadgets_Carts_CartId",
                table: "Gadgets",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
