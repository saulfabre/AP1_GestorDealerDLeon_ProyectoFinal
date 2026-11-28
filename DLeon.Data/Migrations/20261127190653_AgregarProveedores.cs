using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLeon.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProveedores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[dbo].[Proveedores]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[Proveedores] (
                        [ProveedorId] int NOT NULL IDENTITY,
                        [Nombre] nvarchar(max) NOT NULL,
                        [RNC] nvarchar(max) NOT NULL,
                        [Telefono] nvarchar(max) NOT NULL,
                        [Correo] nvarchar(max) NOT NULL,
                        [Direccion] nvarchar(max) NOT NULL,
                        [Activo] bit NOT NULL,
                        CONSTRAINT [PK_Proveedores] PRIMARY KEY ([ProveedorId])
                    );
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[dbo].[Proveedores]', N'U') IS NOT NULL
                    DROP TABLE [dbo].[Proveedores];
                """);
        }
    }
}
