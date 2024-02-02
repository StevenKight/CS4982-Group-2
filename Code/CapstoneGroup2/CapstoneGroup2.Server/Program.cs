using System.IdentityModel.Tokens.Jwt;
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
builder.Services.AddDbContext<DocunotesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add Dal classes
builder.Services
    .AddTransient<IDbDal<User>, UserDal>()
    .AddTransient<IDbDal<Source>, SourceDal>()
    .AddTransient<IDbDal<Note>, NotesDal>()
    .AddTransient<IDbDal<Shared>, SharedDal>();

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

// Get the token from the request header and set the current user
app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    if (!string.IsNullOrWhiteSpace(token))
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        var username = jsonToken?.Claims.First(claim => claim.Type == "unique_name").Value;
        var dbContext = context.RequestServices.GetRequiredService<DocunotesDbContext>();
        dbContext.CurrentUser = dbContext.Users.Find(username);
    }

    await next();
});

app.Run();