using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Web_253505_Sniazhko.API.Data;
using Web_253505_Sniazhko.API.Models;
using Web_253505_Sniazhko.API.Services.CategoryService;
using Web_253505_Sniazhko.API.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authServer = builder.Configuration.GetSection("AuthServer").Get<AuthServerData>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                    // Адрес метаданных конфигурации OpenID
                    o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";
                    // Authority сервера аутентификации
                    o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";
                    // Audience для токена JWT
                    o.Audience = "account";
                    // Запретить HTTPS для использования локальной версии Keycloak
                    // В рабочем проекте должно быть true
                    o.RequireHttpsMetadata = false;
                });
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
});

var app = builder.Build();

await DbInitializer.SeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
