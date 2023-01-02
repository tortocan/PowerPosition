using CsvHelper.Configuration.Attributes;

namespace PowerPosition.Models
{
	public class PowerPositionModel
	{
		[Name("Local Time")]
		[Format("HH:mm")]
		public DateTime LocalTime { get; set; }

		[Name("Volume")]
		public double Volume { get; set; }
	}
}