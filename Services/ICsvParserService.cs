namespace PowerPosition.Services
{
	public interface ICsvParserService<T> where T : class
	{
		IEnumerable<T> ReadFile(string path);

		string WriteFile(string path, IEnumerable<T> values);

		void SetDelimiter(string delimiter);
	}
}