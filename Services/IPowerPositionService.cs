using Axpo;
using PowerPosition.Models;

namespace PowerPosition.Services
{
	public interface IPowerPositionService
	{
		IEnumerable<PowerPositionModel> ReadFile(string path);

		string WriteFile(string path, IEnumerable<PowerPositionModel> values);

		IEnumerable<PowerTrade> GetTrades();

		void SetDelimiter(string delimiter);
	}
}