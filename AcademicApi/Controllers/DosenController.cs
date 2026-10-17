using AcademicApi.Data;
using AcademicApi.DTOs;
using AcademicApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DosenController : ControllerBase
    {
        private readonly AcademicDbContext _context;

        public DosenController(AcademicDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DosenReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DosenReadDto>>> GetAll()
        {
            var list = await _context.Dosens
                .Select(d => new DosenReadDto
                {
                    DosenId = d.DosenId,
                    Nidn = d.Nidn,
                    NamaDosen = d.NamaDosen,
                    Email = d.Email,
                    Prodi = d.Prodi
                }).ToListAsync();

            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DosenReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DosenReadDto>> GetById(int id)
        {
            var d = await _context.Dosens.FindAsync(id);
            if (d == null) return NotFound(new { message = $"Dosen dengan ID {id} tidak ditemukan." });

            return Ok(new DosenReadDto
            {
                DosenId = d.DosenId,
                Nidn = d.Nidn,
                NamaDosen = d.NamaDosen,
                Email = d.Email,
                Prodi = d.Prodi
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(DosenReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<DosenReadDto>> Create([FromBody] DosenCreateUpdateDto dto)
        {
            if (await _context.Dosens.AnyAsync(d => d.Nidn == dto.Nidn))
                return Conflict(new { message = $"NIDN '{dto.Nidn}' sudah digunakan oleh dosen lain." });

            var dosen = new Dosen
            {
                Nidn = dto.Nidn,
                NamaDosen = dto.NamaDosen,
                Email = dto.Email,
                Prodi = dto.Prodi
            };

            _context.Dosens.Add(dosen);
            await _context.SaveChangesAsync();

            var result = new DosenReadDto
            {
                DosenId = dosen.DosenId,
                Nidn = dosen.Nidn,
                NamaDosen = dosen.NamaDosen,
                Email = dosen.Email,
                Prodi = dosen.Prodi
            };

            return CreatedAtAction(nameof(GetById), new { id = dosen.DosenId }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DosenReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] DosenCreateUpdateDto dto)
        {
            var dosen = await _context.Dosens.FindAsync(id);
            if (dosen == null) return NotFound(new { message = $"Dosen dengan ID {id} tidak ditemukan." });

            if (await _context.Dosens.AnyAsync(d => d.Nidn == dto.Nidn && d.DosenId != id))
                return Conflict(new { message = $"NIDN '{dto.Nidn}' sudah digunakan oleh dosen lain." });

            dosen.Nidn = dto.Nidn;
            dosen.NamaDosen = dto.NamaDosen;
            dosen.Email = dto.Email;
            dosen.Prodi = dto.Prodi;

            await _context.SaveChangesAsync();
            return Ok(new DosenReadDto
            {
                DosenId = dosen.DosenId,
                Nidn = dosen.Nidn,
                NamaDosen = dosen.NamaDosen,
                Email = dosen.Email,
                Prodi = dosen.Prodi
            });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(int id)
        {
            var dosen = await _context.Dosens.FindAsync(id);
            if (dosen == null) return NotFound(new { message = $"Dosen dengan ID {id} tidak ditemukan." });

            bool memimpinMahasiswa = await _context.Mahasiswas.AnyAsync(m => m.DosenId == id);
            if (memimpinMahasiswa)
            {
                return Conflict(new { message = "Gagal menghapus! Dosen ini masih terdaftar sebagai dosen wali untuk beberapa mahasiswa." });
            }

            _context.Dosens.Remove(dosen);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}