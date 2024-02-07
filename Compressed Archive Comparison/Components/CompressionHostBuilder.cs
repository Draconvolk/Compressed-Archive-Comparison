using CompressedArchiveComparison.CompressedReadonlyReaders;
using CompressedArchiveComparison.Compressions;
using CompressedArchiveComparison.Config;
using CompressedArchiveComparison.Exceptions;
using CompressedArchiveComparison.Workflow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CompressedArchiveComparison.Components
{
	public static class CompressionHostBuilder
	{
		public const string rar = ".rar";
		public const string sevenZ = ".7z";
		public const string zip = ".zip";

		public static HostApplicationBuilder CreateHostBuilder()
		{
			try
			{
				var builder = Host.CreateApplicationBuilder();
				builder.Services.RegisterCompressionServices();
				return builder;
			}
			catch (Exception ex)
			{
				ExceptionList.Add(ex, $"Something went wrong trying to build a new application builder", $"CompressionHostBuilder\\CreateHostVuilder");
				throw;
			}
		}

		public static IServiceCollection RegisterCompressionServices(this IServiceCollection services)
		{
			try
			{
				services.AddSingleton<IComparisonWorkflow, ComparisonWorkflow>();
				services.AddScoped<IInfo, ConfigurationInfo>();
				services.AddSingleton<IWorkflowActions, WorkflowActions>();
				services.AddTransient<ICompressedReader, RarReader>();
				services.AddTransient<ICompressedReader, SevenZipReader>();
				services.AddTransient<ICompressedReader, ZipReader>();
				services.AddTransient<RarCompression>();
				services.AddTransient<SevenZipCompression>();
				services.AddTransient<ZipCompression>();
				services.AddTransient<ICustomException, CustomException>();

#pragma warning disable CS8603 // Possible null reference return.
				services.AddTransient<CompressionResolver>(serviceProvider => key =>
				key.ToLower() switch
				{
					rar => serviceProvider.GetService<RarCompression>(),
					sevenZ => serviceProvider.GetService<SevenZipCompression>(),
					zip => serviceProvider.GetService<ZipCompression>(),
					_ => serviceProvider.GetService<SevenZipCompression>()
				});
#pragma warning restore CS8603 // Possible null reference return.
				return services;
			}
			catch(Exception ex)
			{
				ExceptionList.Add(ex, $"Something went wrong trying to register services", $"CompressionHostBuilder\\RegisterCompressionServices");
				throw;
			}
		}
	}
}
