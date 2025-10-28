using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Add_TipoMovimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoMovimentoId",
                table: "Extratos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TiposMovimentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tipo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposMovimentos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Extratos_TipoMovimentoId",
                table: "Extratos",
                column: "TipoMovimentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Extratos_TiposMovimentos_TipoMovimentoId",
                table: "Extratos",
                column: "TipoMovimentoId",
                principalTable: "TiposMovimentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Extratos_TiposMovimentos_TipoMovimentoId",
                table: "Extratos");

            migrationBuilder.DropTable(
                name: "TiposMovimentos");

            migrationBuilder.DropIndex(
                name: "IX_Extratos_TipoMovimentoId",
                table: "Extratos");

            migrationBuilder.DropColumn(
                name: "TipoMovimentoId",
                table: "Extratos");
        }
    }
}
