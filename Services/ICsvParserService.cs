namespace PowerPosition.Services
{
	public interface ICsvParserService<T> where T : class
	{
		IEnumerable<T> ReadFile(string path);
		void WriteFile(string path, IEnumerable<T> values);
	}
}