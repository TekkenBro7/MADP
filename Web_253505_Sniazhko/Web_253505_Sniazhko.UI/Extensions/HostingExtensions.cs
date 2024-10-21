using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.UI.ApiInteraction;
using Web_253505_Sniazhko.UI.ApiInteraction.Services;
using Web_253505_Sniazhko.UI.Controllers;
using Web_253505_Sniazhko.UI.HelperClasses;
using Web_253505_Sniazhko.UI.Services.Authentication;
using Web_253505_Sniazhko.UI.Services.Authorization;
using Web_253505_Sniazhko.UI.Services.CategoryService;
using Web_253505_Sniazhko.UI.Services.FileService;
using Web_253505_Sniazhko.UI.Services.ProductService;
using Web_253505_Sniazhko.UI.ViewComponents;

namespace Web_253505_Sniazhko.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder, UriData? uriData)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
            builder.Services.AddScoped<IProductService, MemoryProductService>();
            builder.Services
            .AddHttpClient<IProductService, ApiProductService>(opt =>
            opt.BaseAddress = new Uri(uriData.ApiUri));
            builder.Services
                .AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
                opt.BaseAddress = new Uri(uriData.ApiUri));
            builder.Services.AddHttpClient<IFileService, ApiFileService>(opt => opt.BaseAddress = new Uri($"{uriData.ApiUri}Files"));
            builder.Services.AddHttpContextAccessor();
            builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
            var keycloakData = builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddJwtBearer()
            .AddOpenIdConnect(options =>
            {
                options.Authority = $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
                options.ClientId = keycloakData.ClientId;
                options.ClientSecret = keycloakData.ClientSecret;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.Scope.Add("openid"); // Customize scopes as needed
                options.SaveTokens = true;
                options.RequireHttpsMetadata = false; // позволяет обращаться к локальному Keycloak по http
                options.MetadataAddress = $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
            });
            builder.Services.AddScoped<CartViewComponent>();

            builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();
            builder.Services.AddScoped<IAuthService, KeycloakAuthService>();
            
            builder.Services.AddDistributedMemoryCache();       
            builder.Services.AddSession();


            builder.Services.AddScoped<CartContainer, SessionCart>();
        }
    }
}
