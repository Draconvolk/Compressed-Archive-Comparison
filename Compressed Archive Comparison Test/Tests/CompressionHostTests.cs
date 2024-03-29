﻿using Microsoft.Extensions.DependencyInjection;
using CompressedArchiveComparison.Config;
using CompressedArchiveComparison.CompressedReadonlyReaders;
using CompressedArchiveComparison.Components;
using Microsoft.Extensions.Hosting;
using CompressedArchiveComparison.Workflow;
using CompressedArchiveComparison.Exceptions;
using CompressedArchiveComparison.Factories;

namespace CompressedArchiveComparisonTests
{
    [TestClass]
	public class CompressionHostTests
	{
		[TestMethod]
		[DataRow(typeof(IComparisonWorkflow), 1)]
		[DataRow(typeof(IWorkflowActions), 1)]
		[DataRow(typeof(IInfo), 1)]
        [DataRow(typeof(ICompressedReader), 3)]
        [DataRow(typeof(DataProcessing), 1)]
        [DataRow(typeof(CompressionResolver), 1)]
        [DataRow(typeof(IExceptionList), 1)]
        [DataRow(typeof(ICompressionFactory), 1)]
        public void A_CreateHostBuilder_Type_Valid(Type testData, int count)
		{
			var host = CompressionHostBuilder.CreateHostBuilder(true).Build();

			Assert.IsNotNull(host);
			var resultInstance = host.Services.GetServices(testData);

			Assert.IsNotNull(resultInstance);
			Assert.IsTrue(resultInstance.Count() == count);
		}

		[TestMethod]
		[DataRow(".rar", "RarCompression")]
		[DataRow(".7z", "SevenZipCompression")]
		[DataRow(".zip", "ZipCompression")]
		public void A_CreateHostBuilder_CompressionResolver_Valid(string testData, string expectedResult)
		{
			var host = CompressionHostBuilder.CreateHostBuilder(true).Build();

			Assert.IsNotNull(host);
			var resultInstance = host.Services.GetService<CompressionResolver>();

			Assert.IsNotNull(resultInstance);
			var result = resultInstance(testData);

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result.GetTypeName());
		}

		[TestMethod]
        [DataRow(typeof(IComparisonWorkflow), 1)]
        [DataRow(typeof(IWorkflowActions), 1)]
        [DataRow(typeof(IInfo), 1)]
        [DataRow(typeof(ICompressedReader), 3)]
        [DataRow(typeof(DataProcessing), 1)]
        [DataRow(typeof(CompressionResolver), 1)]
        [DataRow(typeof(IExceptionList), 1)]
        [DataRow(typeof(ICompressionFactory), 1)]
        public void B_RegisterCompressionServices_Type_Valid(Type testData, int count)
		{
			var builder = Host.CreateApplicationBuilder();
			_ = builder.Services.RegisterCompressionServices(true);
			var host = builder.Build();

			Assert.IsNotNull(host);
			var resultInstance = host.Services.GetServices(testData);

			Assert.IsNotNull(resultInstance);
			Assert.IsTrue(resultInstance.Count() == count);
		}

		[TestMethod]
		[DataRow(".rar", "RarCompression")]
		[DataRow(".7z", "SevenZipCompression")]
		[DataRow(".zip", "ZipCompression")]
		public void B_RegisterCompressionServices_CompressionResolver_Valid(string testData, string expectedResult)
		{
			var builder = Host.CreateApplicationBuilder();
			var services = builder.Services.RegisterCompressionServices(true);
			var host = builder.Build();

			Assert.IsNotNull(host);
			var resultInstance = host.Services.GetService<CompressionResolver>();

			Assert.IsNotNull(resultInstance);
			var result = resultInstance(testData);

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result.GetTypeName());
		}

		[TestMethod]
		[DataRow(".rar")]
		public void C_CompressionHostBuilder_Rar_Valid(string testData)
		{
			var result = CompressionHostBuilder.rar;

			Assert.IsNotNull(result);
			Assert.AreEqual(testData, result);
		}

		[TestMethod]
		[DataRow(".7z")]
		public void C_CompressionHostBuilder_7z_Valid(string testData)
		{
			var result = CompressionHostBuilder.sevenZ;

			Assert.IsNotNull(result);
			Assert.AreEqual(testData, result);
		}

		[TestMethod]
		[DataRow(".zip")]
		public void C_CompressionHostBuilder_Zip_Valid(string testData)
		{
			var result = CompressionHostBuilder.zip;

			Assert.IsNotNull(result);
			Assert.AreEqual(testData, result);
		}
	}
}
