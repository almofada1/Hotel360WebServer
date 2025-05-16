using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel360InteractiveServer.Data.Migrations.Auth
{
    /// <inheritdoc />
    public partial class CreateRefeicoesConfirmacaoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfirmRefeicao",
                columns: table => new
                {
                    CodigoRefeicao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataRefeicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoRefeicao = table.Column<int>(type: "int", nullable: false),
                    Confirmed = table.Column<bool>(type: "bit", nullable: false)
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfirmRefeicao");
        }
    }
}
