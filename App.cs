using Microsoft.Extensions.Options;
using PowerPosition.Models;
using PowerPosition.Services;

namespace PowerPosition
{
	public class App
	{
		private readonly IPowerPositionService powerPositionService;
		private readonly IOptions<PowerPositionOptions> powerPositionOptions;

		public App(IPowerPositionService powerPositionService, IOptions<PowerPositionOptions> powerPositionOptions)
		{
			this.powerPositionService = powerPositionService;
			this.powerPositionOptions = powerPositionOptions;
			this.powerPositionService.SetDelimiter(powerPositionOptions.Value.CsvDelimiter);
		}

		public int Run(string[] args)
		{
			Console.WriteLine($"{DateTime.UtcNow}: Output String: '{powerPositionOptions.Value.CsvOutputPath}'");

			var trades = powerPositionService.GetTrades();
			var values = new List<PowerPositionModel>();

			var groupedTradePeriods = trades.SelectMany(x => x.Periods).GroupBy(x => x.Period);

			foreach (var item in groupedTradePeriods)
			{
				//The PowerTrade class contains an array of PowerPeriods for the given day. The period number starts at 1, which is the first period of the day and starts at 23:00 (11 pm) on the previous day
				var localDate = new DateTime().AddHours(22).AddHours(item.First().Period);
				var volume = Math.Round(item.Sum(x => x.Volume), 2);
				values.Add(new PowerPositionModel { LocalTime = localDate, Volume = volume });
			}
			var path = powerPositionService.WriteFile(powerPositionOptions.Value.CsvOutputPath, values);

			var file = powerPositionService.ReadFile(path);

			file.ToList().ForEach(x => Console.WriteLine(x.LocalTime));
			return 0;
		}
	}
}