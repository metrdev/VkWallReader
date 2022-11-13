using System.Reflection;
using BLL.Interfaces;
using BLL.Services;
using DAL.Commands.AddCountedWall;
using DAL.EF;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using DAL;
using WebApp.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
    options.Filters.Add<ExceptionFilter>());
// DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));
// Injections
builder.Services.AddScoped<IReaderService, ReaderService>();
builder.Services.AddScoped<IVkParser, VkParser>();
builder.Services.AddScoped<IAddCountedWallCommandHandler, AddCountedWallCommandHandler>();
// Logger
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();
builder.Logging.AddSerilog(logger);
// Swagger
builder.Services.AddSwaggerGen(config =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.RoutePrefix = String.Empty;
    config.SwaggerEndpoint("swagger/v1/swagger.json", "VkWallReader");
});


app.UseHttpsRedirection();
app.MapControllers();

app.Run();
