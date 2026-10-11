using AcademicApi.Data;
using AcademicApi.DTOs;
using AcademicApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MahasiswaController : ControllerBase
    {
        private readonly AcademicDbContext _context;

        public MahasiswaController(AcademicDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MahasiswaReadDto>>> GetAll()
        {
            var list = await _context.Mahasiswas
                .Include(m => m.DosenWali)
                .Select(m => new MahasiswaReadDto
                {
                    MahasiswaId = m.MahasiswaId,
                    Nim = m.Nim,
                    NamaMahasiswa = m.NamaMahasiswa,
                    Email = m.Email,
                    Angkatan = m.Angkatan,
                    DosenId = m.DosenId,
                    NamaDosenWali = m.DosenWali != null ? m.DosenWali.NamaDosen : "-"
                }).ToListAsync();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MahasiswaReadDto>> GetById(int id)
        {
            var m = await _context.Mahasiswas.Include(x => x.DosenWali).FirstOrDefaultAsync(x => x.MahasiswaId == id);
            if (m == null) return NotFound(new { message = $"Mahasiswa dengan ID {id} tidak ditemukan." });

            return Ok(new MahasiswaReadDto
            {
                MahasiswaId = m.MahasiswaId,
                Nim = m.Nim,
                NamaMahasiswa = m.NamaMahasiswa,
                Email = m.Email,
                Angkatan = m.Angkatan,
                DosenId = m.DosenId,
                NamaDosenWali = m.DosenWali != null ? m.DosenWali.NamaDosen : "-"
            });
        }

        [HttpPost]
        public async Task<ActionResult<MahasiswaReadDto>> Create([FromBody] MahasiswaCreateUpdateDto dto)
        {
            if (await _context.Mahasiswas.AnyAsync(m => m.Nim == dto.Nim))
                return Conflict(new { message = $"NIM '{dto.Nim}' sudah terdaftar." });

            var dosenExists = await _context.Dosens.AnyAsync(d => d.DosenId == dto.DosenId);
            if (!dosenExists)
                return BadRequest(new { message = "Dosen wali yang dipilih tidak valid." });

            var m = new Mahasiswa
            {
                Nim = dto.Nim,
                NamaMahasiswa = dto.NamaMahasiswa,
                Email = dto.Email,
                Angkatan = dto.Angkatan,
                DosenId = dto.DosenId
            };

            _context.Mahasiswas.Add(m);
            await _context.SaveChangesAsync();

            var dosen = await _context.Dosens.FindAsync(dto.DosenId);

            var result = new MahasiswaReadDto
            {
                MahasiswaId = m.MahasiswaId,
                Nim = m.Nim,
                NamaMahasiswa = m.NamaMahasiswa,
                Email = m.Email,
                Angkatan = m.Angkatan,
                DosenId = m.DosenId,
                NamaDosenWali = dosen?.NamaDosen ?? "-"
            };

            return CreatedAtAction(nameof(GetById), new { id = m.MahasiswaId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MahasiswaCreateUpdateDto dto)
        {
            var m = await _context.Mahasiswas.FindAsync(id);
            if (m == null) return NotFound(new { message = $"Mahasiswa dengan ID {id} tidak ditemukan." });

            if (await _context.Mahasiswas.AnyAsync(x => x.Nim == dto.Nim && x.MahasiswaId != id))
                return Conflict(new { message = $"NIM '{dto.Nim}' telah digunakan oleh mahasiswa lain." });

            var dosenExists = await _context.Dosens.AnyAsync(d => d.DosenId == dto.DosenId);
            if (!dosenExists)
                return BadRequest(new { message = "Dosen wali yang dipilih tidak valid." });

            m.Nim = dto.Nim;
            m.NamaMahasiswa = dto.NamaMahasiswa;
            m.Email = dto.Email;
            m.Angkatan = dto.Angkatan;
            m.DosenId = dto.DosenId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var m = await _context.Mahasiswas.FindAsync(id);
            if (m == null) return NotFound(new { message = $"Mahasiswa dengan ID {id} tidak ditemukan." });

            _context.Mahasiswas.Remove(m);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}