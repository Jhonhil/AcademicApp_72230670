using AcademicApi.Controllers;
using AcademicApi.Data;
using AcademicApi.DTOs;
using AcademicApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AcademicApi.Tests
{
    public class MahasiswaControllerTests
    {
        private AcademicDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AcademicDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AcademicDbContext(options);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithMahasiswaListAndDosenWali()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var dosen = new Dosen { DosenId = 1, Nidn = "051111", NamaDosen = "Dr. Anita Wijaya", Email = "anita@kampus.ac.id", Prodi = "SI" };
            var mhs = new Mahasiswa { MahasiswaId = 1, Nim = "72230001", NamaMahasiswa = "Budi", Email = "budi@student.ac.id", Angkatan = 2023, DosenId = 1, DosenWali = dosen };
            context.Dosens.Add(dosen);
            context.Mahasiswas.Add(mhs);
            await context.SaveChangesAsync();

            var controller = new MahasiswaController(context);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var list = Assert.IsAssignableFrom<IEnumerable<MahasiswaReadDto>>(okResult.Value);
            var item = Assert.Single(list);
            Assert.Equal("Dr. Anita Wijaya", item.NamaDosenWali);
        }

        [Fact]
        public async Task Create_ValidMahasiswa_ReturnsCreatedAtAction()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var dosen = new Dosen { DosenId = 1, Nidn = "051111", NamaDosen = "Dr. Anita Wijaya", Email = "anita@kampus.ac.id", Prodi = "SI" };
            context.Dosens.Add(dosen);
            await context.SaveChangesAsync();

            var controller = new MahasiswaController(context);
            var dto = new MahasiswaCreateUpdateDto
            {
                Nim = "72230001",
                NamaMahasiswa = "Budi Santoso",
                Email = "budi@student.kampus.ac.id",
                Angkatan = 2023,
                DosenId = 1
            };

            // Act
            var result = await controller.Create(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var model = Assert.IsType<MahasiswaReadDto>(createdResult.Value);
            Assert.Equal("72230001", model.Nim);
            Assert.Equal("Dr. Anita Wijaya", model.NamaDosenWali);
        }

        [Fact]
        public async Task Create_DuplicateNim_ReturnsConflict()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var dosen = new Dosen { DosenId = 1, Nidn = "051111", NamaDosen = "Dr. Anita", Email = "anita@kampus.ac.id", Prodi = "SI" };
            var mhs = new Mahasiswa { MahasiswaId = 1, Nim = "72230001", NamaMahasiswa = "Existing", Email = "ex@student.ac.id", Angkatan = 2023, DosenId = 1 };
            context.Dosens.Add(dosen);
            context.Mahasiswas.Add(mhs);
            await context.SaveChangesAsync();

            var controller = new MahasiswaController(context);
            var dto = new MahasiswaCreateUpdateDto
            {
                Nim = "72230001", // Duplicate NIM
                NamaMahasiswa = "New Student",
                Email = "new@student.ac.id",
                Angkatan = 2023,
                DosenId = 1
            };

            // Act
            var result = await controller.Create(dto);

            // Assert
            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        [Fact]
        public async Task Create_InvalidDosenId_ReturnsBadRequest()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new MahasiswaController(context);
            var dto = new MahasiswaCreateUpdateDto
            {
                Nim = "72230002",
                NamaMahasiswa = "New Student",
                Email = "new@student.ac.id",
                Angkatan = 2023,
                DosenId = 999 // Dosen tidak ada
            };

            // Act
            var result = await controller.Create(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Delete_ExistingMahasiswa_ReturnsNoContent()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var dosen = new Dosen { DosenId = 1, Nidn = "051111", NamaDosen = "Dr. Anita", Email = "anita@kampus.ac.id", Prodi = "SI" };
            var mhs = new Mahasiswa { MahasiswaId = 1, Nim = "72230001", NamaMahasiswa = "Budi", Email = "budi@student.ac.id", Angkatan = 2023, DosenId = 1 };
            context.Dosens.Add(dosen);
            context.Mahasiswas.Add(mhs);
            await context.SaveChangesAsync();

            var controller = new MahasiswaController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Empty(await context.Mahasiswas.ToListAsync());
        }
    }
}
