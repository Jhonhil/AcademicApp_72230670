using System.Net.Http.Json;
using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services
{
    public class MahasiswaService
    {
        private readonly HttpClient _http;

        public MahasiswaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<MahasiswaModel>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<MahasiswaModel>>("api/mahasiswa") ?? new List<MahasiswaModel>();
        }

        public async Task<MahasiswaModel?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<MahasiswaModel>($"api/mahasiswa/{id}");
        }

        public async Task<(bool Success, string Message)> CreateAsync(MahasiswaModel m)
        {
            var response = await _http.PostAsJsonAsync("api/mahasiswa", m);
            if (response.IsSuccessStatusCode) return (true, "Berhasil menambahkan mahasiswa.");

            var err = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            return (false, err != null && err.ContainsKey("message") ? err["message"] : "Gagal menambahkan mahasiswa.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(int id, MahasiswaModel m)
        {
            var response = await _http.PutAsJsonAsync($"api/mahasiswa/{id}", m);
            if (response.IsSuccessStatusCode) return (true, "Berhasil mengubah data mahasiswa.");

            var err = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            return (false, err != null && err.ContainsKey("message") ? err["message"] : "Gagal mengubah data mahasiswa.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/mahasiswa/{id}");
            if (response.IsSuccessStatusCode) return (true, "Berhasil menghapus mahasiswa.");

            var err = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            return (false, err != null && err.ContainsKey("message") ? err["message"] : "Gagal menghapus mahasiswa.");
        }
    }
}