// See https://aka.ms/new-console-template for more information
using Skyrim_Mod_Verification;

Console.WriteLine("Hello, World!");

var readData = DataProcessing.ReadPathInfo();
var folderInfo = DataProcessing.GetAsInfo(readData);

if(!DataProcessing.IsValidInfo(folderInfo))
{
	Console.WriteLine("Invalid Source and Destination data in Json file. Please specify valid paths and try again.");
	Console.WriteLine();
	Console.WriteLine();
	Console.WriteLine();
	Console.WriteLine();
	Environment.Exit(-1);
}

Console.WriteLine($"Src: {folderInfo.CompressedSource}");
Console.WriteLine($"Dest: {folderInfo.DeployDestination}");


//using (var archiveFile = new ArchiveFile(@"Archive.ARJ"))
//{
//	archiveFile.Extract("Output"); // extract all
//}
Console.WriteLine();
Console.WriteLine("Code Complete!");
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();