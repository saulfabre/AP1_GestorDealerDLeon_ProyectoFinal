using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLeon.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[dbo].[Vehiculos]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[Vehiculos] (
                        [VehiculoId] int NOT NULL IDENTITY,
                        [Marca] nvarchar(50) NOT NULL,
                        [Modelo] nvarchar(50) NOT NULL,
                        [Ano] int NOT NULL,
                        [Matricula] nvarchar(20) NOT NULL,
                        [Color] nvarchar(30) NOT NULL,
                        [Precio] decimal(18,2) NOT NULL,
                        [Kilometraje] int NOT NULL,
                        [Combustible] nvarchar(30) NOT NULL,
                        [Transmision] nvarchar(30) NOT NULL,
                        [Foto] nvarchar(500) NOT NULL,
                        CONSTRAINT [PK_Vehiculos] PRIMARY KEY ([VehiculoId])
                    );
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehiculos");
        }
    }
}
