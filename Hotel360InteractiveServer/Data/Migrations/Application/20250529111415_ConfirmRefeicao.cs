using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel360InteractiveServer.Data.Migrations.Application
{
    /// <inheritdoc />
    public partial class ConfirmRefeicao : Migration
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
                    TipoRefeicao = table.Column<string>(type: "nvarchar(50)", nullable: false),
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
