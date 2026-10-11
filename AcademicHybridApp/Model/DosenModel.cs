using System.ComponentModel.DataAnnotations;

namespace AcademicHybridApp.Models
{
    public class DosenModel
    {
        public int DosenId { get; set; }

        [Required(ErrorMessage = "NIDN wajib diisi.")]
        [StringLength(20, ErrorMessage = "NIDN maksimal 20 karakter.")]
        public string Nidn { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nama Dosen wajib diisi.")]
        [StringLength(100, ErrorMessage = "Nama Dosen maksimal 100 karakter.")]
        public string NamaDosen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email wajib diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prodi wajib diisi.")]
        public string Prodi { get; set; } = string.Empty;
    }
}