using DevSkill.Inventory.WorkerService;
using Serilog.Events;
using Serilog;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
     .AddEnvironmentVariables()
     .Build();

Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.File("Logs/bootstrap-log.txt", rollingInterval: RollingInterval.Day)
        .CreateBootstrapLogger();

try
{
    Log.Information("Starting up the application with bootstrap logger");

 

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    var migrationAssemblyName = typeof(Worker).Assembly.FullName;

    // Reconfigure Serilog with the main logger configuration
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    Log.Information("Application Starting up");

    IHost host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .UseSerilog()
        .ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterModule(new WorkerModule(connectionString, migrationAssemblyName));
        })
        .ConfigureServices(services =>
        {
            services.AddHostedService<Worker>();
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
