using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace PowerPosition.Services
{
	public class CsvParserService<T> : ICsvParserService<T> where T : class
	{
		private readonly CsvConfiguration config;

		public CsvParserService()
		{
			config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				NewLine = Environment.NewLine
			};
		}

		public void SetDelimiter(string delimiter)
		{
			config.Delimiter = delimiter;
		}

		public string WriteFile(string path, IEnumerable<T> values)
		{
			using StreamWriter sw = new(path, false, new UTF8Encoding(true));
			using CsvWriter cw = new(sw, config);
			cw.WriteHeader<T>();
			cw.NextRecord();
			foreach (T emp in values)
			{
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
				throw new Exception(e.Message);
			}
			catch (FieldValidationException e)
			{
				throw new Exception(e.Message);
			}
			catch (CsvHelperException e)
			{
				throw new Exception(e.Message);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
	}
}