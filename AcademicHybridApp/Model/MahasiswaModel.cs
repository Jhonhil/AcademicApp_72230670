using System.ComponentModel.DataAnnotations;

namespace AcademicHybridApp.Models
{
    public class MahasiswaModel
    {
        public int MahasiswaId { get; set; }

        [Required(ErrorMessage = "NIM wajib diisi.")]
        [StringLength(20, ErrorMessage = "NIM maksimal 20 karakter.")]
        public string Nim { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nama Mahasiswa wajib diisi.")]
        [StringLength(100, ErrorMessage = "Nama Mahasiswa maksimal 100 karakter.")]
        public string NamaMahasiswa { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email wajib diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Angkatan wajib diisi.")]
        [Range(2020, 2026, ErrorMessage = "Angkatan harus antara tahun 2020 - 2026.")]
        public int Angkatan { get; set; } = 2023;

        [Required(ErrorMessage = "Dosen Wali wajib dipilih.")]
        [Range(1, int.MaxValue, ErrorMessage = "Silakan pilih Dosen Wali.")]
        public int DosenId { get; set; }

        public string NamaDosenWali { get; set; } = string.Empty;
    }
}