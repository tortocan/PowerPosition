// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using PowerPosition.Models;
using PowerPosition.Services;

var configuration = new ConfigurationBuilder()
			  .SetBasePath(Directory.GetCurrentDirectory())
			  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			  .AddEnvironmentVariables(prefix: "CRON_")
			  .Build();

var consoleSettings = new ConsoleSettings();
configuration.GetSection(nameof(ConsoleSettings)).Bind(consoleSettings);

Console.WriteLine($"{DateTime.UtcNow}: Output String: '{consoleSettings.OutputString}'");
var csvService = new CsvParserService<PowerPositionModel>();
csvService.WriteFile(consoleSettings.OutputString, new List<PowerPositionModel>() { new PowerPositionModel { LocalTime = DateTime.Now.ToLocalTime(), Volume = 100 } });

var file = csvService.ReadFile(consoleSettings.OutputString);

file.ToList().ForEach(x => Console.WriteLine(x.LocalTime));