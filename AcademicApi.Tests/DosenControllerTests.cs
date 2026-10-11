using AcademicApi.Controllers;
using AcademicApi.Data;
using AcademicApi.DTOs;
using AcademicApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AcademicApi.Tests
{
    public class DosenControllerTests
    {
        private AcademicDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AcademicDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AcademicDbContext(options);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResultWithDosenList()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Dosens.Add(new Dosen { DosenId = 1, Nidn = "051111", NamaDosen = "Test Dosen", Email = "test@kampus.ac.id", Prodi = "SI" });
            await context.SaveChangesAsync();

            var controller = new DosenController(context);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var list = Assert.IsAssignableFrom<IEnumerable<DosenReadDto>>(okResult.Value);
            Assert.Single(list);
        }

        [Fact]
        public async Task Create_DuplicateNidn_ReturnsConflict()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Dosens.Add(new Dosen { DosenId = 1, Nidn = "051111", NamaDosen = "Dosen Lama", Email = "lama@kampus.ac.id", Prodi = "SI" });
            await context.SaveChangesAsync();

            var controller = new DosenController(context);

            var newDto = new DosenCreateUpdateDto
            {
                Nidn = "051111", // NIDN Sama
                NamaDosen = "Dosen Baru",
                Email = "baru@kampus.ac.id",
                Prodi = "IF"
            };

            // Act
            var result = await controller.Create(newDto);

            // Assert
            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        [Fact]
        public async Task Delete_DosenWithMahasiswa_ReturnsConflict()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var dosen = new Dosen { DosenId = 1, Nidn = "051111", NamaDosen = "Dosen Wali", Email = "wali@kampus.ac.id", Prodi = "SI" };
            var mhs = new Mahasiswa { MahasiswaId = 1, Nim = "7223001", NamaMahasiswa = "Mhs Bimbingan", Email = "mhs@student.ac.id", Angkatan = 2023, DosenId = 1 };

            context.Dosens.Add(dosen);
            context.Mahasiswas.Add(mhs);
            await context.SaveChangesAsync();

            var controller = new DosenController(context);

            // Act (Coba hapus dosen yang masih punya mahasiswa bimbingan)
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
        }
    }
}