using Axpo;
using Microsoft.Extensions.Logging;
using PowerPosition.Models;

namespace PowerPosition.Services
{
	public class PowerPositionService : IPowerPositionService
	{
		private readonly ICsvParserService<PowerPositionModel> csvParserService;
		private readonly IPowerService powerService;
		private readonly ILogger<PowerPositionService> logger;

		public PowerPositionService(ICsvParserService<PowerPositionModel> csvParserService, IPowerService powerService, ILogger<PowerPositionService> logger)
		{
			this.csvParserService = csvParserService;
			this.powerService = powerService;
			this.logger = logger;
		}

		public IEnumerable<PowerPositionModel> ReadFile(string path)
		{
			return csvParserService.ReadFile(path);
		}

		public IEnumerable<PowerTrade> GetTrades()
		{
			var retry = 0;
			IEnumerable<PowerTrade> trades = new List<PowerTrade>();
			try
			{
				logger.LogInformation("Getting trades");
				trades = powerService.GetTrades(DateTime.Now.ToLocalTime());
				logger.LogDebug($"The count of trades is ({trades.Count()})");
			}
			catch
			{
				if (retry == 3)
				{
					throw;
				}
				else
				{
					var date = new DateTime().AddSeconds(10);
					logger.LogWarning($"The power service has thrown an error will retry again ({date.Second})");
					Thread.Sleep(date.Millisecond);
					retry++;
					GetTrades();
				}
			}
			return trades;
		}

		public string WriteFile(string path, IEnumerable<PowerPositionModel> values)
		{
			var date = DateTime.Now.ToLocalTime();
			string filePath = GetTimestampFilename(path, date);

			return csvParserService.WriteFile(filePath, values);
		}

		private string GetTimestampFilename(string path, DateTime date)
		{
			var fileInfo = new FileInfo(path);
			var fileName = fileInfo.Name.Replace(fileInfo.Extension, string.Empty);
			var filePath = Path.Combine(fileInfo.DirectoryName ?? string.Empty, $"{fileName}_{date:yyyymmdd}_{date:HHmm}{fileInfo.Extension}");
			logger.LogInformation($"Setting new file path {filePath}");
			return filePath;
		}

		public void SetDelimiter(string delimiter)
		{
			csvParserService.SetDelimiter(delimiter);
		}
	}
}