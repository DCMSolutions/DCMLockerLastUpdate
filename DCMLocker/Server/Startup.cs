using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;
using DCMLocker.Server.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using DCMLocker.Server.BaseController;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace DCMLocker.Server
{
    public class Startup
    {
        public TBaseLockerController BaseController;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            BaseController = new TBaseLockerController(System.IO.Directory.GetCurrentDirectory() + "\\Base");
            BaseController.Upgrade();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<TBaseLockerController>(BaseController);

            //services.Configure<ForwardedHeadersOptions>(options =>
            //{
            //    options.KnownProxies.Add(IPAddress.Parse("127.0.0.1"));
            //    options.KnownProxies.Add(IPAddress.Parse("192.168.0.5"));
            //});

            // SignalR
            services.AddSignalR();

            services.AddControllersWithViews();
            services.AddRazorPages();

            // SignalR
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            //JWTBeare
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,  // emisor
                    ValidateAudience = true, // Destino
                    ValidAudience = Configuration["AuthSettings:Audience"], // valida de donde viene el token y si es correcto lo usa
                    ValidIssuer = Configuration["AuthSettings:Issuer"],
                    RequireExpirationTime = false, //true,
                    ValidateLifetime = false,//true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["AuthSettings:key"]))
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // SignalR
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.MapWhen(ctx => ctx.Request.Host.Port == 5021 || ctx.Request.Host.Port == 5020 ||
               ctx.Request.Host.Equals("Locker.com"), first =>
               {
                   first.Use((ctx, nxt) =>
                   {
                       ctx.Request.Path = "/ClientApp" + ctx.Request.Path;
                       return nxt();
                   });

                   first.UseBlazorFrameworkFiles("/ClientApp");

                    // Se debe de agregar la ruta fisica para habilitar los archivos estaticos en las imagenes
                    first.UseStaticFiles();
                   first.UseStaticFiles("/ClientApp");
                   first.UseStaticFiles(
                       new StaticFileOptions
                       {
                           FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Content")),
                           RequestPath = "/ClientApp/Content"
                       });

                   first.UseRouting();
                   first.UseAuthentication();
                   first.UseAuthorization();

                   first.UseEndpoints(endpoints =>
                   {
                       endpoints.MapRazorPages();
                       endpoints.MapControllers();
                       endpoints.MapHub<LockerHub>("/ClientApp/LockerHub");
                       endpoints.MapFallbackToFile("/ClientApp/{*path:nonfile}",
                       "ClientApp/index.html");
                   });
               });

            app.MapWhen(ctx => ctx.Request.Host.Port == 5024 || ctx.Request.Host.Port == 5025 ||
               ctx.Request.Host.Equals("LockerMonitor.com"), Monitor =>
               {
                   Monitor.Use((ctx, nxt) =>
                   {
                       ctx.Request.Path = "/MonitorApp" + ctx.Request.Path;
                       return nxt();
                   });

                   Monitor.UseBlazorFrameworkFiles("/MonitorApp");

                   // Se debe de agregar la ruta fisica para habilitar los archivos estaticos en las imagenes
                   Monitor.UseStaticFiles();
                   Monitor.UseStaticFiles("/MonitorApp");
                   Monitor.UseStaticFiles(
                       new StaticFileOptions
                       {
                           FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Content")),
                           RequestPath = "/MonitorApp/Content"
                       });

                   Monitor.UseRouting();
                   Monitor.UseAuthentication();
                   Monitor.UseAuthorization();

                   Monitor.UseEndpoints(endpoints =>
                   {
                       endpoints.MapRazorPages();
                       endpoints.MapControllers();
                       endpoints.MapHub<LockerHub>("/MonitorApp/LockerHub");
                       endpoints.MapFallbackToFile("/MonitorApp/{*path:nonfile}",
                       "MonitorApp/index.html");
                   });
               });

            app.MapWhen(ctx => ctx.Request.Host.Port == 5023 || ctx.Request.Host.Port == 5022 ||
                ctx.Request.Host.Equals("lockerkiosk.com"), second =>
                {
                    second.Use((ctx, nxt) =>
                    {
                        ctx.Request.Path = "/KioskApp" + ctx.Request.Path;
                        return nxt();
                    });

                    second.UseBlazorFrameworkFiles("/KioskApp");
                    second.UseStaticFiles();
                    second.UseStaticFiles("/KioskApp");
                    second.UseStaticFiles(
                        new StaticFileOptions
                        {
                            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Content")),
                            RequestPath = "/KioskApp/Content"
                        });
                    second.UseRouting();

                    second.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                        endpoints.MapFallbackToFile("/KioskApp/{*path:nonfile}",
                        "KioskApp/index.html");
                    });
                });
            






            /*
            // app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<LockerHub>("/LockerHub");
                endpoints.MapFallbackToFile("index.html");
            });
            */
        }
    }
}
