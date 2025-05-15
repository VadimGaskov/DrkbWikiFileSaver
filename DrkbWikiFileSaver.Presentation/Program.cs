using System.Text.Json.Serialization;
using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Domain.Interfaces;
using DrkbWikiFileSaver.Infrastructure;
using DrkbWikiFileSaver.Infrastructure.Data;
using DrkbWikiFileSaver.Infrastructure.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DrkbWikiFileSaverContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IFileSaver, DirectoryFileSaver>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", conf =>
    {
        conf.AllowAnyHeader();
        conf.AllowAnyOrigin();
        conf.AllowAnyMethod();
    });
});

//Увеличения лимита загрузки видео до 1.5гб
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1_500 * 1024 * 1024; // 1.5 ГБ
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 1_500 * 1024 * 1024; // 1.5 ГБ
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();


//TODO ПЕРЕДЕЛАТЬ!!!!
var documentPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
documentPath = Path.Combine(documentPath, "Documents");

if (!Directory.Exists(documentPath))
{
    Directory.CreateDirectory(documentPath);
}

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(documentPath), 
    RequestPath = "/documents"
});

var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
videoPath = Path.Combine(videoPath, "Video");

if (!Directory.Exists(videoPath))
{
    Directory.CreateDirectory(videoPath);
}

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(videoPath), 
    RequestPath = "/video"
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();