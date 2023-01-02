using Microsoft.Extensions.Options;
using PowerPosition.Models;
using PowerPosition.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPosition
{
	public class App
	{
		private readonly IPowerPositionService csvService;
		private readonly IOptions<PowerPositionOptions> powerPositionOptions;

		public App(IPowerPositionService csvService, IOptions<PowerPositionOptions> powerPositionOptions)
		{
			this.csvService = csvService;
			this.powerPositionOptions = powerPositionOptions;
		}

		public int Run(string[] args)
		{
			Console.WriteLine($"{DateTime.UtcNow}: Output String: '{powerPositionOptions.Value.CsvOutputPath}'");

			var path = csvService.WriteFile(powerPositionOptions.Value.CsvOutputPath, new List<PowerPositionModel>() { new PowerPositionModel { LocalTime = DateTime.Now.ToLocalTime(), Volume = 100 } });

			var file = csvService.ReadFile(path);

			file.ToList().ForEach(x => Console.WriteLine(x.LocalTime));
			return 0;
		}
	}
}