using API.Dal;
using API.Model;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add controllers and endpoints for OpenAPI/Swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add db context with SQL Server
builder.Services.AddDbContext<StudyApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add Dal classes
builder.Services
    .AddSingleton<IDbDal<User>, UserDal>()
    .AddSingleton<IDbDal<Note>, NotesDal>()
    .AddSingleton<IDbDal<Source>, SourceDal>()
    .AddSingleton<IDbDal<Shared>, SharedDal>();

// Build and configure the app.
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();