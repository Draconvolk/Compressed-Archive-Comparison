using CompressedArchiveComparison.CompressedReadonlyReaders;
using CompressedArchiveComparison.Compressions;
using CompressedArchiveComparison.Config;
using CompressedArchiveComparison.Exceptions;
using CompressedArchiveComparison.Factories;
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
        public static IExceptionList HostExceptionList { get; private set; } = new ExceptionList();

        public static HostApplicationBuilder CreateHostBuilder(bool unitTesting = false)
        {
            try
            {
                var builder = Host.CreateApplicationBuilder();
                builder.Services.RegisterCompressionServices(unitTesting);
                return builder;
            }
            catch (Exception ex)
            {
                HostExceptionList.Add(ex, $"Something went wrong trying to build a new application builder", $"CompressionHostBuilder\\CreateHostVuilder");
                throw;
            }
        }

        /// <summary>
        /// Register all objects for Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="unitTesting"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterCompressionServices(this IServiceCollection services, bool unitTesting)
        {
            try
            {
                services.AddSingleton<IComparisonWorkflow, ComparisonWorkflow>();
                services.AddScoped<IInfo, ConfigurationInfo>();
                services.AddSingleton<IWorkflowActions, WorkflowActions>();
                services.AddTransient<ICompressedReader, RarReader>();
                services.AddTransient<ICompressedReader, SevenZipReader>();
                services.AddTransient<ICompressedReader, ZipReader>();
                services.AddSingleton<ICompressionFactory, CompressionFactory>();
                services.AddTransient<RarCompression>();
                services.AddTransient<SevenZipCompression>();
                services.AddTransient<ZipCompression>();
                services.AddSingleton<DataProcessing>();
                services.AddTransient<ICustomException, CustomException>();
                services.AddSingleton<IExceptionList>(s => s.ResolveWith<ExceptionList>(unitTesting));

#pragma warning disable CS8603 // Possible null reference return.
                services.AddTransient<CompressionResolver>(serviceProvider =>
                    key =>
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
            catch (Exception ex)
            {
                HostExceptionList.Add(ex, $"Something went wrong trying to register services", $"CompressionHostBuilder\\RegisterCompressionServices");
                throw;
            }
        }

        /// <summary>
        /// Register an implementation of type <typeparamref name="T"/> that requires <paramref name="parameters"/> in the constructor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T ResolveWith<T>(this IServiceProvider provider, params object[] parameters) where T : class
            => ActivatorUtilities.CreateInstance<T>(provider, parameters);
    }
}
