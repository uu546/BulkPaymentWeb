using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BulkPaymentWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "registries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    registry_id = table.Column<int>(type: "integer", nullable: false),
                    payer_inn = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    payer_account = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    receiver_inn = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    receiver_account = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    receiver_bik = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    purpose = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_valid = table.Column<bool>(type: "boolean", nullable: false),
                    validation_error = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_registries_registry_id",
                        column: x => x.registry_id,
                        principalTable: "registries",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_payments_registry_id",
                table: "payments",
                column: "registry_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "registries");
        }
    }
}
