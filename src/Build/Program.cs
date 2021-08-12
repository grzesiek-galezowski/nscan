using System;
using System.IO;
using AtmaFileSystem;
using AtmaFileSystem.IO;
using FluentAssertions;
using NScan.Adapters.Secondary.NotifyingSupport;
using NScan.SharedKernel.WritingProgramOutput.Ports;
using TddXt.NScan;
using static Bullseye.Targets;
using static SimpleExec.Command;


var nscan = "nscan";
var nscanConsole = "nscan.console";
var cakeNscan = "cake.nscan";
var solutionName = "NScan.sln";

// Define directories. (TODO: clean this up even more)
var configuration = "Release";
var root = AbsoluteFilePath.OfThisFile().ParentDirectory(2).Value;
var buildDir = root.AddDirectoryName("build").AddDirectoryName(configuration);
var publishDir = root.AddDirectoryName("publish");
var srcDir = root.AddDirectoryName("src");
var buildNScanDir = buildDir.AddDirectoryName(nscan).AddDirectoryName("netstandard2.1");
var buildNScanConsoleDir = buildDir.AddDirectoryName(nscanConsole).AddDirectoryName("netcoreapp3.1"); //bug
var buildCakeNScanDir = buildDir.AddDirectoryName(cakeNscan).AddDirectoryName("netstandard2.1");
var srcNetStandardDir = srcDir; //TODO inline
var slnNetStandard = srcNetStandardDir.AddFileName(solutionName);
var version = "0.72.0";
var nugetPath = root.AddDirectoryName("nuget");

//////////////////////////////////////////////////////////////////////
// HELPER FUNCTIONS
//////////////////////////////////////////////////////////////////////
void Build(AbsoluteDirectoryPath outputPath, DirectoryName workingDirectoryLastSegment)
{
  Run($"dotnet",
    "build " +
    $"-c {configuration} " +
    $"-p:VersionPrefix={version}",
    workingDirectory: (srcDir + workingDirectoryLastSegment).ToString());
}

void Test(AbsoluteDirectoryPath workingDirectory)
{
  Run($"dotnet",
    "test" +
    //$" --no-build" +
    $" -c {configuration}" +
    //$" -o {buildDir}" +
    $" -p:VersionPrefix={version}",
    workingDirectory: workingDirectory.ToString());
}

void Pack(AbsoluteDirectoryPath outputPath, AbsoluteDirectoryPath rootSourceDir, string projectName)
{
  Run("dotnet",
    $"pack" +
    $" -c {configuration}" +
    $" --include-symbols" +
    $" --no-build" +
    $" -p:SymbolPackageFormat=snupkg" +
    $" -p:VersionPrefix={version}" +
    $" -o {outputPath}",
    workingDirectory: rootSourceDir.AddDirectoryName(projectName).ToString());
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Target("Clean", () =>
{
  buildDir.Delete(true);
  nugetPath.Delete(true);
  Run($"dotnet",
    "clean " +
    $"-c {configuration} ",
    workingDirectory: srcNetStandardDir.ToString());
});

Target("RunPreviousNScan", () =>
{
  NScanMain.Run(
    new InputArgumentsDto
    {
      RulesFilePath = AbsoluteDirectoryPath.OfThisFile().AddFileName("nscan.config").AsAnyFilePath(),
      SolutionPath = slnNetStandard.AsAnyFilePath()
    },
    new ConsoleOutput(),
    new ConsoleSupport(Console.WriteLine)
  ).Should().Be(0);
});

Target("BuildNScan", DependsOn("RunPreviousNScan"), () =>
{
  Build(buildNScanDir, DirectoryName.Value("NScan.Main"));
});

Target("BuildNScanConsole", () =>
{
  Build(buildNScanConsoleDir, DirectoryName.Value("NScan.Console"));
});

Target("BuildCakeNScan", () =>
{
  Build(buildCakeNScanDir, DirectoryName.Value("Cake.NScan"));
});

Target("RunNScanUnitTests", DependsOn("BuildNScan"), () => //todo this runs integration tests as well
{
  var projectFiles = srcNetStandardDir.GetFiles("*Specification.csproj", SearchOption.AllDirectories);
  foreach (var file in projectFiles)
  {
    Test(file.ParentDirectory());
  }

  Test(srcNetStandardDir.AddDirectoryName("NScanSpecification.Component"));
});

Target("RunE2ETests", DependsOn("BuildNScanConsole", "RunNScanUnitTests"), () =>
{
  Test(srcNetStandardDir.AddDirectoryName("NScanSpecification.E2E"));
});

Target("PackNScan", DependsOn("BuildNScan", "RunE2ETests"), () =>
{
  Pack(nugetPath, srcNetStandardDir, "NScan.Main");
});

Target("PackNScanConsole", DependsOn("BuildNScanConsole", "RunE2ETests"), () =>
{
  Pack(nugetPath, srcNetStandardDir, "NScan.Console");
});

Target("PackCakeNScan", DependsOn("BuildCakeNScan", "RunE2ETests"), () =>
{
  Pack(nugetPath, srcNetStandardDir, "NScan.Console");
});

Target("Push", DependsOn("Clean", "PackNScan", "PackNScanConsole", "PackCakeNScan"), () =>
{
  foreach (var nupkgPath in nugetPath.GetFiles("*.nupkg"))
  {
    Run("dotnet", $"nuget push {nupkgPath}" +
                  $" --source https://api.nuget.org/v3/index.json");
  }
});

Target("default", DependsOn(
  "BuildNScan", 
  "BuildNScanConsole", 
  "BuildCakeNScan", 
  "RunNScanUnitTests", 
  "PackNScan", 
  "PackNScanConsole", 
  "PackCakeNScan"));

RunTargetsAndExit(args);

public class ConsoleOutput : INScanOutput
{
  public void WriteAnalysisReport(string analysisReport)
  {
    Console.WriteLine(analysisReport);
  }

  public void WriteVersion(string coreVersion)
  {
    Console.WriteLine(coreVersion);
  }
}

///////////////////////////////////////////////////////////////

//Task("DupFinder")
//    .Description("Find duplicates in the code")
//    .Does(() =>
//{
//    var settings = new DupFinderSettings() {
//        ShowStats = true,
//        ShowText = true,
//        OutputFile = $"dupfinder.xml",
//        ThrowExceptionOnFindingDuplicates = true,
//		ExcludePattern = new string[] { "**/*Specification/**/*Specification.cs" }
//    };
//    DupFinder(slnNetStandard, settings);
//});
