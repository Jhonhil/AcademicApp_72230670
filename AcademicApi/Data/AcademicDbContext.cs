using AcademicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicApi.Data
{
    public class AcademicDbContext : DbContext
    {
        public AcademicDbContext(DbContextOptions<AcademicDbContext> options) : base(options) { }

        public DbSet<Dosen> Dosens => Set<Dosen>();
        public DbSet<Mahasiswa> Mahasiswas => Set<Mahasiswa>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Constraint unik NIDN dan NIM
            modelBuilder.Entity<Dosen>()
                .HasIndex(d => d.Nidn)
                .IsUnique();

            modelBuilder.Entity<Mahasiswa>()
                .HasIndex(m => m.Nim)
                .IsUnique();

            // Seed Data Awal
            modelBuilder.Entity<Dosen>().HasData(
                new Dosen { DosenId = 1, Nidn = "0512345678", NamaDosen = "Dr. Anita Wijaya", Email = "anita@kampus.ac.id", Prodi = "Sistem Informasi" },
                new Dosen { DosenId = 2, Nidn = "0587654321", NamaDosen = "Budi Raharja, M.Kom.", Email = "budi.r@kampus.ac.id", Prodi = "Informatika" },
                new Dosen { DosenId = 3, Nidn = "0511223344", NamaDosen = "Cintya Dewi, Ph.D.", Email = "cintya@kampus.ac.id", Prodi = "Sistem Informasi" }
            );

            modelBuilder.Entity<Mahasiswa>().HasData(
                new Mahasiswa { MahasiswaId = 1, Nim = "72230001", NamaMahasiswa = "Budi Santoso", Email = "budi@student.kampus.ac.id", Angkatan = 2023, DosenId = 1 },
                new Mahasiswa { MahasiswaId = 2, Nim = "72230002", NamaMahasiswa = "Siti Aminah", Email = "siti@student.kampus.ac.id", Angkatan = 2023, DosenId = 1 },
                new Mahasiswa { MahasiswaId = 3, Nim = "72220001", NamaMahasiswa = "Andi Wijaya", Email = "andi@student.kampus.ac.id", Angkatan = 2022, DosenId = 2 },
                new Mahasiswa { MahasiswaId = 4, Nim = "72240001", NamaMahasiswa = "Dewi Lestari", Email = "dewi@student.kampus.ac.id", Angkatan = 2024, DosenId = 2 },
                new Mahasiswa { MahasiswaId = 5, Nim = "72230003", NamaMahasiswa = "Eko Prasetyo", Email = "eko@student.kampus.ac.id", Angkatan = 2023, DosenId = 3 }
            );
        }
    }
}