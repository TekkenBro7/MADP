using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Web_253505_Sniazhko.UI;
using Web_253505_Sniazhko.UI.ApiInteraction;
using Web_253505_Sniazhko.UI.ApiInteraction.Services;
using Web_253505_Sniazhko.UI.Extensions;
using Web_253505_Sniazhko.UI.HelperClasses;
using Web_253505_Sniazhko.UI.Services.CategoryService;
using Web_253505_Sniazhko.UI.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.RegisterCustomServices(uriData);






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapRazorPages();

app.Run();
