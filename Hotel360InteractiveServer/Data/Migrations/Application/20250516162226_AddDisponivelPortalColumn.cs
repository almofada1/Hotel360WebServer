using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel360InteractiveServer.Data.Migrations.Application
{
    /// <inheritdoc />
    public partial class AddDisponivelPortalColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'whotestadoquartos'
                      AND COLUMN_NAME = 'cdu_disponivelPortal'
                )
                BEGIN
                    ALTER TABLE dbo.whotestadoquartos
                    ADD cdu_disponivelPortal BIT NOT NULL DEFAULT 0;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'whotestadoquartos'
                      AND COLUMN_NAME = 'cdu_disponivelPortal'
                )
                BEGIN
                    ALTER TABLE dbo.whotestadoquartos
                    DROP COLUMN cdu_disponivelPortal;
                END
            ");
        }
    }
}
