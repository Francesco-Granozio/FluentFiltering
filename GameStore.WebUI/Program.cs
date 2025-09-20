using FluentValidation;
using GameStore.Infrastructure;
using GameStore.Infrastructure.Extensions;
using GameStore.Infrastructure.Interceptors;
using GameStore.Infrastructure.Seeding;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add Radzen.Blazor services
builder.Services.AddRadzenComponents();


// Registrazione DbContext con interceptor per auditing e soft delete
builder.Services.AddScoped<AuditableEntitySaveChangesInterceptor>();

// Configurazione DbContext per Blazor Server
builder.Services.AddDbContext<ApplicationDbContext>((provider, options) =>
{
    AuditableEntitySaveChangesInterceptor interceptor = provider.GetRequiredService<AuditableEntitySaveChangesInterceptor>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
        sqlOptions.CommandTimeout(30);
    })
    .AddInterceptors(interceptor)
    // Configurazioni per Blazor Server
    .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
    .EnableDetailedErrors(builder.Environment.IsDevelopment())
    .ConfigureWarnings(warnings => warnings.Throw(
        RelationalEventId.MultipleCollectionIncludeWarning));
}, ServiceLifetime.Scoped);

// Registrazione del seeder
builder.Services.AddScoped<IDataSeeder, DatabaseSeeder>();

// Registrazione Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

// Registrazione DbContext Factory per gestire meglio le istanze in Blazor Server
builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();

// Registrazione AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

// Registrazione servizio di mapping astratto
builder.Services.AddScoped<IMappingService, AutoMapperService>();

// Registrazione FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(MappingProfile)));

// Registrazione Application services
builder.Services.AddScoped<IUtenteService, UtenteService>();
builder.Services.AddScoped<IGiocoService, GiocoService>();
builder.Services.AddScoped<IAcquistoService, AcquistoService>();
builder.Services.AddScoped<IRecensioneService, RecensioneService>();
builder.Services.AddScoped<IStatisticheService, StatisticheService>();
builder.Services.AddScoped<IGiochiAcquistatiService, GiochiAcquistatiService>();

// Registrazione API Controllers
builder.Services.AddControllers();

// Registrazione Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "GameStore API", Version = "v1" });
});

WebApplication app = builder.Build();

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
    using IServiceScope scope = app.Services.CreateScope();
    IDataSeeder seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();

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
