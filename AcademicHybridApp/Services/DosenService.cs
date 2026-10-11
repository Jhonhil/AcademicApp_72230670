using System.Net.Http.Json;
using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services
{
    public class DosenService
    {
        private readonly HttpClient _http;

        public DosenService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<DosenModel>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<DosenModel>>("api/dosen") ?? new List<DosenModel>();
        }

        public async Task<DosenModel?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<DosenModel>($"api/dosen/{id}");
        }

        public async Task<(bool Success, string Message)> CreateAsync(DosenModel dosen)
        {
            var response = await _http.PostAsJsonAsync("api/dosen", dosen);
            if (response.IsSuccessStatusCode) return (true, "Berhasil menambahkan data dosen.");

            var err = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            return (false, err != null && err.ContainsKey("message") ? err["message"] : "Gagal menyimpan data dosen.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(int id, DosenModel dosen)
        {
            var response = await _http.PutAsJsonAsync($"api/dosen/{id}", dosen);
            if (response.IsSuccessStatusCode) return (true, "Berhasil memperbarui data dosen.");

            var err = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            return (false, err != null && err.ContainsKey("message") ? err["message"] : "Gagal mengubah data dosen.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/dosen/{id}");
            if (response.IsSuccessStatusCode) return (true, "Berhasil menghapus dosen.");

            var err = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            return (false, err != null && err.ContainsKey("message") ? err["message"] : "Gagal menghapus dosen.");
        }
    }
}