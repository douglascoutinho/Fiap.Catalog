using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Catalog.Infra.Migrations
{
    /// <inheritdoc />
    public partial class CommitInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Jogo",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Preco = table.Column<decimal>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JogoBiblioteca",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id_Usuario = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_Jogo = table.Column<int>(type: "INTEGER", nullable: false),
                    Preco = table.Column<decimal>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JogoBiblioteca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JogoBiblioteca_Jogo",
                        column: x => x.Id_Jogo,
                        principalSchema: "dbo",
                        principalTable: "Jogo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JogoBiblioteca_Id_Jogo",
                schema: "dbo",
                table: "JogoBiblioteca",
                column: "Id_Jogo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JogoBiblioteca",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Jogo",
                schema: "dbo");
        }
    }
}
