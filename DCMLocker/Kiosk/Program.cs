using DCMLocker.Kiosk.Authentication;
using DCMLocker.Kiosk.Cliente;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DCMLocker.Kiosk
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // ----------- AUTENTICACION -----------------
            // Se desarrollo CustomAuthorizationStateProvider
            // Se desarrollo funciones.js
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<MOFAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(op => op.GetRequiredService<MOFAuthenticationStateProvider>());
            //---------------------------------------------


            builder.RootComponents.Add<App>("#app");
            builder.Services.AddSingleton(s => new TLocker());
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<DCMLocker.Kiosk.Cliente.TLockerCliente>();
            builder.Services.AddScoped<DCMLocker.Kiosk.Authentication.TAuthCliente>();
            await builder.Build().RunAsync();
        }
    }
}
