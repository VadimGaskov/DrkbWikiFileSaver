using System.Reflection;
using System.Text.Json.Serialization;
using Amazon.S3;
using DrkbWikiFileSaver.Application.Interfaces;
using DrkbWikiFileSaver.Application.Interfaces.Configurations;
using DrkbWikiFileSaver.Application.Mapper;
using DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveFile;
using DrkbWikiFileSaver.Application.UseCases.Video.Commands.SaveVideo;
using DrkbWikiFileSaver.Domain.Interfaces;
using DrkbWikiFileSaver.Infrastructure;
using DrkbWikiFileSaver.Infrastructure.Configuration;
using DrkbWikiFileSaver.Infrastructure.Data;
using DrkbWikiFileSaver.Infrastructure.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
IConfiguration Configuration = builder.Configuration;
builder.Services.AddSwaggerGen(options =>
{
    //options.CustomSchemaIds(x => x.Name);

    options.SwaggerDoc("v1", new OpenApiInfo { Title = "DrkbWikiFileSaver", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    
    OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
    {
        Name = "Bearer",
        BearerFormat = "JWT",
        Scheme = "bearer",
        Description = "Specify the authorization token.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
    };

    options.AddSecurityDefinition("jwt_auth", securityDefinition);

    OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference()
        {
            Id = "jwt_auth",
            Type = ReferenceType.SecurityScheme
        }
    };

    OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
    {
        {securityScheme, new string[] { }},
    };

    options.AddSecurityRequirement(securityRequirements);
});

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

var applicationAssembly = AppDomain.CurrentDomain.GetAssemblies()
    .FirstOrDefault(a => a.GetName().Name == "DrkbWikiFileSaver.Application");

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(SaveVideoCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(SaveFileCommand).Assembly);
});

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
    options.MultipartBodyLengthLimit = 3L * 1024 * 1024 * 1024; // 3 ГБ
});



builder.Services.Configure<VideoSettings>(builder.Configuration.GetSection("VideoSettings"));
builder.Services.AddTransient<IVideoConfiguration, VideoConfiguration>();

//MAPPER
builder.Services.AddAutoMapper(typeof(Mapper));

//S3
builder.Services.Configure<SelectelStorageConfiguration>(
    builder.Configuration.GetSection("S3Storage"));
builder.Services.AddSingleton<ISelectelStorageConfiguration>(sp =>
    sp.GetRequiredService<IOptions<SelectelStorageConfiguration>>().Value);
builder.Services.AddTransient<IObjectStorageService, UploadSelectel>();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5004); // Укажите нужный порт
    serverOptions.Limits.MaxRequestBodySize = 3L * 1024 * 1024 * 1024; // 3 ГБ
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "DrkbWiki.Presentation");
            options.RoutePrefix = string.Empty;
        });
}

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