// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using CompressedArchiveComparison.Workflow;
using CompressedArchiveComparison.Components;
using CompressedArchiveComparison.Exceptions;

using var host = CompressionHostBuilder.CreateHostBuilder().Build();
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;
var comparisonService = services.GetRequiredService<IComparisonWorkflow>();
var exceptionList = services.GetRequiredService<IExceptionList>();

if (!CompressionHostBuilder.HostExceptionList.Any)
{
    await comparisonService.StartWorkflow(args);
}
if (!exceptionList.Any)
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
        if (exceptionList.Any)
        {
            break;
        }
    }
    Task.WaitAll([.. displayTasks]);
}

if (exceptionList.Any)
{
    exceptionList.DisplayExceptions();
}

Console.WriteLine();
Console.WriteLine("Code Complete!");
Console.WriteLine();

//End of Main