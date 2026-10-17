using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AcademicApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dosens",
                columns: table => new
                {
                    DosenId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nidn = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    NamaDosen = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Prodi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dosens", x => x.DosenId);
                });

            migrationBuilder.CreateTable(
                name: "Mahasiswas",
                columns: table => new
                {
                    MahasiswaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nim = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    NamaMahasiswa = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Angkatan = table.Column<int>(type: "INTEGER", nullable: false),
                    DosenId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahasiswas", x => x.MahasiswaId);
                    table.ForeignKey(
                        name: "FK_Mahasiswas_Dosens_DosenId",
                        column: x => x.DosenId,
                        principalTable: "Dosens",
                        principalColumn: "DosenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Dosens",
                columns: new[] { "DosenId", "Email", "NamaDosen", "Nidn", "Prodi" },
                values: new object[,]
                {
                    { 1, "anita@kampus.ac.id", "Dr. Anita Wijaya", "0512345678", "Sistem Informasi" },
                    { 2, "budi.r@kampus.ac.id", "Budi Raharja, M.Kom.", "0587654321", "Informatika" },
                    { 3, "cintya@kampus.ac.id", "Cintya Dewi, Ph.D.", "0511223344", "Sistem Informasi" }
                });

            migrationBuilder.InsertData(
                table: "Mahasiswas",
                columns: new[] { "MahasiswaId", "Angkatan", "DosenId", "Email", "NamaMahasiswa", "Nim" },
                values: new object[,]
                {
                    { 1, 2023, 1, "budi@student.kampus.ac.id", "Budi Santoso", "72230001" },
                    { 2, 2023, 1, "siti@student.kampus.ac.id", "Siti Aminah", "72230002" },
                    { 3, 2022, 2, "andi@student.kampus.ac.id", "Andi Wijaya", "72220001" },
                    { 4, 2024, 2, "dewi@student.kampus.ac.id", "Dewi Lestari", "72240001" },
                    { 5, 2023, 3, "eko@student.kampus.ac.id", "Eko Prasetyo", "72230003" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dosens_Nidn",
                table: "Dosens",
                column: "Nidn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mahasiswas_DosenId",
                table: "Mahasiswas",
                column: "DosenId");

            migrationBuilder.CreateIndex(
                name: "IX_Mahasiswas_Nim",
                table: "Mahasiswas",
                column: "Nim",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mahasiswas");

            migrationBuilder.DropTable(
                name: "Dosens");
        }
    }
}
