using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Add_PessoId_SaidaFixa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PessoaId",
                table: "SaidasFixas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SaidasFixas_PessoaId",
                table: "SaidasFixas",
                column: "PessoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaidasFixas_Pessoas_PessoaId",
                table: "SaidasFixas",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaidasFixas_Pessoas_PessoaId",
                table: "SaidasFixas");

            migrationBuilder.DropIndex(
                name: "IX_SaidasFixas_PessoaId",
                table: "SaidasFixas");

            migrationBuilder.DropColumn(
                name: "PessoaId",
                table: "SaidasFixas");
        }
    }
}
