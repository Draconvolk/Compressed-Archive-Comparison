// See https://aka.ms/new-console-template for more information
using CompressedArchiveComparison;

const string source = "source";

var folderInfo = GetInfoFromJson();
var sourceList = await GetFileList(folderInfo);

Console.WriteLine("Compressed Files Found");

await PrintFileList(sourceList, folderInfo);

var destinationList = await DataProcessing.GetDirectoryFileList(folderInfo);
var missingList = await DataProcessing.GetMissingSourceFiles(folderInfo, sourceList, destinationList);

await PrintFileList(missingList, folderInfo, "missing");
await ExportToFile(missingList, folderInfo);

Console.WriteLine();
Console.WriteLine("Code Complete!");
Console.WriteLine();
//End of Main
static IInfo GetInfoFromJson()
{
	var readData = DataProcessing.ReadPathInfo();

	var folderInfo = DataProcessing.GetAsInfo(readData);
	return folderInfo;
}

static async Task<IEnumerable<string>> GetFileList(IInfo folderInfo)
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
	var fileList = await DataProcessing.GetCompressedFileList(folderInfo);
	return fileList;
}

static async Task PrintFileList(IEnumerable<string> fileList, IInfo info, string type = source)
{
	foreach (var file in fileList)
	{
		Console.WriteLine(file);
		var fileContents = type == source ? await DataProcessing.GetCompressedFileContent(file) : new List<string>();

		if (info.Verbose) {
			await Task.Run(() =>
			{
				foreach (var contents in fileContents)
				{
					Console.WriteLine($"   {contents}");
				}
			});
		}

		if (type == source)
		{
			Console.WriteLine();
		}
	}
}

static async Task ExportToFile(IEnumerable<string> fileList, IInfo info)
{
	if (await DataProcessing.WriteToFile(fileList, info.ExportFileName))
	{
		Console.WriteLine();
		Console.WriteLine("Missing File List export successful");
		Console.WriteLine();
	}
	else
	{
		Console.WriteLine("*** An error was encountered attempting to export the Missing File List");
		Console.WriteLine();
	}
}