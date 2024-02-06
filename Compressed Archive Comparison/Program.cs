// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CompressedArchiveComparison;
using CompressedArchiveComparison.Workflow;

var builder = Host.CreateApplicationBuilder();

using var host = CompressionHostBuilder.CreateHostBuilder().Build();
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;
var comparisonService = services.GetRequiredService<IComparisonWorkflow>();

await comparisonService.StartWorkflow(args);
var displayTasks = new List<Task>();

while (!comparisonService.IsFinished())
{
	if (displayTasks.Count == 0
		&& comparisonService.CanDisplay(DisplayType.Source))
	{
		displayTasks.Add(comparisonService.DisplaySource());
	}
	if (displayTasks.Count == 1
		&& comparisonService.CanDisplay(DisplayType.Missing))
	{
		displayTasks.Add(displayTasks.First().ContinueWith(first => comparisonService.DisplayMissing()));
	}
	await comparisonService.Next();
}
Task.WaitAll([.. displayTasks]);

Console.WriteLine();
Console.WriteLine("Code Complete!");
Console.WriteLine();

//End of Main