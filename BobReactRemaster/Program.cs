using BobReactRemaster.Data;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus;
using BobReactRemaster.Services;
using BobReactRemaster.Services.Chat;
using BobReactRemaster.Services.Chat.Commands;
using BobReactRemaster.Services.Chat.Discord;
using BobReactRemaster.Services.Chat.Twitch;
using BobReactRemaster.Services.Scheduler;
using BobReactRemaster.Services.Stream;
using BobReactRemaster.SettingsOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using BobReactRemaster.Auth;
using BobReactRemaster.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(10, 3, 8));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mysqloptions =>
    {
        mysqloptions.EnableStringComparisonTranslations();
    })
    .LogTo(Console.WriteLine, LogLevel.Error);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
    config.AddPolicy(Policies.User, Policies.UserPolicy());
});

builder.Services.Configure<WebServerSettingsOptions>(builder.Configuration.GetSection(WebServerSettingsOptions.Position));
builder.Services.AddControllers(o => o.InputFormatters.Insert(o.InputFormatters.Count, new TextPlainInputFormatter()));
builder.Services.AddSingleton<IMessageBus, MessageBus>();
builder.Services.AddSingleton<IHostedService, SchedulerService>();
builder.Services.AddSingleton<IUserRegistrationService, UserRegistrationService>();
builder.Services.AddSingleton<IHostedService, DiscordChat>();
builder.Services.AddSingleton<IHostedService, TwitchChat>();
builder.Services.AddSingleton<IHostedService, StreamCheckerService>();
builder.Services.AddSingleton<IRelayService, RelayService>();
builder.Services.AddSingleton<IHostedService, CommandCenter>();
builder.Services.AddSingleton<SubscriptionService, SubscriptionService>();

builder.Services.AddRazorPages();
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/build";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseStaticFiles();
    app.UseSpaStaticFiles();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    var cachePeriod = builder.Configuration["StaticFiles:CachePeriod"];
    var staticFileOptions = new StaticFileOptions()
    {
        OnPrepareResponse = ctx =>
        {
            ctx.Context.Response.Headers.Append("Cache-Control", $"public,max-age={cachePeriod}");
        }
    };
    app.UseStaticFiles(staticFileOptions);
    app.UseSpaStaticFiles(staticFileOptions);
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

    if (app.Environment.IsDevelopment())
    {
        spa.UseReactDevelopmentServer(npmScript: "start");
    }
});

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
try
{
    await ApplicationDBInitializer.SeedUsers(scopeFactory);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while seeding the database.");
}

app.Run();