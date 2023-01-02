using CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PowerPosition.Models;
using PowerPosition.Services;

namespace PowerPosition
{
	public class App
	{
		private readonly IPowerPositionService powerPositionService;
		private readonly IOptions<PowerPositionOptions> powerPositionOptions;
		private readonly ILogger<App> logger;

		public App(IPowerPositionService powerPositionService, IOptions<PowerPositionOptions> powerPositionOptions, ILogger<App> logger)
		{
			this.powerPositionService = powerPositionService;
			this.powerPositionOptions = powerPositionOptions;
			this.logger = logger;
		}

		public int Run(string[] args)
		{
			try
			{
				logger.LogInformation("App started running...");
				var csvOutputPath = powerPositionOptions.Value.CsvOutputPath;
				Parser.Default.ParseArguments<PowerPositionOptions>(args)
					   .WithParsed(o =>
					   {
						   if (!string.IsNullOrWhiteSpace(o.CsvDelimiter))
						   {
							   powerPositionService.SetDelimiter(o.CsvDelimiter);
						   }
						   else
						   {
							   powerPositionService.SetDelimiter(powerPositionOptions.Value.CsvDelimiter);
						   }
						   if (!string.IsNullOrWhiteSpace(o.CsvOutputPath))
						   {
							   logger.LogDebug("Setting csv output file from args");
							   csvOutputPath = o.CsvOutputPath;
						   }
					   });

				var trades = powerPositionService.GetTrades();

				var values = new List<PowerPositionModel>();

				var groupedTradePeriods = trades.SelectMany(x => x.Periods).GroupBy(x => x.Period);
				logger.LogInformation($"Total grouped trade periods are ({groupedTradePeriods.Count()})");

				foreach (var item in groupedTradePeriods)
				{
					//The PowerTrade class contains an array of PowerPeriods for the given day. The period number starts at 1, which is the first period of the day and starts at 23:00 (11 pm) on the previous day
					if (item.Any())
					{
						var localDate = new DateTime().AddHours(22).AddHours(item.First().Period);
						logger.LogInformation($"Found a total of ({item.Count()}) periods");
						var volume = Math.Round(item.Sum(x => x.Volume), 2);
						logger.LogInformation($"Adding new value with LocalTime {localDate.TimeOfDay} and Volume {volume}");
						values.Add(new PowerPositionModel { LocalTime = localDate, Volume = volume });
					}
					else
					{
						logger.LogWarning("Item has no periods !");
					}
				}
				var path = powerPositionService.WriteFile(csvOutputPath, values);

				var file = powerPositionService.ReadFile(path);

				file.ToList().ForEach(x => logger.LogDebug($"{x.LocalTime} {x.Volume}"));
				logger.LogInformation("App finished succesfully");
			}
			catch (Exception ex)
			{
				logger.LogCritical(ex.Message);
				return 1;
			}
			return 0;
		}
	}
}