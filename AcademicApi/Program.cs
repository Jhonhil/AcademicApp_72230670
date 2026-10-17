using AcademicApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<AcademicDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Routing agar URL di Swagger & API otomatis lowercase (/api/dosen & /api/mahasiswa)
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistem Informasi Akademik API",
        Version = "v1",
        Description = "Layanan REST API CRUD Mahasiswa dan Dosen — Tugas Pengganti UTS Pemrograman Mobile 2026.\n\n" +
                      "**Nama:** Yohanes Hilapok | **NIM:** 72230670 | **Prodi:** Sistem Informasi",
        Contact = new OpenApiContact
        {
            Name = "Yohanes Hilapok (72230670)",
            Email = "yohaneshilpaok0@gmail.com"
        }
    });
});

// Terapkan CORS agar Blazor Hybrid / HttpClient bisa mengakses API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Jalankan Migration otomatis saat aplikasi dimulai
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AcademicDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();