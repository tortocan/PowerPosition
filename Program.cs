// See https://aka.ms/new-console-template for more information
using Axpo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PowerPosition;
using PowerPosition.Services;

var configuration = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
		.AddEnvironmentVariables(prefix: "CRON_")
		.Build();

var services = new ServiceCollection();
services.AddScoped<IPowerService, PowerService>();
services.AddOptions<PowerPositionOptions>().Bind(configuration.GetSection(nameof(PowerPositionOptions)));
services.AddScoped(typeof(ICsvParserService<>), typeof(CsvParserService<>));
services.AddScoped<IPowerPositionService, PowerPositionService>();
services.AddLogging(x => x.AddConsole());
services.AddTransient<App>();

var serviceProvider = services.BuildServiceProvider();

serviceProvider.GetService<App>().Run(args);