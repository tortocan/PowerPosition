using CommandLine;

[Verb("csv")]
public class PowerPositionOptions
{
	[Option('o', "csv-output-path", Required = false, HelpText = "Sets CSV output path.")]
	public string CsvOutputPath { get; set; }

	[Option('d', "csv-delimiter", Required = false, HelpText = "Sets CSV delimiter.")]
	public string CsvDelimiter { get; set; }
}