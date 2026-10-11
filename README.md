# 🎓 Sistem Informasi Akademik Sederhana
### Tugas Pengganti UTS — Pemrograman Mobile (2026)

Aplikasi Sistem Informasi Akademik Sederhana berbasis arsitektur **Client-Server** yang memisahkan antarmuka pengguna (**Blazor Hybrid / .NET MAUI**) dan layanan data backend (**ASP.NET Core Web API** dengan **Entity Framework Core** dan **SQLite**).

---

## 👤 Identitas Mahasiswa

* **Nama:** Yohanes Hilapok
* **NIM:** 72230670
* **Mata Kuliah:** Pemrograman Mobile
* **Repositori GitHub:** [Jhonhil/AcademicApp_72230670](https://github.com/Jhonhil/AcademicApp_72230670)

---

## 🏗️ Arsitektur Aplikasi

Aplikasi dibangun dengan memisahkan tiga lapisan utama (*separation of concerns*):

```text
+--------------------------------------------------+
|           Blazor Hybrid (.NET MAUI)              |
|   (Antarmuka, Navigasi, Form Input & Validasi)   |
+--------------------------------------------------+
                        │  ▲
              HTTP/JSON │  │ HttpClient / Services
                        ▼  │
+--------------------------------------------------+
|              ASP.NET Core Web API                |
|   (Endpoints CRUD, Validasi Server, Rules/DTO)   |
+--------------------------------------------------+
                        │  ▲
          LINQ / Queries│  │ Entity Framework Core
                        ▼  │
+--------------------------------------------------+
|                 SQLite Database                  |
|                 (academic.db)                    |
+--------------------------------------------------+
```

* **Frontend (`AcademicHybridApp`):** Antarmuka pengguna Blazor Hybrid berbasis komponen Razor, mengelola form, validasi awal, indikator loading, serta komunikasi API via `HttpClient`.
* **Backend (`AcademicApi`):** Layanan Web API dengan Swagger UI, mengimplementasikan pengolahan data CRUD, pemetaan DTO untuk mencegah *circular reference*, serta validasi aturan bisnis.
* **Database (`SQLite`):** Media penyimpanan permanen relational untuk entitas Dosen dan Mahasiswa.

---

## 🚀 Fitur & Modul Utama

### 1. Modul Dosen
* **Daftar Dosen:** Menampilkan NIDN, Nama, Email, dan Program Studi.
* **Tambah & Edit Dosen:** Form input dengan validasi atribut NIDN unik, format email, dan field wajib.
* **Hapus Dosen:** Proteksi aturan bisnis — Dosen yang masih menjadi Dosen Wali bagi mahasiswa **tidak dapat dihapus** (`409 Conflict`).

### 2. Modul Mahasiswa
* **Daftar Mahasiswa:** Menampilkan NIM, Nama, Email, Angkatan, serta Nama Dosen Wali.
* **Tambah & Edit Mahasiswa:** Form input dengan dropdown Dosen Wali yang **diambil secara langsung dari API Dosen**. Validasi rentang angkatan (2020–2026) dan NIM unik.
* **Hapus Mahasiswa:** Menghapus data mahasiswa secara permanen dan memperbarui daftar secara otomatis.

---

## 🗄️ Model Data & Relasi

### Entitas Dosen (`1`)
| Atribut | Tipe Data | Ketentuan |
| :--- | :--- | :--- |
| `DosenId` | `int` | Primary Key, Auto Increment |
| `Nidn` | `string` | Wajib, Unik, Maksimal 20 karakter |
| `NamaDosen` | `string` | Wajib, Maksimal 100 karakter |
| `Email` | `string` | Wajib, Format Email Valid, Maksimal 100 karakter |
| `Prodi` | `string` | Wajib, Maksimal 100 karakter |

### Entitas Mahasiswa (`N`)
| Atribut | Tipe Data | Ketentuan |
| :--- | :--- | :--- |
| `MahasiswaId` | `int` | Primary Key, Auto Increment |
| `Nim` | `string` | Wajib, Unik, Maksimal 20 karakter |
| `NamaMahasiswa` | `string` | Wajib, Maksimal 100 karakter |
| `Email` | `string` | Wajib, Format Email Valid, Maksimal 100 karakter |
| `Angkatan` | `int` | Wajib, Rentang 2020–2026 |
| `DosenId` | `int` | Foreign Key, merujuk ke `Dosen.DosenId` |

---

## 🧪 Data Uji Bawaan (Seed Data)

Database SQLite terisi otomatis saat backend dijalankan pertama kali:

* **Dosen (3 Data):**
  1. `0512345678` — Dr. Anita Wijaya (Sistem Informasi)
  2. `0587654321` — Budi Raharja, M.Kom. (Informatika)
  3. `0511223344` — Cintya Dewi, Ph.D. (Sistem Informasi)

* **Mahasiswa (5 Data):**
  1. `72230001` — Budi Santoso (2023) — Wali: Dr. Anita Wijaya
  2. `72230002` — Siti Aminah (2023) — Wali: Dr. Anita Wijaya
  3. `72220001` — Andi Wijaya (2022) — Wali: Budi Raharja, M.Kom.
  4. `72240001` — Dewi Lestari (2024) — Wali: Budi Raharja, M.Kom.
  5. `72230003` — Eko Prasetyo (2023) — Wali: Cintya Dewi, Ph.D.

---

## 📡 Dokumentasi REST API Endpoint

### Endpoint Dosen (`/api/dosen`)
* `GET /api/dosen` — Mengambil seluruh data dosen (`200 OK`).
* `GET /api/dosen/{id}` — Mengambil detail satu dosen (`200 OK` / `404 Not Found`).
* `POST /api/dosen` — Menambah dosen baru (`201 Created` / `409 Conflict`).
* `PUT /api/dosen/{id}` — Memperbarui data dosen (`200 OK` / `400 Bad Request`).
* `DELETE /api/dosen/{id}` — Menghapus dosen (`204 No Content` / `409 Conflict`).

### Endpoint Mahasiswa (`/api/mahasiswa`)
* `GET /api/mahasiswa` — Mengambil seluruh mahasiswa beserta nama dosen wali (`200 OK`).
* `GET /api/mahasiswa/{id}` — Mengambil detail mahasiswa (`200 OK` / `404 Not Found`).
* `POST /api/mahasiswa` — Menambah mahasiswa baru (`201 Created` / `409 Conflict`).
* `PUT /api/mahasiswa/{id}` — Memperbarui data mahasiswa (`204 No Content` / `400 Bad Request`).
* `DELETE /api/mahasiswa/{id}` — Menghapus mahasiswa (`204 No Content`).

---

## 🛠️ Prasyarat Sistem

* **Visual Studio 2022 / 2026** dengan Workload:
  * `.NET MAUI Development`
  * `ASP.NET and web development`
* **.NET 8.0 SDK** atau versi yang lebih baru.
* **SQLite** (sudah terintegrasi otomatis via Entity Framework Core).

---

## ⚙️ Konfigurasi & Cara Menjalankan

### 1. Konfigurasi Connection String & Base Address

* **Backend (`AcademicApi/appsettings.json`):**
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=academic.db"
  }
  ```

* **Frontend (`AcademicHybridApp/MauiProgram.cs`):**
  ```csharp
  string apiBaseUrl = "http://localhost:5057/";
  if (DeviceInfo.Platform == DevicePlatform.Android)
  {
      apiBaseUrl = "http://10.0.2.2:5057/";
  }
  ```

### 2. Langkah Menjalankan via Visual Studio (Rekomendasi)

1. Buka file solusi `AcademicApp.slnx` di Visual Studio.
2. Di **Solution Explorer**, klik kanan pada Solution **`AcademicApp`** > pilih **Configure Startup Projects...**
3. Pilih **Multiple startup projects**:
   * Set `AcademicApi` ke **Start**
   * Set `AcademicHybridApp` ke **Start**
4. Tekan **F5** (atau klik tombol **Start**).
5. Swagger API akan terbuka di browser, dan aplikasi Blazor Hybrid akan terbuka di jendela baru.

### 3. Langkah Menjalankan via Terminal / CLI

**Langkah A — Jalankan Backend Web API:**
```powershell
cd AcademicApi
dotnet run
```
*API berjalan di `http://localhost:5057` / Swagger UI di `http://localhost:5057/swagger`.*

**Langkah B — Jalankan Frontend Blazor Hybrid:**
Buka jendela terminal baru:
```powershell
cd AcademicHybridApp
dotnet build
dotnet run
```

---

## 📋 Skenario Pengujian (Test Cases)

| ID | Skenario Pengujian | Hasil Yang Diharapkan | Status |
|---|---|---|---|
| **T01** | Menambah dosen dengan data valid | Data tersimpan dan muncul di daftar dosen | ✅ Lolos |
| **T02** | Menambah dosen dengan email tidak valid | Form menolak pengiriman data dan menampilkan pesan validasi | ✅ Lolos |
| **T03** | Menambah dosen dengan NIDN yang sama | API menolak duplikasi (`409 Conflict`) | ✅ Lolos |
| **T04** | Menambah mahasiswa dan memilih dosen wali | Data mahasiswa tersimpan beserta relasi dosen wali | ✅ Lolos |
| **T05** | Mengubah dosen wali mahasiswa | Pilihan dropdown diperbarui dan nama dosen wali pada daftar berubah | ✅ Lolos |
| **T06** | Menghapus mahasiswa | Data hilang dari daftar dan database | ✅ Lolos |
| **T07** | Menghapus dosen yang masih menjadi wali | Operasi ditolak API (`409 Conflict`) dengan pesan error informatif | ✅ Lolos |
| **T08** | Mengakses ID yang tidak tersedia | API mengembalikan status `404 Not Found` | ✅ Lolos |
| **T09** | API tidak aktif saat frontend dibuka | Aplikasi menampilkan alert kegagalan tanpa mengalami crash | ✅ Lolos |

---

## 🤖 Sumber & Alat Bantu

Dalam penyusunan proyek ini, alat bantu dan dokumentasi yang digunakan meliputi:
* **Dokumentasi Resmi Microsoft:** [.NET MAUI Blazor Hybrid Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/hybrid/) & [ASP.NET Core Web API](https://learn.microsoft.com/en-us/aspnet/core/web-api/).
* **Gemini AI:** Digunakan sebagai asisten pembimbing coding untuk perancangan struktur DTO, konsumsi HttpClient, serta penyusunan dokumentasi proyek.
