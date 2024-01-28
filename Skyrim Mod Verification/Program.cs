// See https://aka.ms/new-console-template for more information
using SkyrimModVerification;
using System.Globalization;

const string source = "source";

var folderInfo = GetInfoFromJson();

var sourceList = GetFileList(folderInfo);

Console.WriteLine("Compressed Files Found");

PrintFileList(sourceList);

var destinationList = DataProcessing.GetDirectoryFileList(folderInfo);

var missingList = DataProcessing.GetMissingSourceFiles(folderInfo, sourceList, destinationList);

PrintFileList(destinationList, "destination");

Console.WriteLine();
Console.WriteLine("Code Complete!");
Console.WriteLine();



static IInfo GetInfoFromJson()
{
	var readData = DataProcessing.ReadPathInfo();

	var folderInfo = DataProcessing.GetAsInfo(readData);
	return folderInfo;
}

static IEnumerable<string> GetFileList(IInfo folderInfo)
{

	if (!DataProcessing.IsValidInfo(folderInfo))
	{
		Console.WriteLine("Invalid Source and Destination data in Json file. Please specify valid paths and try again.");
		Console.WriteLine();
		Environment.Exit(-1);
	}

	Console.WriteLine();
	Console.WriteLine($"Src: {folderInfo.CompressedSource}");
	Console.WriteLine($"Dest: {folderInfo.DeployDestination}");
	Console.WriteLine();
	var fileList = DataProcessing.GetCompressedFileList(folderInfo);
	return fileList;
}

static void PrintFileList(IEnumerable<string> fileList, string type = source)
{
	foreach (var file in fileList)
	{
		Console.WriteLine(file);

		var fileContents = type == source ? DataProcessing.GetCompressedFileContent(file): new List<string>();

#if DEBUG
		foreach (var contents in fileContents)
		{
			Console.WriteLine($"   {contents}");
		}
		if (type == source)
		{
			Console.WriteLine();
		}
#endif
	}
}
