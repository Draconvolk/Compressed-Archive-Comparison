// See https://aka.ms/new-console-template for more information
using CompressedArchiveComparison.Components;
using CompressedArchiveComparison.Config;

var workflow = new Workflow(new ConfigurationInfo());
var configName = "Config.json";

try
{
	Console.WriteLine("The default config fileis [Config.json]. A custom config file can be specified as an argument");
	workflow.SetConfig(args, configName);
	var loadConfig = Task.Run(workflow.LoadConfig);
	await loadConfig;
}
catch
{
	Environment.Exit(-1);
}

var loadSource = Task.Run(workflow.LoadCompressedSource);
var loadDestination = Task.Run(workflow.LoadDestination);

Task.WaitAll(loadSource, loadDestination);

var displaySource = Task.Run(workflow.DisplaySource);
var identifyMissing = Task.Run(workflow.IdentifyMissingFiles);

Task.WaitAll(displaySource, identifyMissing);

var exportMissing = Task.Run(workflow.ExportMissingFiles);
var displayMissing = Task.Run(workflow.DisplayMissingFiles);

Task.WaitAll(exportMissing, displayMissing);

Console.WriteLine();
Console.WriteLine("Code Complete!");
Console.WriteLine();

//End of Main