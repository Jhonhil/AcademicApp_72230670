using System.ComponentModel.DataAnnotations;

namespace AcademicApi.Models
{
    public class Dosen
    {
        [Key]
        public int DosenId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Nidn { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string NamaDosen { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Prodi { get; set; } = string.Empty;

        // Relasi 1-to-N: Dosen bisa menjadi wali dari banyak mahasiswa
        public ICollection<Mahasiswa> DaftarMahasiswa { get; set; } = new List<Mahasiswa>();
    }
}