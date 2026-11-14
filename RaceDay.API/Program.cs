using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Services;

var builder = WebApplication.CreateBuilder(args);

// IgnoreCycles stops the 500 error "possible object cycle" when Include() loads Event <-> Enrolment or Category <-> Event.
// SuppressImplicitRequiredAttribute...: our models use non-nullable strings with "" defaults (e.g. Description, BannerImageUrl),
// so an empty value from the MVC app must not be rejected with a 400.
builder.Services.AddControllers(o => o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
    .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RaceDayDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<BlobService>();
builder.Services.AddCors(o => o.AddPolicy("AllowMVC", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// Development convenience: create / update the database on startup.
// If you have created EF migrations (dotnet ef migrations add InitialCreate) they are applied;
// otherwise the schema is created straight from the models so the app runs out of the box.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<RaceDayDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        if (db.Database.GetMigrations().Any()) db.Database.Migrate();
        else db.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Could not create/update the database. Check the connection string in appsettings.json.");
    }
}

if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowMVC");
app.UseSession();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Needed so test projects can reference the entry point if ever required.
public partial class Program { }
