using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLeon.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarComprasRepuestos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComprasRepuestos",
                columns: table => new
                {
                    CompraId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroFactura = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprasRepuestos", x => x.CompraId);
                    table.ForeignKey(
                        name: "FK_ComprasRepuestos_Proveedores_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedores",
                        principalColumn: "ProveedorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComprasRepuestosDetalles",
                columns: table => new
                {
                    DetalleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompraId = table.Column<int>(type: "int", nullable: false),
                    RepuestoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprasRepuestosDetalles", x => x.DetalleId);
                    table.ForeignKey(
                        name: "FK_ComprasRepuestosDetalles_ComprasRepuestos_CompraId",
                        column: x => x.CompraId,
                        principalTable: "ComprasRepuestos",
                        principalColumn: "CompraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComprasRepuestosDetalles_Repuestos_RepuestoId",
                        column: x => x.RepuestoId,
                        principalTable: "Repuestos",
                        principalColumn: "RepuestoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComprasRepuestos_ProveedorId",
                table: "ComprasRepuestos",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprasRepuestosDetalles_CompraId",
                table: "ComprasRepuestosDetalles",
                column: "CompraId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprasRepuestosDetalles_RepuestoId",
                table: "ComprasRepuestosDetalles",
                column: "RepuestoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComprasRepuestosDetalles");

            migrationBuilder.DropTable(
                name: "ComprasRepuestos");
        }
    }
}
