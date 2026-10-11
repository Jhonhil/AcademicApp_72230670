using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicApi.Models
{
    public class Mahasiswa
    {
        [Key]
        public int MahasiswaId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Nim { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string NamaMahasiswa { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(2020, 2026)]
        public int Angkatan { get; set; }

        [Required]
        public int DosenId { get; set; }

        [ForeignKey("DosenId")]
        public Dosen? DosenWali { get; set; }
    }
}