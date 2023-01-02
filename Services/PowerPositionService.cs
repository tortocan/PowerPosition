﻿using PowerPosition.Models;

namespace PowerPosition.Services
{
	public class PowerPositionService : ICsvParserService<PowerPositionModel>
	{
		private readonly ICsvParserService<PowerPositionModel> csvParserService;

		public PowerPositionService(ICsvParserService<PowerPositionModel> csvParserService)
		{
			this.csvParserService = csvParserService;
		}

		public IEnumerable<PowerPositionModel> ReadFile(string path)
		{
			return csvParserService.ReadFile(path);
		}

		public string WriteFile(string path, IEnumerable<PowerPositionModel> values)
		{
			var fileInfo = new FileInfo(path);
			var date = DateTime.Now;
			var fileName = fileInfo.Name.Replace(fileInfo.Extension, string.Empty);
			var filePath = Path.Combine(fileInfo.DirectoryName, $"{fileName}_{date.ToString("yyyymmdd")}_{date.ToString("HHmm")}.{fileInfo.Extension}");
			return csvParserService.WriteFile(filePath, values);
		}
	}
}