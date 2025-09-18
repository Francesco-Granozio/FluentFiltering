using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Iniziale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Giochi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Titolo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrezzoListino = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataRilascio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Genere = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Piattaforma = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sviluppatore = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DataCreazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaModifica = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCancellato = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DataCancellazione = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Giochi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utenti",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    NomeCompleto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Paese = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataRegistrazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataCreazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaModifica = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCancellato = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DataCancellazione = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utenti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Acquisti",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GiocoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataAcquisto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrezzoPagato = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantita = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    MetodoPagamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodiceSconto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DataCreazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaModifica = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCancellato = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DataCancellazione = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acquisti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Acquisti_Giochi_GiocoId",
                        column: x => x.GiocoId,
                        principalTable: "Giochi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Acquisti_Utenti_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recensioni",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GiocoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Punteggio = table.Column<int>(type: "int", nullable: false),
                    Titolo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Corpo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataRecensione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRecensioneVerificata = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AcquistoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataCreazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaModifica = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCancellato = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DataCancellazione = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recensioni", x => x.Id);
                    table.CheckConstraint("CK_Recensioni_Punteggio", "Punteggio BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_Recensioni_Acquisti_AcquistoId",
                        column: x => x.AcquistoId,
                        principalTable: "Acquisti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Recensioni_Giochi_GiocoId",
                        column: x => x.GiocoId,
                        principalTable: "Giochi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recensioni_Utenti_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Acquisti_DataAcquisto",
                table: "Acquisti",
                column: "DataAcquisto");

            migrationBuilder.CreateIndex(
                name: "IX_Acquisti_GiocoId",
                table: "Acquisti",
                column: "GiocoId");

            migrationBuilder.CreateIndex(
                name: "IX_Acquisti_UtenteId",
                table: "Acquisti",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Acquisti_UtenteId_GiocoId",
                table: "Acquisti",
                columns: new[] { "UtenteId", "GiocoId" });

            migrationBuilder.CreateIndex(
                name: "IX_Giochi_DataRilascio",
                table: "Giochi",
                column: "DataRilascio");

            migrationBuilder.CreateIndex(
                name: "IX_Giochi_Genere",
                table: "Giochi",
                column: "Genere");

            migrationBuilder.CreateIndex(
                name: "IX_Giochi_Piattaforma",
                table: "Giochi",
                column: "Piattaforma");

            migrationBuilder.CreateIndex(
                name: "IX_Giochi_PrezzoListino",
                table: "Giochi",
                column: "PrezzoListino");

            migrationBuilder.CreateIndex(
                name: "IX_Giochi_Titolo",
                table: "Giochi",
                column: "Titolo");

            migrationBuilder.CreateIndex(
                name: "IX_Recensioni_AcquistoId",
                table: "Recensioni",
                column: "AcquistoId");

            migrationBuilder.CreateIndex(
                name: "IX_Recensioni_DataRecensione",
                table: "Recensioni",
                column: "DataRecensione");

            migrationBuilder.CreateIndex(
                name: "IX_Recensioni_GiocoId",
                table: "Recensioni",
                column: "GiocoId");

            migrationBuilder.CreateIndex(
                name: "IX_Recensioni_IsRecensioneVerificata",
                table: "Recensioni",
                column: "IsRecensioneVerificata");

            migrationBuilder.CreateIndex(
                name: "IX_Recensioni_Punteggio",
                table: "Recensioni",
                column: "Punteggio");

            migrationBuilder.CreateIndex(
                name: "IX_Recensioni_UtenteId",
                table: "Recensioni",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Recensioni_UtenteId_GiocoId_Unique",
                table: "Recensioni",
                columns: new[] { "UtenteId", "GiocoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utenti_DataRegistrazione",
                table: "Utenti",
                column: "DataRegistrazione");

            migrationBuilder.CreateIndex(
                name: "IX_Utenti_Email_Unique",
                table: "Utenti",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utenti_Username_Unique",
                table: "Utenti",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recensioni");

            migrationBuilder.DropTable(
                name: "Acquisti");

            migrationBuilder.DropTable(
                name: "Giochi");

            migrationBuilder.DropTable(
                name: "Utenti");
        }
    }
}
