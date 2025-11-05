using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Add_Optional_FKs_EntradaFixa_SaidaFixa_To_Extratos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntradaFixaId",
                table: "Extratos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SaidaFixaId",
                table: "Extratos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Extratos_EntradaFixaId",
                table: "Extratos",
                column: "EntradaFixaId");

            migrationBuilder.CreateIndex(
                name: "IX_Extratos_SaidaFixaId",
                table: "Extratos",
                column: "SaidaFixaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Extratos_EntradasFixas_EntradaFixaId",
                table: "Extratos",
                column: "EntradaFixaId",
                principalTable: "EntradasFixas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Extratos_SaidasFixas_SaidaFixaId",
                table: "Extratos",
                column: "SaidaFixaId",
                principalTable: "SaidasFixas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Extratos_EntradasFixas_EntradaFixaId",
                table: "Extratos");

            migrationBuilder.DropForeignKey(
                name: "FK_Extratos_SaidasFixas_SaidaFixaId",
                table: "Extratos");

            migrationBuilder.DropIndex(
                name: "IX_Extratos_EntradaFixaId",
                table: "Extratos");

            migrationBuilder.DropIndex(
                name: "IX_Extratos_SaidaFixaId",
                table: "Extratos");

            migrationBuilder.DropColumn(
                name: "EntradaFixaId",
                table: "Extratos");

            migrationBuilder.DropColumn(
                name: "SaidaFixaId",
                table: "Extratos");
        }
    }
}
