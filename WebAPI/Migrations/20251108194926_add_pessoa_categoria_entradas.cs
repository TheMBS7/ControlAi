using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class add_pessoa_categoria_entradas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "EntradasFixas",
                type: "integer",
                nullable: false,
                defaultValue: 31);

            migrationBuilder.AddColumn<int>(
                name: "PessoaId",
                table: "EntradasFixas",
                type: "integer",
                nullable: false,
                defaultValue: 72);

            migrationBuilder.CreateIndex(
                name: "IX_EntradasFixas_CategoriaId",
                table: "EntradasFixas",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradasFixas_PessoaId",
                table: "EntradasFixas",
                column: "PessoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntradasFixas_Categorias_CategoriaId",
                table: "EntradasFixas",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntradasFixas_Pessoas_PessoaId",
                table: "EntradasFixas",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntradasFixas_Categorias_CategoriaId",
                table: "EntradasFixas");

            migrationBuilder.DropForeignKey(
                name: "FK_EntradasFixas_Pessoas_PessoaId",
                table: "EntradasFixas");

            migrationBuilder.DropIndex(
                name: "IX_EntradasFixas_CategoriaId",
                table: "EntradasFixas");

            migrationBuilder.DropIndex(
                name: "IX_EntradasFixas_PessoaId",
                table: "EntradasFixas");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "EntradasFixas");

            migrationBuilder.DropColumn(
                name: "PessoaId",
                table: "EntradasFixas");
        }
    }
}
