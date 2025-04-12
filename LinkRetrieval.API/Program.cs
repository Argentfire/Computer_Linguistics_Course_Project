using LinkRetrieval.Core.Contracts;
using LinkRetrieval.Core.Services;
using LinkRetrieval.Data.DB;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ISearchService, SearchService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddMvcCore().AddDataAnnotations();

builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();



string connectionString = "";
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
if (dbHost == null || dbName == null || dbPassword == null)
{
  connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}
else
{
  connectionString = $"Data Source={dbHost};Initial Catalog={dbName}; User ID=sa;Password={dbPassword}";
}
//var connectionString = $"Server={dbHost}; Database = {dbName}; Trusted_Connection = True; Password={dbPassword}";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  options.UseLazyLoadingProxies();
  options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
  {
    sqlOptions.EnableRetryOnFailure();
  });
});
builder.Services.AddCors(option =>
{
  option.AddDefaultPolicy(builder =>
  {
    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
  });
  option.AddPolicy("AllowAll", policy =>
  {
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
  });
});
//builder.Services.AddDbContext<ApplicationDbContext>(opt =>
//{
//  opt.UseSqlServer(connectionString,
//  sqlServerOptionsAction: sqlOptions =>
//  {
//    sqlOptions.EnableRetryOnFailure();
//  });
//});
builder.WebHost.ConfigureKestrel(serverOptions =>
{
  serverOptions.ListenAnyIP(5084); // HTTP for fallback/testing
  serverOptions.ListenAnyIP(7009, listenOptions =>
  {
    listenOptions.UseHttps();
  });
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.UseSwagger();
  app.UseSwaggerUI(); // default is at /swagger
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
