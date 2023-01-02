using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

namespace PowerPosition.Services
{
	public class CsvParserService<T> : ICsvParserService<T> where T : class
	{
		private readonly CsvConfiguration config;
		private readonly ILogger<CsvParserService<T>> logger;

		public CsvParserService(ILogger<CsvParserService<T>> logger)
		{
			config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				NewLine = Environment.NewLine
			};
			this.logger = logger;
		}

		public void SetDelimiter(string delimiter)
		{
			if (string.IsNullOrWhiteSpace(delimiter)) return;
			logger.LogInformation($"Change delimiter from {config.Delimiter} to {delimiter}");
			config.Delimiter = delimiter;
		}

		public string WriteFile(string path, IEnumerable<T> values)
		{
			using StreamWriter sw = new(path, false, new UTF8Encoding(true));
			using CsvWriter cw = new(sw, config);
			logger.LogInformation($"Total values to be written ({values.Count()})");
			logger.LogDebug($"Writting heders");
			cw.WriteHeader<T>();
			cw.NextRecord();
			foreach (T emp in values)
			{
				logger.LogDebug($"Writting new record");
				cw.WriteRecord(emp);
				cw.NextRecord();
			}
			return path;
		}

		public IEnumerable<T> ReadFile(string path)
		{
			try
			{
				using StreamReader reader = new(path, Encoding.Default);
				using CsvReader csv = new(reader, config);
				var records = csv.GetRecords<T>().ToList();
				return records;
			}
			catch (UnauthorizedAccessException e)
			{
				logger.LogError(e.Message);
				throw new Exception(e.Message);
			}
			catch (FieldValidationException e)
			{
				logger.LogError(e.Message);
				throw new Exception(e.Message);
			}
			catch (CsvHelperException e)
			{
				logger.LogError(e.Message);
				throw new Exception(e.Message);
			}
			catch (Exception e)
			{
				logger.LogError(e.Message);
				throw new Exception(e.Message);
			}
		}
	}
}