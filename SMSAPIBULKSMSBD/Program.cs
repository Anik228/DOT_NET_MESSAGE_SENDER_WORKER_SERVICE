/*using SMSAPIBULKSMSBD;
using SMSAPIBULKSMSBD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=***;Database=***;User Id=**;Password=***;")
);

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.Run();*/


/*using SMSAPIBULKSMSBD;
using SMSAPIBULKSMSBD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
var configuration = builder.Configuration;

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration) // Use Serilog settings from appsettings.json
    .CreateLogger();

// Use Serilog as the logging provider
builder.Host.UseSerilog();

// Add DbContext service with connection string from configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Register configuration sections for custom settings
builder.Services.Configure<AppSettings>(configuration);

// Register the Worker as a hosted service
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

try
{
    Log.Information("Starting the application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start");
}
finally
{
    Log.CloseAndFlush(); // Ensure logs are flushed to file
}*/

/*using SMSAPIBULKSMSBD;
using SMSAPIBULKSMSBD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
var configuration = builder.Configuration;

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration) // Use Serilog settings from appsettings.json
    .CreateLogger();

// Use Serilog as the logging provider
builder.Host.UseSerilog();

// Add Windows Service hosting
builder.Host.UseWindowsService();

// Add DbContext service with connection string from configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Register configuration sections for custom settings
builder.Services.Configure<AppSettings>(configuration);

// Register the Worker as a hosted service
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

try
{
    Log.Information("Starting the application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start");
}
finally
{
    Log.CloseAndFlush(); // Ensure logs are flushed to file
}*/

/*using SMSAPIBULKSMSBD;
using SMSAPIBULKSMSBD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Explicitly set the base path and load configuration from appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)!) // Ensure the base path is where the app runs
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)    // Load appsettings.json
    .Build();

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration) // Use Serilog settings from appsettings.json
    .CreateLogger();

// Use Serilog as the logging provider
builder.Host.UseSerilog();

// Add Windows Service hosting
builder.Host.UseWindowsService();

// Add DbContext service with connection string from configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Register configuration sections for custom settings
builder.Services.Configure<AppSettings>(configuration);

// Register the Worker as a hosted service
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

try
{
    Log.Information("Starting the application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start.");
}
finally
{
    Log.CloseAndFlush(); // Ensure logs are flushed to file
}*/

using SMSAPIBULKSMSBD;
using SMSAPIBULKSMSBD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Serilog;
using System.Reflection;
using Microsoft.Extensions.Configuration;

var options = new WebApplicationOptions
{
    ContentRootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)!, 
};


var builder = WebApplication.CreateBuilder(options);

builder.Configuration.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)!)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) 
    .CreateLogger();


builder.Host.UseSerilog();

builder.Host.UseWindowsService();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

try
{
    Log.Information("Starting the application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start.");
}
finally
{
    Log.CloseAndFlush();
}



