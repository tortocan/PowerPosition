using Axpo;
using PowerPosition.Models;
using System.IO;

namespace PowerPosition.Services
{
	public class PowerPositionService : IPowerPositionService
	{
		private readonly ICsvParserService<PowerPositionModel> csvParserService;
		private readonly IPowerService powerService;

		public PowerPositionService(ICsvParserService<PowerPositionModel> csvParserService, IPowerService powerService)
		{
			this.csvParserService = csvParserService;
			this.powerService = powerService;
		}

		public IEnumerable<PowerPositionModel> ReadFile(string path)
		{
			return csvParserService.ReadFile(path);
		}

		public IEnumerable<PowerTrade> GetTrades()
		{
			return powerService.GetTrades(DateTime.Now.ToLocalTime());
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
			return filePath;
		}

		public void SetDelimiter(string delimiter)
		{
			csvParserService.SetDelimiter(delimiter);
		}
	}
}