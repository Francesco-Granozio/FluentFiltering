using FluentValidation;
using GameStore.Domain.Interfaces;
using GameStore.Application.Mapping;
using GameStore.Application.Services;
using GameStore.Infrastructure;
using GameStore.Infrastructure.Extensions;
using GameStore.Infrastructure.Interceptors;
using GameStore.Infrastructure.Repositories;
using GameStore.Infrastructure.Seeding;
using GameStore.WebUI.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// Add Radzen.Blazor services
builder.Services.AddRadzenComponents();

// Registrazione DbContext con interceptor per auditing e soft delete
builder.Services.AddScoped<AuditableEntitySaveChangesInterceptor>();
builder.Services.AddDbContext<ApplicationDbContext>((provider, options) =>
{
    var interceptor = provider.GetRequiredService<AuditableEntitySaveChangesInterceptor>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .AddInterceptors(interceptor);
});

// Registrazione del seeder
builder.Services.AddScoped<IDataSeeder, DatabaseSeeder>();

// Registrazione Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

// Registrazione AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

// Registrazione FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(GameStore.Application.Mapping.MappingProfile)));

// Registrazione Application services
builder.Services.AddScoped<IUtenteService, UtenteService>();
builder.Services.AddScoped<IGiocoService, GiocoService>();
builder.Services.AddScoped<IAcquistoService, AcquistoService>();
builder.Services.AddScoped<IRecensioneService, RecensioneService>();

// Registrazione API Controllers
builder.Services.AddControllers();

// Registrazione Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "GameStore API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Mappatura API Controllers
app.MapControllers();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Esegui il seeding del database se in ambiente di sviluppo
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    
    try
    {
        await seeder.SeedAsync();
        app.Logger.LogInformation("Seeding del database completato con successo.");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Errore durante il seeding del database.");
    }
}

app.Run();
