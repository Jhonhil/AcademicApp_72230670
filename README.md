# 🎓 Sistem Informasi Akademik Sederhana
> **Tugas Pengganti UTS — Pemrograman Mobile (2026)**  
> Implementasi Aplikasi Client-Server Berbasis Blazor Hybrid (.NET MAUI) dan ASP.NET Core Web API dengan Entity Framework Core & SQLite.

---

## 👤 Identitas Mahasiswa & Repositori

* **Nama Mahasiswa:** Yohanes Hilapok
* **NIM:** 72230670
* **Mata Kuliah:** Pemrograman Mobile
* **Tautan Repositori Git:** [https://github.com/Jhonhil/AcademicApp_72230670](https://github.com/Jhonhil/AcademicApp_72230670)

---

## 🏛️ Arsitektur Aplikasi & Gambaran Umum

Aplikasi ini dibangun menggunakan arsitektur **Client-Server** terpisah. Aplikasi Klien ( Blazor Hybrid / .NET MAUI) **tidak mengakses database secara langsung**, melainkan mengonsumsi layanan **ASP.NET Core Web API** melalui protokol **HTTP JSON**. Backend bertanggung jawab atas validasi, operasi CRUD, serta interaksi dengan SQLite menggunakan Entity Framework Core.

```text
+-------------------------------------------------------------+
|         Blazor Hybrid (.NET MAUI) - Klien                   |
|  - UI / Razor Components                                    |
|  - Navigasi & Form Validasi                                 |
|  - DosenService & MahasiswaService (HttpClient)             |
+-------------------------------------------------------------+
                              │
                    HTTP/JSON │ (REST API)
                              ▼
+-------------------------------------------------------------+
|         ASP.NET Core Web API - Backend                      |
|  - DosenController & MahasiswaController                    |
|  - Validasi Bisnis, Aturan Penghapusan, Status HTTP         |
|  - DTOs / LINQ Projection (Pencegahan Circular Reference)   |
+-------------------------------------------------------------+
                              │
                      EF Core │ (ORM / LINQ)
                              ▼
+-------------------------------------------------------------+
|         SQLite Database (academic.db)                       |
|  - Tabel Dosen & Tabel Mahasiswa                            |
+-------------------------------------------------------------+
```

---

## 📁 Struktur Solusi Proyek

```text
AcademicSystem/
├── AcademicApi/                         # Proyek Backend ASP.NET Core Web API
│   ├── Controllers/
│   │   ├── DosenController.cs           # Endpoint CRUD Dosen & Proteksi Dosen Wali
│   │   └── MahasiswaController.cs       # Endpoint CRUD Mahasiswa & Join Dosen Wali
│   ├── Data/
│   │   ├── AcademicDbContext.cs        # EF Core DbContext, Index Unik, & Seed Data
│   │   ├── academic.db                  # Database File SQLite
│   │   ├── academic.db-shm
│   │   └── academic.db-wal
│   ├── DTOs/
│   │   ├── DosenDtos.cs                 # DosenReadDto, DosenCreateUpdateDto
│   │   └── MahasiswaDtos.cs             # MahasiswaReadDto, MahasiswaCreateUpdateDto
│   ├── Model/
│   │   ├── Dosen.cs                     # Entitas Dosen
│   │   └── Mahasiswa.cs                 # Entitas Mahasiswa
│   ├── Properties/
│   │   └── launchSettings.json          # Konfigurasi Port HTTP (5057)
│   ├── appsettings.json                 # Connection String SQLite
│   └── Program.cs                       # Registrasi Service, CORS, EF Core, & Swagger
│
├── AcademicHybridApp/                   # Proyek Frontend .NET MAUI Blazor Hybrid
│   ├── Components/
│   │   ├── Layout/
│   │   │   ├── MainLayout.razor         # Layout Utama & Container Body
│   │   │   └── NavMenu.razor            # Menu Navigasi Beranda, Dosen, Mahasiswa
│   │   ├── Pages/
│   │   │   ├── Home.razor               # Dashboard / Halaman Utama
│   │   │   ├── Dosen/
│   │   │   │   ├── DosenList.razor      # Tabel Daftar Dosen & Aksi Hapus/Edit
│   │   │   │   └── DosenForm.razor      # Form Tambah/Edit Dosen + Validasi
│   │   │   └── Mahasiswa/
│   │   │       ├── MahasiswaList.razor  # Tabel Daftar Mahasiswa + Nama Dosen Wali
│   │   │       └── MahasiswaForm.razor  # Form Tambah/Edit + Dropdown Dosen API
│   │   └── _Imports.razor               # Global Using Directives (Models & Services)
│   ├── Model/
│   │   ├── DosenModel.cs                # Model Form & Validation Dosen
│   │   └── MahasiswaModel.cs            # Model Form & Validation Mahasiswa
│   ├── Services/
│   │   ├── DosenService.cs              # HttpClient Handler Modul Dosen
│   │   └── MahasiswaService.cs          # HttpClient Handler Modul Mahasiswa
│   └── MauiProgram.cs                   # Registrasi HttpClient BaseAddress (Port 5057)
│
└── AcademicApp.slnx                     # Solution File
```

---

## 🗄️ Model Data, Relasi & Aturan Bisnis

### 1. Entitas Dosen (`1`)
* **`DosenId`** (`int`): Primary Key, Identity/Auto Increment.
* **`Nidn`** (`string`): Wajib, Unik, Maksimal 20 karakter.
* **`NamaDosen`** (`string`): Wajib, Maksimal 100 karakter.
* **`Email`** (`string`): Wajib, Format Email Valid, Maksimal 100 karakter.
* **`Prodi`** (`string`): Wajib, Maksimal 100 karakter.

### 2. Entitas Mahasiswa (`N`)
* **`MahasiswaId`** (`int`): Primary Key, Identity/Auto Increment.
* **`Nim`** (`string`): Wajib, Unik, Maksimal 20 karakter.
* **`NamaMahasiswa`** (`string`): Wajib, Maksimal 100 karakter.
* **`Email`** (`string`): Wajib, Format Email Valid, Maksimal 100 karakter.
* **`Angkatan`** (`int`): Wajib, Rentang Nilai `2020` – `2026`.
* **`DosenId`** (`int`): Foreign Key, Wajib merujuk ke Dosen yang tersedia.

### 3. Aturan Bisnis & Aturan Penghapusan
* **Pencegahan Penghapusan Dosen Wali:** Data dosen yang masih digunakan sebagai Dosen Wali oleh minimal satu mahasiswa **tidak boleh dihapus**. API mengembalikan respons `409 Conflict` dengan pesan error yang jelas.
* **Circular Reference Handling:** Endpoint GET Mahasiswa memanfaatkan `MahasiswaReadDto` untuk memuat string `NamaDosenWali` tanpa menyebabkan *infinite loop* JSON.

---

## 🧪 Data Uji Bawaan (Seed Data)

Aplikasi secara otomatis melakukan *seeding* data awal pada database SQLite saat Backend API dijalankan pertama kali:

* **Data Dosen (3 Data):**
  1. `DosenId: 1` | **NIDN:** `0512345678` | **Nama:** Dr. Anita Wijaya | **Email:** `anita@kampus.ac.id` | **Prodi:** Sistem Informasi
  2. `DosenId: 2` | **NIDN:** `0587654321` | **Nama:** Budi Raharja, M.Kom. | **Email:** `budi.r@kampus.ac.id` | **Prodi:** Informatika
  3. `DosenId: 3` | **NIDN:** `0511223344` | **Nama:** Cintya Dewi, Ph.D. | **Email:** `cintya@kampus.ac.id` | **Prodi:** Sistem Informasi

* **Data Mahasiswa (5 Data):**
  1. `MahasiswaId: 1` | **NIM:** `72230001` | **Nama:** Budi Santoso | **Email:** `budi@student.kampus.ac.id` | **Angkatan:** 2023 | **Dosen Wali:** Dr. Anita Wijaya
  2. `MahasiswaId: 2` | **NIM:** `72230002` | **Nama:** Siti Aminah | **Email:** `siti@student.kampus.ac.id` | **Angkatan:** 2023 | **Dosen Wali:** Dr. Anita Wijaya
  3. `MahasiswaId: 3` | **NIM:** `72220001` | **Nama:** Andi Wijaya | **Email:** `andi@student.kampus.ac.id` | **Angkatan:** 2022 | **Dosen Wali:** Budi Raharja, M.Kom.
  4. `MahasiswaId: 4` | **NIM:** `72240001` | **Nama:** Dewi Lestari | **Email:** `dewi@student.kampus.ac.id` | **Angkatan:** 2024 | **Dosen Wali:** Budi Raharja, M.Kom.
  5. `MahasiswaId: 5` | **NIM:** `72230003` | **Nama:** Eko Prasetyo | **Email:** `eko@student.kampus.ac.id` | **Angkatan:** 2023 | **Dosen Wali:** Cintya Dewi, Ph.D.

---

## 📡 Dokumentasi Spesifikasi REST API Endpoint

### 1. Endpoint Dosen (`/api/dosen`)
| Metode | Endpoint | Fungsi | Status Sukses | Respons Error |
| :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/dosen` | Mengambil seluruh data dosen | `200 OK` | - |
| **GET** | `/api/dosen/{id}` | Mengambil detail dosen berdasarkan ID | `200 OK` | `404 Not Found` |
| **POST** | `/api/dosen` | Menambahkan dosen baru | `201 Created` | `400 Bad Request` / `409 Conflict` (NIDN Duplikat) |
| **PUT** | `/api/dosen/{id}` | Memperbarui data dosen | `200 OK` | `400 Bad Request` / `404 Not Found` / `409 Conflict` |
| **DELETE** | `/api/dosen/{id}` | Menghapus dosen jika tidak punya mahasiswa | `204 No Content` | `404 Not Found` / `409 Conflict` (Masih Jadi Wali) |

### 2. Endpoint Mahasiswa (`/api/mahasiswa`)
| Metode | Endpoint | Fungsi | Status Sukses | Respons Error |
| :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/mahasiswa` | Mengambil seluruh mahasiswa + Dosen Wali | `200 OK` | - |
| **GET** | `/api/mahasiswa/{id}` | Mengambil detail mahasiswa | `200 OK` | `404 Not Found` |
| **POST** | `/api/mahasiswa` | Menambahkan mahasiswa baru | `201 Created` | `400 Bad Request` / `409 Conflict` (NIM Duplikat) |
| **PUT** | `/api/mahasiswa/{id}` | Memperbarui mahasiswa & Dosen Wali | `204 No Content` | `400 Bad Request` / `404 Not Found` / `409 Conflict` |
| **DELETE** | `/api/mahasiswa/{id}` | Menghapus mahasiswa | `204 No Content` | `404 Not Found` |

---

## 🛠️ Prasyarat Sistem & Dependensi

* **IDE:** Visual Studio 2022 / 2026 dengan Workload:
  * `.NET MAUI Development`
  * `ASP.NET and web development`
* **SDK:** `.NET 8.0 SDK` atau lebih baru.
* **Package NuGet Backend (`AcademicApi`):**
  * `Microsoft.EntityFrameworkCore.Sqlite`
  * `Microsoft.EntityFrameworkCore.Tools`
  * `Swashbuckle.AspNetCore` (Swagger UI)

---

## ⚙️ Konfigurasi Connection String & Base Address

### 1. Backend (`AcademicApi/appsettings.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=academic.db"
  }
}
```

### 2. Frontend (`AcademicHybridApp/MauiProgram.cs`)
```csharp
// Base URL dikonfigurasi ke Port 5057
string apiBaseUrl = "http://localhost:5057/";

if (DeviceInfo.Platform == DevicePlatform.Android)
{
    apiBaseUrl = "http://10.0.2.2:5057/"; // IP localhost emulator Android
}

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
```

---

## 💻 Langkah-Langkah Menjalankan Proyek

### Cara 1: Menggunakan Visual Studio (Rekomendasi)

1. Buka file solusi `AcademicApp.slnx` di Visual Studio.
2. Di panel **Solution Explorer**, klik kanan pada nama Solution `AcademicApp` -> pilih **Configure Startup Projects...**
3. Pilih opsi **Multiple startup projects**:
   * Set `AcademicApi` ke **Start**
   * Set `AcademicHybridApp` ke **Start**
4. Klik **Apply** dan **OK**.
5. Tekan tombol **F5** (atau tombol **Start**).
6. Browser Swagger (`http://localhost:5057/swagger`) dan Aplikasi Blazor Hybrid akan terbuka dan terhubung secara otomatis.

### Cara 2: Menggunakan Terminal / PowerShell

1. **Jalankan Backend Web API:**
   ```powershell
   cd AcademicApi
   dotnet run
   ```
   *API akan aktif mendengarkan request di `http://localhost:5057`.*

2. **Jalankan Frontend Blazor Hybrid:**
   Buka jendela terminal baru:
   ```powershell
   cd AcademicHybridApp
   dotnet build
   dotnet run
   ```

---

## 📋 Hasil Skenario Pengujian Minimum (Test Cases)

| ID | Skenario Pengujian | Hasil Yang Diharapkan | Status |
| :---: | :--- | :--- | :---: |
| **T01** | Menambah dosen dengan seluruh data valid. | Data tersimpan di SQLite dan muncul pada tabel daftar dosen. | ✅ PASS |
| **T02** | Menambah dosen dengan email tidak valid. | Form menolak submit dan pesan error validasi tampil di bawah field. | ✅ PASS |
| **T03** | Menambah dosen dengan NIDN yang sama. | API merespons `409 Conflict` dan pesan peringatan duplikasi tampil. | ✅ PASS |
| **T04** | Menambah mahasiswa dan memilih dosen wali. | Data tersimpan dengan relasi FK `DosenId` yang sesuai. | ✅ PASS |
| **T05** | Mengubah dosen wali mahasiswa. | Dropdown diperbarui, daftar mahasiswa menampilkan nama dosen wali baru. | ✅ PASS |
| **T06** | Menghapus mahasiswa. | Data terhapus dari daftar UI dan tabel SQLite. | ✅ PASS |
| **T07** | Menghapus dosen yang masih menjadi wali. | API menolak delete (`409 Conflict`) & pesan peringatan muncul di Blazor. | ✅ PASS |
| **T08** | Mengakses ID yang tidak tersedia. | API mengembalikan status `404 Not Found`. | ✅ PASS |
| **T09** | API tidak aktif saat frontend dibuka. | UI menampilkan alert kegagalan koneksi tanpa crash/berhenti tidak terkontrol. | ✅ PASS |

---

## 🤖 Sumber & Alat Bantu Utama

1. **Dokumentasi Microsoft Learn:**
   * [ASP.NET Core Web API Endpoints & Swagger](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
   * [.NET MAUI Blazor Hybrid App Development](https://learn.microsoft.com/en-us/dotnet/maui/blazor/)
   * [Entity Framework Core SQLite Provider](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/)
2. **AI Tooling (Gemini AI):** Digunakan sebagai alat bantu konsultasi arsitektur DTO, pembuatan template validasi DataAnnotations, serta penyusunan struktur dokumentasi `README.md`.

---

### 🌟 Fitur Bonus yang Diterapkan (+10 Poin)
1. **Dashboard Ringkas (+2 Poin):** Menampilkan statistik total Dosen & Mahasiswa secara *real-time* di halaman utama `Home.razor`.
2. **Pencarian Mahasiswa (+3 Poin):** Fitur *Search Bar* instan berdasarkan NIM atau Nama Mahasiswa.
3. **Sorting / Pengurutan (+2 Poin):** Klik kolom tabel untuk mengurutkan data (*Ascending/Descending*).
4. **Unit Test Backend (+3 Poin):** Menggunakan `xUnit` & `EF Core In-Memory` pada proyek `AcademicApi.Tests` untuk menguji controller backend.

---

## Checklist Sebelum Pengumpulan

- [x] Backend dapat build dan berjalan tanpa error.
- [x] Swagger UI menampilkan seluruh endpoint CRUD Dosen dan Mahasiswa.
- [x] Database SQLite (`academic.db`) terintegrasi dan memiliki seed data awal.
- [x] Frontend terhubung ke Base URL API yang tepat (`http://localhost:5057/`).
- [x] Dropdown pilihan Dosen Wali pada form mahasiswa diperoleh dari API Dosen.
- [x] Proteksi hapus dosen yang masih bertugas sebagai Dosen Wali berfungsi.
- [x] Folder `bin` dan `obj` tidak ikut terunggah ke repositori Git (`.gitignore` aktif).
- [x] Dokumen `README.md` dan tautan repositori sudah lengkap.
