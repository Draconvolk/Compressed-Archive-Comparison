// See https://aka.ms/new-console-template for more information
using Skyrim_Mod_Verification;

var readData = DataProcessing.ReadPathInfo();
var folderInfo = DataProcessing.GetAsInfo(readData);

if(!DataProcessing.IsValidInfo(folderInfo))
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

Console.WriteLine("Compressed Files Found");
foreach (var file in fileList)
{
	Console.WriteLine(file);
}




//using (var archiveFile = new ArchiveFile(@"Archive.ARJ"))
//{
//	archiveFile.Extract("Output"); // extract all
//}
Console.WriteLine();
Console.WriteLine("Code Complete!");
Console.WriteLine();