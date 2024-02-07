// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CompressedArchiveComparison.Workflow;
using CompressedArchiveComparison.Components;
using CompressedArchiveComparison.Exceptions;

var builder = Host.CreateApplicationBuilder();

using var host = CompressionHostBuilder.CreateHostBuilder().Build();
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;
var comparisonService = services.GetRequiredService<IComparisonWorkflow>();
if (!ExceptionList.Any){
	await comparisonService.StartWorkflow(args);
}
if (!ExceptionList.Any)
{
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
		if (ExceptionList.Any)
		{
			break;
		}
	}
	Task.WaitAll([.. displayTasks]);
}

if (ExceptionList.Any)
{
	ExceptionList.DisplayExceptions();
}

Console.WriteLine();
Console.WriteLine("Code Complete!");
Console.WriteLine();

//End of Main