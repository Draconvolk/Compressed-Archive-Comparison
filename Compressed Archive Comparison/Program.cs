// See https://aka.ms/new-console-template for more information
using CompressedArchiveComparison;

var workflow = new Workflow(new ConfigurationInfo());
var configName = "Config.json";

try
{
	Console.WriteLine("The default config fileis [Config.json]. A custom config file can be specified as an argument");
	workflow.SetConfig(args, configName);
	workflow.LoadConfig();
}
catch
{
	Environment.Exit(-1);
}
await workflow.LoadCompressedSource();
await workflow.DisplaySource();
await workflow.LoadDestination();
await workflow.IdentifyMissingFiles();
await workflow.DisplayMissingFiles();
await workflow.ExportMissingFiles();

Console.WriteLine();
Console.WriteLine("Code Complete!");
Console.WriteLine();

//End of Main