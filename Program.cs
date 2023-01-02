// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
			  .SetBasePath(Directory.GetCurrentDirectory())
			  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			  .AddEnvironmentVariables(prefix: "CRON_")
			  .Build();

var consoleSettings = new ConsoleSettings();
configuration.GetSection(nameof(ConsoleSettings)).Bind(consoleSettings);

Console.WriteLine($"{DateTime.UtcNow}: Output String: '{consoleSettings.OutputString}'");