// See https://aka.ms/new-console-template for more information
using SkyrimModVerification;

var fileList = GetFileList();

Console.WriteLine("Compressed Files Found");
foreach (var file in fileList)
{
	Console.WriteLine(file);

	var fileContents = DataProcessing.GetCompressedFileContent(file);
	foreach (var contents in fileContents)
	{
		Console.WriteLine($"   {contents}");
	}
	Console.WriteLine();
	Console.WriteLine();
}


Console.WriteLine();
Console.WriteLine("Code Complete!");
Console.WriteLine();



static IEnumerable<string> GetFileList()
{
	var readData = DataProcessing.ReadPathInfo();
	var folderInfo = DataProcessing.GetAsInfo(readData);

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