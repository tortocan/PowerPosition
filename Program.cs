// See https://aka.ms/new-console-template for more information
using Axpo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PowerPosition;
using PowerPosition.Services;

var configuration = new ConfigurationBuilder()
			  .SetBasePath(Directory.GetCurrentDirectory())
			  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			  .AddEnvironmentVariables(prefix: "CRON_")
			  .Build();

var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped<IPowerService, PowerService>();
serviceCollection.AddOptions<PowerPositionOptions>().Bind(configuration.GetSection(nameof(PowerPositionOptions)));
serviceCollection.AddScoped(typeof(ICsvParserService<>), typeof(CsvParserService<>));
serviceCollection.AddScoped<IPowerPositionService, PowerPositionService>();
serviceCollection.AddTransient<App>();

var serviceProvider = serviceCollection.BuildServiceProvider();

serviceProvider.GetService<App>().Run(args);