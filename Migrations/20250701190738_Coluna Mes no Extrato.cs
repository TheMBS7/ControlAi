using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ColunaMesnoExtrato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MesId",
                table: "Extratos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Extratos_MesId",
                table: "Extratos",
                column: "MesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Extratos_Meses_MesId",
                table: "Extratos",
                column: "MesId",
                principalTable: "Meses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Extratos_Meses_MesId",
                table: "Extratos");

            migrationBuilder.DropIndex(
                name: "IX_Extratos_MesId",
                table: "Extratos");

            migrationBuilder.DropColumn(
                name: "MesId",
                table: "Extratos");
        }
    }
}
