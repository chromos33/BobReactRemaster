using System;
using System.Text;
using BobReactRemaster.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using BobReactRemaster.Data;
using BobReactRemaster.EventBus;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.Services;
using BobReactRemaster.Services.Chat;
using BobReactRemaster.Services.Chat.Commands;
using BobReactRemaster.Services.Chat.Discord;
using BobReactRemaster.Services.Chat.Twitch;
using BobReactRemaster.Services.Scheduler;
using BobReactRemaster.Services.Stream;
using BobReactRemaster.SettingsOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace BobReactRemaster
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), mysqlOptions =>
                 {
                     mysqlOptions.ServerVersion(new Version(10, 3, 8), ServerType.MariaDb);
                 })
             );
            //services.AddResponseCompression();
            //services.AddResponseCaching();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
                config.AddPolicy(Policies.User, Policies.UserPolicy());
            });
            services.Configure<WebServerSettingsOptions>(Configuration.GetSection(WebServerSettingsOptions.Position));
            
            services.AddSingleton<IMessageBus, MessageBus>();
            services.AddSingleton<IHostedService,SchedulerService>();
            services.AddSingleton<IUserRegistrationService, UserRegistrationService>();
            services.AddSingleton<IHostedService,DiscordChat>();
            services.AddSingleton<IHostedService,TwitchChat>();
            services.AddSingleton<IHostedService,StreamCheckerService>();
            services.AddSingleton<IRelayService, RelayService>();
            services.AddSingleton<IHostedService, CommandCenter>();
            services.AddSingleton<SubscriptionService, SubscriptionService>();


            //services.AddControllersWithViews();
            services.AddRazorPages();
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseStaticFiles();
                app.UseSpaStaticFiles();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                var cachePeriod = "604800";
                var StaticFileOptions = new StaticFileOptions()
                {
                    OnPrepareResponse = ctx =>
                    {
                        ctx.Context.Response.Headers.Append("Cache-Control", $"public,max-age={cachePeriod}");
                    }
                };
                app.UseStaticFiles(StaticFileOptions);
                app.UseSpaStaticFiles(StaticFileOptions);
            }

            app.UseHttpsRedirection();
            

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
