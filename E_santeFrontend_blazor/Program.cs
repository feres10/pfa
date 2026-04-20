using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using E_santeFrontend.Helpers;
using E_santeFrontend.Services;

namespace E_santeFrontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<LocalStorageService>();
            builder.Services.AddTransient<JwtAuthHandler>();

            // Configure HttpClient to call backend API via a named client
            // Default to backend HTTPS endpoint to avoid HTTP->HTTPS redirects
            var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7245"; // backend HTTPS

            builder.Services
                .AddHttpClient("ApiClient", client =>
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                })
                .AddHttpMessageHandler<JwtAuthHandler>();

            // Provide the configured client for DI where HttpClient is required
            builder.Services.AddScoped(sp =>
                sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<PatientService>();
            builder.Services.AddScoped<OrdonnanceService>();
            builder.Services.AddScoped<MedicamentService>();
            builder.Services.AddScoped<MedecinService>();
            builder.Services.AddScoped<AdminService>();
            builder.Services.AddScoped<StockService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<E_santeFrontend.Services.RendezvousService>();
            builder.Services.AddScoped<ConsultationService>();

            await builder.Build().RunAsync();
        }
    }
}
