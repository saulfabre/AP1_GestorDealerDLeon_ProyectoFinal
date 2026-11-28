using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLeon.Data.Migrations
{
    /// <inheritdoc />
    public partial class SincronizarModeloActual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Las tablas se crean en las migraciones AgregarProveedores y AgregarRepuestos.
            // Esta migraciÃ³n solo sincroniza el snapshot con el modelo combinado actual.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No hay cambios de esquema que revertir.
        }
    }
}
