using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLeon.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[dbo].[Modelos]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[Modelos] (
                        [ModeloId] int NOT NULL IDENTITY,
                        [Marca] nvarchar(50) NOT NULL,
                        [Nombre] nvarchar(50) NOT NULL,
                        [Activo] bit NOT NULL,
                        CONSTRAINT [PK_Modelos] PRIMARY KEY ([ModeloId])
                    );
                END

                IF OBJECT_ID(N'[dbo].[Vehiculos]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[Vehiculos] (
                        [VehiculoId] int NOT NULL IDENTITY,
                        [ModeloId] int NOT NULL,
                        [Ano] int NOT NULL,
                        [Matricula] nvarchar(20) NOT NULL,
                        [Color] nvarchar(30) NOT NULL,
                        [Precio] decimal(18,2) NOT NULL,
                        [Kilometraje] int NOT NULL,
                        [Tipo] nvarchar(30) NOT NULL,
                        [Combustible] nvarchar(30) NOT NULL,
                        [Transmision] nvarchar(30) NOT NULL,
                        [Estado] nvarchar(20) NOT NULL,
                        [Foto] nvarchar(500) NOT NULL,
                        CONSTRAINT [PK_Vehiculos] PRIMARY KEY ([VehiculoId]),
                        CONSTRAINT [FK_Vehiculos_Modelos_ModeloId] FOREIGN KEY ([ModeloId])
                            REFERENCES [dbo].[Modelos] ([ModeloId]) ON DELETE NO ACTION
                    );
                END
                ELSE IF COL_LENGTH(N'dbo.Vehiculos', N'ModeloId') IS NULL
                BEGIN
                    INSERT INTO [dbo].[Modelos] ([Marca], [Nombre], [Activo])
                    SELECT DISTINCT v.[Marca], v.[Modelo], 1
                    FROM [dbo].[Vehiculos] AS v
                    WHERE NOT EXISTS (
                        SELECT 1 FROM [dbo].[Modelos] AS m
                        WHERE m.[Marca] = v.[Marca] AND m.[Nombre] = v.[Modelo]);

                    ALTER TABLE [dbo].[Vehiculos] ADD [ModeloId] int NULL;
                    ALTER TABLE [dbo].[Vehiculos] ADD [Tipo] nvarchar(30) NOT NULL
                        CONSTRAINT [DF_Vehiculos_Tipo_Migracion] DEFAULT N'Sedan';
                    ALTER TABLE [dbo].[Vehiculos] ADD [Estado] nvarchar(20) NOT NULL
                        CONSTRAINT [DF_Vehiculos_Estado_Migracion] DEFAULT N'Disponible';

                    UPDATE v SET v.[ModeloId] = m.[ModeloId]
                    FROM [dbo].[Vehiculos] AS v
                    INNER JOIN [dbo].[Modelos] AS m
                        ON m.[Marca] = v.[Marca] AND m.[Nombre] = v.[Modelo];

                    ALTER TABLE [dbo].[Vehiculos] ALTER COLUMN [ModeloId] int NOT NULL;
                    ALTER TABLE [dbo].[Vehiculos] DROP CONSTRAINT [DF_Vehiculos_Tipo_Migracion];
                    ALTER TABLE [dbo].[Vehiculos] DROP CONSTRAINT [DF_Vehiculos_Estado_Migracion];
                    ALTER TABLE [dbo].[Vehiculos] DROP COLUMN [Marca], [Modelo];
                END

                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE [name] = N'IX_Modelos_Marca_Nombre')
                    CREATE UNIQUE INDEX [IX_Modelos_Marca_Nombre] ON [dbo].[Modelos] ([Marca], [Nombre]);

                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE [name] = N'IX_Vehiculos_Matricula')
                    CREATE UNIQUE INDEX [IX_Vehiculos_Matricula] ON [dbo].[Vehiculos] ([Matricula]);

                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE [name] = N'IX_Vehiculos_ModeloId')
                    CREATE INDEX [IX_Vehiculos_ModeloId] ON [dbo].[Vehiculos] ([ModeloId]);

                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE [name] = N'FK_Vehiculos_Modelos_ModeloId')
                    ALTER TABLE [dbo].[Vehiculos] ADD CONSTRAINT [FK_Vehiculos_Modelos_ModeloId]
                        FOREIGN KEY ([ModeloId]) REFERENCES [dbo].[Modelos] ([ModeloId]) ON DELETE NO ACTION;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[dbo].[Vehiculos]', N'U') IS NOT NULL
                   AND COL_LENGTH(N'dbo.Vehiculos', N'ModeloId') IS NOT NULL
                   AND COL_LENGTH(N'dbo.Vehiculos', N'Marca') IS NULL
                BEGIN
                    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE [name] = N'FK_Vehiculos_Modelos_ModeloId')
                        ALTER TABLE [dbo].[Vehiculos] DROP CONSTRAINT [FK_Vehiculos_Modelos_ModeloId];
                    IF EXISTS (SELECT 1 FROM sys.indexes WHERE [name] = N'IX_Vehiculos_ModeloId')
                        DROP INDEX [IX_Vehiculos_ModeloId] ON [dbo].[Vehiculos];
                    IF EXISTS (SELECT 1 FROM sys.indexes WHERE [name] = N'IX_Vehiculos_Matricula')
                        DROP INDEX [IX_Vehiculos_Matricula] ON [dbo].[Vehiculos];

                    ALTER TABLE [dbo].[Vehiculos] ADD [Marca] nvarchar(50) NULL;
                    ALTER TABLE [dbo].[Vehiculos] ADD [Modelo] nvarchar(50) NULL;
                    UPDATE v SET v.[Marca] = m.[Marca], v.[Modelo] = m.[Nombre]
                    FROM [dbo].[Vehiculos] AS v
                    INNER JOIN [dbo].[Modelos] AS m ON m.[ModeloId] = v.[ModeloId];
                    ALTER TABLE [dbo].[Vehiculos] ALTER COLUMN [Marca] nvarchar(50) NOT NULL;
                    ALTER TABLE [dbo].[Vehiculos] ALTER COLUMN [Modelo] nvarchar(50) NOT NULL;
                    ALTER TABLE [dbo].[Vehiculos] DROP COLUMN [ModeloId], [Tipo], [Estado];
                    DROP TABLE [dbo].[Modelos];
                END
                """);
        }
    }
}
